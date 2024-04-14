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

package appservice

import (
	"github.com/gin-gonic/gin"

	"github.com/seacraft/component-base/pkg/core"
	metav1 "github.com/seacraft/component-base/pkg/meta/v1"
	"github.com/seacraft/errors"
	msg "github.com/seacraft/internal/apiserver/service/message/v1"
	"github.com/seacraft/internal/pkg/code"
	"github.com/seacraft/pkg/log"
)

// Update update a  application service  by the id  identifier.
func (a *AppServiceController) Update(c *gin.Context) {
	log.L(c).Info("update app service function called.")
	var r msg.UpdateAppServiceRequest
	if err := c.ShouldBindJSON(&r); err != nil {
		core.WriteResponse(c, errors.WithCode(code.ErrBind, err.Error()), nil)
		return
	}
	id, ok := a.ParseUint64(c, c.Param("id"))
	if !ok {
		return
	}
	r.Id = id
	if err := a.srv.AppService().Update(c, &r, metav1.UpdateOptions{}); err != nil {
		core.WriteResponse(c, err, nil)
		return
	}
	core.WriteResponse(c, nil, r)
}
