// Copyright 2024 The seacraft Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http:www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

package gorm

import (
	"context"

	"gorm.io/gorm"

	"github.com/seacraft/errors"
	"github.com/seacraft/pkg/db"
	"github.com/seacraft/pkg/message"
)

type Repository[TEntity any, TKey any] struct {
	DB         *gorm.DB
	unitOfWork db.IUnitOfWork
	db.IRepositoryKey[TEntity, TKey]
	DetailsFunc func(*gorm.DB) *gorm.DB
}

func NewRepository[TEntity any, TKey any](db *gorm.DB, detailsFunc func(*gorm.DB) *gorm.DB) *Repository[TEntity, TKey] {
	return &Repository[TEntity, TKey]{
		DB:          db,
		unitOfWork:  NewUnitOfWork(db),
		DetailsFunc: detailsFunc,
	}
}

func (r *Repository[TEntity, TKey]) Insert(ctx context.Context, entity *TEntity) (*TEntity, error) {
	if err := r.DB.Create(entity).Error; err != nil {
		return nil, err
	}

	return entity, nil
}

func (r *Repository[TEntity, TKey]) Update(ctx context.Context, entity *TEntity) (*TEntity, error) {
	if err := r.DB.Model(entity).Save(entity).Error; err != nil {
		return nil, err
	}

	return entity, nil
}

func (r *Repository[TEntity, TKey]) Delete(ctx context.Context, entity *TEntity) error {
	if err := r.DB.Delete(entity).Error; err != nil {
		return err
	}

	return nil
}

func (r *Repository[TEntity, TKey]) Get(
	ctx context.Context,
	predicate *db.Expression,
	includeDetails bool,
) (*TEntity, error) {
	dbCtx := r.Queryable(r.DB, predicate)
	if includeDetails {
		dbCtx = r.Queryable(r.WithDetails(), predicate)
	}
	var entity TEntity
	err := dbCtx.First(&entity).Error
	if err != nil && !errors.Is(err, gorm.ErrRecordNotFound) {
		return nil, errors.WithCode(db.ErrDatabase, err.Error())
	}

	return &entity, nil
}

func (r *Repository[TEntity, TKey]) GetList(
	ctx context.Context,
	predicate *db.Expression,
	order string,
	includeDetails bool,
) ([]*TEntity, error) {
	dbCtx := r.Queryable(r.DB, predicate)
	if includeDetails {
		dbCtx = r.Queryable(r.WithDetails(), predicate)
	}
	var entitys []*TEntity
	if err := dbCtx.Order(order).Find(&entitys).Error; err != nil {
		return nil, err
	}

	return entitys, nil
}

func (r *Repository[TEntity, TKey]) GetPageList(
	ctx context.Context,
	predicate *db.Expression,
	order string,
	req *message.PagedListRequest,
) ([]*TEntity, int64, error) {
	var (
		entitys []*TEntity
		total   int64
	)
	dbCtx := r.Queryable(r.DB, predicate).Order(order)
	err := dbCtx.Model(&entitys).Count(&total).Error
	if err != nil && !errors.Is(err, gorm.ErrRecordNotFound) {
		return nil, 0, errors.WithCode(db.ErrDatabase, err.Error())
	}
	err = dbCtx.Offset(req.Skip).Limit(req.PageSize).Find(&entitys).Error
	if err != nil && !errors.Is(err, gorm.ErrRecordNotFound) {
		return nil, 0, errors.WithCode(db.ErrDatabase, err.Error())
	}

	return entitys, total, nil
}

func (r *Repository[TEntity, TKey]) Exist(
	ctx context.Context,
	predicate *db.Expression,
	includeDetails bool,
) (bool, error) {
	dbCtx := r.DB
	if includeDetails {
		dbCtx = r.WithDetails()
	}
	var entity TEntity
	err := r.Queryable(dbCtx, predicate).First(&entity).Error
	if err != nil && !errors.Is(err, gorm.ErrRecordNotFound) {
		return false, errors.WithCode(db.ErrDatabase, err.Error())
	}

	return !errors.Is(err, gorm.ErrRecordNotFound), nil
}

func (r *Repository[TEntity, TKey]) GetById(ctx context.Context, id TKey, includeDetails bool) (*TEntity, error) {
	dbCtx := r.DB
	if includeDetails {
		dbCtx = r.WithDetails()
	}
	var entity TEntity
	err := dbCtx.Where("id = ?", id).First(&entity).Error
	if err != nil && !errors.Is(err, gorm.ErrRecordNotFound) {
		return nil, errors.WithCode(db.ErrDatabase, err.Error())
	}

	return &entity, nil
}

func (r *Repository[TEntity, TKey]) DeleteById(ctx context.Context, id TKey) error {
	var entity TEntity
	err := r.DB.Where("id = ?", id).Delete(&entity, id).Error
	if err != nil && !errors.Is(err, gorm.ErrRecordNotFound) {
		return errors.WithCode(db.ErrDatabase, err.Error())
	}

	return nil
}

func (r *Repository[TEntity, TKey]) Queryable(db *gorm.DB, predicate *db.Expression) *gorm.DB {
	if predicate == nil || predicate.List == nil {
		return db
	}
	for _, pair := range predicate.List {
		db = db.Where(pair.Key, pair.Value.([]interface{})...)
	}

	return db
}

func (r *Repository[TEntity, TKey]) WithDetails() *gorm.DB {
	return r.DB.Scopes(func(db *gorm.DB) *gorm.DB {
		return r.DetailsFunc(r.DB)
	})
}

func (r *Repository[TEntity, TKey]) UnitOfWork() db.IUnitOfWork {
	return r.unitOfWork
}
