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
	"fmt"

	"github.com/google/uuid"
	"gorm.io/gorm"

	"github.com/seacraft/errors"
	"github.com/seacraft/pkg/db"
)

type DbContextTx struct {
	db            *gorm.DB
	TransactionId string
}

func newDbContextTx(db *gorm.DB) *DbContextTx {
	id, _ := uuid.NewUUID()

	return &DbContextTx{
		db:            db,
		TransactionId: id.String(),
	}
}

func (d *DbContextTx) DbContext() any {
	return d.db
}

func (d *DbContextTx) Commit() error {
	return d.db.Commit().Error
}

func (d *DbContextTx) Rollback() {
	d.db.Rollback()
}

func (d *DbContextTx) TxId() string {
	return d.TransactionId
}

type UnitOfWork struct {
	db *gorm.DB
	db.IUnitOfWork
	contextTx *DbContextTx
}

func NewUnitOfWork(db *gorm.DB) db.IUnitOfWork {
	return &UnitOfWork{
		db: db,
	}
}

func (u *UnitOfWork) Current() db.IDbContextTx {
	return u.contextTx
}

func (u *UnitOfWork) IsHasActive() bool {
	return u.contextTx != nil
}

func (u *UnitOfWork) Begin() db.IDbContextTx {
	if u.contextTx != nil {
		return nil
	}
	u.contextTx = newDbContextTx(u.db.Begin())
	return u.contextTx
}

func (u *UnitOfWork) SaveChange(entity ...any) (bool, error) {
	tx := u.db.Begin()
	defer func() {
		if r := recover(); r != nil {
			tx.Rollback()
		}
	}()
	for _, e := range entity {
		if e == nil {
			continue
		}
		if list, ok := e.([]interface{}); ok {
			for _, ent := range list {
				err := tx.Save(ent).Error
				if err != nil {
					tx.Rollback()
					return false, err
				}
			}
		} else {
			err := tx.Save(e).Error
			if err != nil {
				tx.Rollback()
				return false, err
			}
		}
	}
	return true, nil
}

func (u *UnitOfWork) Rollback() {
	if u.contextTx == nil {
		return
	}
	u.contextTx.Rollback()
	u.contextTx = nil
}

func (u *UnitOfWork) Commit(tx db.IDbContextTx, entity ...any) error {
	if tx == nil {
		_, err := u.SaveChange(entity)
		if err != nil {
			u.Rollback()
			return err
		}
		return nil
	}
	if tx.TxId() != u.contextTx.TxId() {
		return errors.New(fmt.Sprintf("Transaction %s is not current", tx.TxId()))
	}
	defer func() {
		if r := recover(); r != nil {
			tx.Rollback()
		}
	}()
	if entity != nil {
		err := u.save(entity)
		if err != nil {
			u.Rollback()
			return err
		}
	}
	if err := tx.Commit(); err != nil {
		u.contextTx = nil
		return err
	}
	return nil
}

func (u *UnitOfWork) save(entity ...any) error {
	for _, e := range entity {
		if e == nil {
			continue
		}
		if list, ok := e.([]interface{}); ok {
			for _, ent := range list {
				err := u.contextTx.db.Save(ent).Error
				if err != nil {
					return err
				}
			}
		} else {
			err := u.contextTx.db.Save(e).Error
			if err != nil {
				return err
			}
		}
	}
	return nil
}
