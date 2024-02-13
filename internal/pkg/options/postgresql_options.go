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

package options

import (
	"time"

	"github.com/spf13/pflag"
	"gorm.io/gorm"

	"github.com/seacraft/pkg/db"
)

type PostgresSQLOptions struct {
	Server                string        `json:"server,omitempty"                   mapstructure:"server"`
	Port                  int           `json:"port,omitempty"                     mapstructure:"port"`
	Username              string        `json:"username,omitempty"                 mapstructure:"username"`
	Password              string        `json:"-"                                  mapstructure:"password"`
	Database              string        `json:"database"                           mapstructure:"database"`
	MaxIdleConnections    int           `json:"max-idle-connections,omitempty"     mapstructure:"max-idle-connections"`
	MaxOpenConnections    int           `json:"max-open-connections,omitempty"     mapstructure:"max-open-connections"`
	MaxConnectionLifeTime time.Duration `json:"max-connection-life-time,omitempty" mapstructure:"max-connection-life-time"`
	LogLevel              int           `json:"log-level"                          mapstructure:"log-level"`
}

// NewPostgresSQLOptions create a `zero` value instance.
func NewPostgresSQLOptions() *PostgresSQLOptions {
	return &PostgresSQLOptions{
		Server:                "127.0.0.1",
		Port:                  5432,
		Username:              "",
		Password:              "",
		Database:              "",
		MaxIdleConnections:    100,
		MaxOpenConnections:    100,
		MaxConnectionLifeTime: time.Duration(10) * time.Second,
		LogLevel:              1, // Silent
	}
}

// Validate verifies flags passed to PostgresSQLOptions.
func (o *PostgresSQLOptions) Validate() []error {
	errs := []error{}

	return errs
}

// AddFlags adds flags related to PostgresSQL storage for a specific APIServer to the specified FlagSet.
func (o *PostgresSQLOptions) AddFlags(fs *pflag.FlagSet) {
	fs.StringVar(&o.Server, "postgres.server", o.Server, ""+
		"Postgres appsvc  address. If left blank, the following related postgres options will be ignored.")

	fs.IntVar(&o.Port, "postgres.port", o.Port, ""+
		"Postgres appsvc port")

	fs.StringVar(&o.Username, "postgres.username", o.Username, ""+
		"Username for access to postgres appsvc.")

	fs.StringVar(&o.Password, "postgres.password", o.Password, ""+
		"Password for access to postgres, should be used pair with password.")

	fs.StringVar(&o.Database, "postgres.database", o.Database, ""+
		"Database name for the server to use.")

	fs.IntVar(&o.MaxIdleConnections, "postgres.max-idle-connections", o.MaxOpenConnections, ""+
		"Maximum idle connections allowed to connect to PostgresSQL.")

	fs.IntVar(&o.MaxOpenConnections, "postgres.max-open-connections", o.MaxOpenConnections, ""+
		"Maximum open connections allowed to connect to postgres.")

	fs.DurationVar(&o.MaxConnectionLifeTime, "postgres.max-connection-life-time", o.MaxConnectionLifeTime, ""+
		"Maximum connection life time allowed to connect to postgres.")

	fs.IntVar(&o.LogLevel, "postgres.log-mode", o.LogLevel, ""+
		"Specify gorm log level.")
}

// NewClient create PostgresSQL store with the given config.
func (o *PostgresSQLOptions) NewClient() (*gorm.DB, error) {
	opts := &db.Options{
		Server:                o.Server,
		Port:                  o.Port,
		Username:              o.Username,
		Password:              o.Password,
		Database:              o.Database,
		MaxIdleConnections:    o.MaxIdleConnections,
		MaxOpenConnections:    o.MaxOpenConnections,
		MaxConnectionLifeTime: o.MaxConnectionLifeTime,
		LogLevel:              o.LogLevel,
	}

	return db.New(opts)
}
