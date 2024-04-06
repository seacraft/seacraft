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

package v1

import "github.com/seacraft/internal/apiserver/repository"

// Service defines functions used to return resource interface.
type Service interface {
	AppTemplates() AppTemplateSrv
	AppService() AppServiceSrv
}

type service struct {
	repo repository.Factory
}

// NewService returns Service interface.
func NewService(repo repository.Factory) Service {
	return &service{
		repo: repo,
	}
}

func (s *service) AppTemplates() AppTemplateSrv {
	return newAppTemplates(s)
}

func (s *service) AppService() AppServiceSrv {
	return newAppServices(s)
}
