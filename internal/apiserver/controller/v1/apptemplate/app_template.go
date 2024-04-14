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

package apptemplate

import (
	v1 "github.com/seacraft/internal/apiserver/controller/v1"
	"github.com/seacraft/internal/apiserver/repository"
	srvv1 "github.com/seacraft/internal/apiserver/service/v1"
)

// AppTemplateController create a application handler used to handle request for application template resource.
type AppTemplateController struct {
	v1.BaseController
	srv srvv1.Service
}

// NewAppTemplateController creates a application template handler.
func NewAppTemplateController(repo repository.Factory) *AppTemplateController {
	return &AppTemplateController{
		srv: srvv1.NewService(repo),
	}
}
