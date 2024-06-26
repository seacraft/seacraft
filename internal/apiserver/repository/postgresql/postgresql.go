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

package postgresql

import (
	"fmt"
	"sync"

	"github.com/seacraft/internal/apiserver/repository"
	v1 "github.com/seacraft/internal/apiserver/repository/model/v1"
	repo "github.com/seacraft/pkg/db/gorm"

	"gorm.io/gorm"

	"github.com/seacraft/errors"
	"github.com/seacraft/internal/pkg/logger"
	genericoptions "github.com/seacraft/internal/pkg/options"
	"github.com/seacraft/pkg/db"
)

type datastore struct {
	db *gorm.DB
}

func (ds *datastore) AppServices() repository.AppServiceRepository {
	return newAppServices(ds)
}

func (ds *datastore) AppTemplates() repository.AppTemplateRepository {
	return newAppTemplates(ds)
}

func (ds *datastore) Close() error {
	db, err := ds.db.DB()
	if err != nil {
		return errors.Wrap(err, "get gorm db instance failed")
	}

	return db.Close()
}

func (ds *datastore) Begin() (db.IUnitOfWork, repository.Factory) {
	unitOfWork := repo.NewUnitOfWork(ds.db)
	_ = unitOfWork.Begin()
	return unitOfWork, &datastore{
		unitOfWork.Current().DbContext().(*gorm.DB),
	}
}

var (
	postgresqlFactory repository.Factory
	once              sync.Once
)

// GetPostgreSQLFactoryOr create postgresql factory with the given config.
func GetPostgreSQLFactoryOr(opts *genericoptions.PostgresSQLOptions) (repository.Factory, error) {
	if opts == nil && postgresqlFactory == nil {
		return nil, fmt.Errorf("failed to get postgresql store fatory")
	}
	var err error
	var dbIns *gorm.DB
	once.Do(func() {
		options := &db.Options{
			Server:                opts.Server,
			Port:                  opts.Port,
			Username:              opts.Username,
			Password:              opts.Password,
			Database:              opts.Database,
			MaxIdleConnections:    opts.MaxIdleConnections,
			MaxOpenConnections:    opts.MaxOpenConnections,
			MaxConnectionLifeTime: opts.MaxConnectionLifeTime,
			LogLevel:              opts.LogLevel,
			Logger:                logger.New(opts.LogLevel),
		}
		dbIns, err = db.New(options)
		postgresqlFactory = &datastore{dbIns}
		_ = migrateDatabase(dbIns)
	})

	if postgresqlFactory == nil || err != nil {
		return nil, fmt.Errorf(
			"failed to get postgresql store fatory, postgresqlFactory: %+v, error: %w",
			postgresqlFactory,
			err,
		)
	}

	return postgresqlFactory, nil
}

// cleanDatabase tear downs the database tables.
//
//nolint:unused // may be reused in the feature, or just show a migrate usage.
func cleanDatabase(db *gorm.DB) error {
	if err := db.Migrator().DropTable(&v1.AppTemplate{}); err != nil {
		return errors.Wrap(err, "drop  appTemplate failed")
	}
	if err := db.Migrator().DropTable(&v1.AppServiceItem{}); err != nil {
		return errors.Wrap(err, "drop  appServiceItem failed")
	}
	if err := db.Migrator().DropTable(&v1.AppService{}); err != nil {
		return errors.Wrap(err, "drop  appService failed")
	}

	return nil
}

// migrateDatabase run auto migration for given models, will only add missing fields,
// won't delete/change current data.
//

func migrateDatabase(db *gorm.DB) error {
	if err := db.AutoMigrate(&v1.AppTemplate{}); err != nil {
		return errors.Wrap(err, "migrate AppTemplate model failed")
	}
	if err := db.AutoMigrate(&v1.AppServiceItem{}); err != nil {
		return errors.Wrap(err, "migrate AppServiceItem model failed")
	}
	if err := db.AutoMigrate(&v1.AppService{}); err != nil {
		return errors.Wrap(err, "migrate AppService model failed")
	}
	return nil
}

// resetDatabase resets the database tables.
//
//nolint:unused,deadcode // may be reused in the feature, or just show a migrate usage.
func resetDatabase(db *gorm.DB) error {
	if err := cleanDatabase(db); err != nil {
		return err
	}
	if err := migrateDatabase(db); err != nil {
		return err
	}

	return nil
}

func FilterDelete(db *gorm.DB) *gorm.DB {
	return db.Where("is_deleted = ?", false)
}
