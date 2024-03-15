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

package db

import (
	"context"

	"github.com/seacraft/pkg/message"
)

type IRepository interface{}

type IRepositoryEntity[TEntity any] interface {
	IRepository
	UnitOfWork() IUnitOfWork

	Insert(ctx context.Context, entity *TEntity) (*TEntity, error)

	Update(ctx context.Context, entity *TEntity) (*TEntity, error)

	Delete(ctx context.Context, entity *TEntity) error

	Get(ctx context.Context, predicate *Expression, includeDetails bool) (*TEntity, error)

	GetList(ctx context.Context, predicate *Expression, order string, includeDetails bool) ([]*TEntity, error)

	GetPageList(
		ctx context.Context,
		predicate *Expression,
		order string,
		req *message.PagedListRequest,
	) ([]*TEntity, int64, error)

	Exist(ctx context.Context, predicate *Expression, includeDetails bool) (bool, error)
}

type IRepositoryKey[TEntity any, TKey any] interface {
	IRepositoryEntity[TEntity]

	UnitOfWork() IUnitOfWork

	GetById(ctx context.Context, id TKey, includeDetails bool) (*TEntity, error)

	DeleteById(ctx context.Context, id TKey) error
}
