package postgresql

import (
	"fmt"
	"github.com/seacraft/errors"
	"github.com/seacraft/internal/apiserver/store"
	"github.com/seacraft/internal/pkg/logger"
	genericoptions "github.com/seacraft/internal/pkg/options"
	"github.com/seacraft/pkg/db"
	"gorm.io/gorm"
	"sync"
)

type datastore struct {
	db *gorm.DB
}

func (ds *datastore) Close() error {
	db, err := ds.db.DB()
	if err != nil {
		return errors.Wrap(err, "get gorm db instance failed")
	}

	return db.Close()
}

var (
	postgresqlFactory store.Factory
	once              sync.Once
)

// GetPostgreSQLFactoryOr create postgresql factory with the given config.
func GetPostgreSQLFactoryOr(opts *genericoptions.PostgresSQLOptions) (store.Factory, error) {
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
		err := migrateDatabase(dbIns)
		if err != nil {
			return
		}
	})

	if postgresqlFactory == nil || err != nil {
		return nil, fmt.Errorf("failed to get postgresql store fatory, postgresqlFactory: %+v, error: %w", postgresqlFactory, err)
	}

	return postgresqlFactory, nil
}

// cleanDatabase tear downs the database tables.
//
//nolint:unused // may be reused in the feature, or just show a migrate usage.
func cleanDatabase(db *gorm.DB) error {

	return nil
}

// migrateDatabase run auto migration for given models, will only add missing fields,
// won't delete/change current data.
//
//nolint:unused // may be reused in the feature, or just show a migrate usage.
func migrateDatabase(db *gorm.DB) error {

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
