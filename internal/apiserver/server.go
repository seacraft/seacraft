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

package apiserver

import (
	"github.com/seacraft/internal/apiserver/config"
	"github.com/seacraft/internal/apiserver/repository"
	"github.com/seacraft/internal/apiserver/repository/postgresql"
	genericoptions "github.com/seacraft/internal/pkg/options"
	genericapiserver "github.com/seacraft/internal/pkg/server"
	"github.com/seacraft/pkg/log"
	"github.com/seacraft/shutdown"
)

type apiServer struct {
	gs               *shutdown.GracefulShutdown
	genericAPIServer *genericapiserver.GenericAPIServer
}
type preparedAPIServer struct {
	*apiServer
}

func createAPIServer(cfg *config.Config) (*apiServer, error) {
	gs := shutdown.New()
	gs.AddShutdownManager(shutdown.NewPosixSignalManager())

	genericConfig, err := buildGenericConfig(cfg)
	if err != nil {
		return nil, err
	}
	extraConfig, err := buildExtraConfig(cfg)
	if err != nil {
		return nil, err
	}
	genericServer, err := genericConfig.Complete().New()
	if err != nil {
		return nil, err
	}
	err = extraConfig.complete().init()
	if err != nil {
		return nil, err
	}
	server := &apiServer{
		gs:               gs,
		genericAPIServer: genericServer,
	}

	return server, nil
}

func (s *apiServer) PrepareRun() preparedAPIServer {
	initRouter(s.genericAPIServer.Engine)

	s.gs.AddShutdownCallback(shutdown.ShutdownFunc(func(string) error {
		postgresqlStore, _ := postgresql.GetPostgreSQLFactoryOr(nil)
		if postgresqlStore != nil {
			_ = postgresqlStore.Close()
		}
		s.genericAPIServer.Close()

		return nil
	}))

	return preparedAPIServer{s}
}

func (s preparedAPIServer) Run() error {
	// start shutdown managers
	if err := s.gs.Start(); err != nil {
		log.Fatalf("start shutdown manager failed: %s", err.Error())
	}

	return s.genericAPIServer.Run()
}

type completedExtraConfig struct {
	*ExtraConfig
}

// init  instance.
func (c *completedExtraConfig) init() error {
	storeIns, _ := postgresql.GetPostgreSQLFactoryOr(c.postgresSQLOptions)
	repository.SetClient(storeIns)

	return nil
}

func buildGenericConfig(cfg *config.Config) (genericConfig *genericapiserver.Config, lastErr error) {
	genericConfig = genericapiserver.NewConfig()
	if lastErr = cfg.GenericServerRunOptions.ApplyTo(genericConfig); lastErr != nil {
		return
	}

	if lastErr = cfg.FeatureOptions.ApplyTo(genericConfig); lastErr != nil {
		return
	}

	if lastErr = cfg.SecureServing.ApplyTo(genericConfig); lastErr != nil {
		return
	}

	if lastErr = cfg.InsecureServing.ApplyTo(genericConfig); lastErr != nil {
		return
	}

	return
}

// ExtraConfig defines extra configuration for the sea-apiserver.
type ExtraConfig struct {
	postgresSQLOptions *genericoptions.PostgresSQLOptions
}

func (c *ExtraConfig) complete() *completedExtraConfig {
	return &completedExtraConfig{c}
}

// nolint: unparam
func buildExtraConfig(cfg *config.Config) (*ExtraConfig, error) {
	return &ExtraConfig{
		postgresSQLOptions: cfg.PostgresSQLOptions,
	}, nil
}
