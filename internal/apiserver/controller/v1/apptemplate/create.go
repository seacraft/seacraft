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
	"github.com/gin-gonic/gin"

	metav1 "github.com/seacraft/component-base/pkg/meta/v1"
	msg "github.com/seacraft/internal/apiserver/service/message/v1"

	"github.com/seacraft/component-base/pkg/core"
	"github.com/seacraft/errors"
	"github.com/seacraft/internal/pkg/code"
	"github.com/seacraft/pkg/log"
)

// Create add new  app template pairs to the storage.
func (a *AppTemplateController) Create(c *gin.Context) {
	log.L(c).Info("create app template function called. ")
	var r msg.CreateAppTemplateRequest
	if err := c.ShouldBindJSON(&r); err != nil {
		core.WriteResponse(c, errors.WithCode(code.ErrBind, err.Error()), nil)
		return
	}
	if err := a.srv.AppTemplates().Create(c, &r, metav1.CreateOptions{}); err != nil {
		core.WriteResponse(c, err, nil)
		return
	}
	core.WriteResponse(c, nil, r)
}
