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
	"github.com/seacraft/pkg/log"
)

// Get return application service by the application service id identifier.
func (a *AppServiceController) Get(c *gin.Context) {
	log.L(c).Info("get app service function called.")
	id, ok := a.ParseUint64(c, c.Param("id"))
	if !ok {
		return
	}
	svc, err := a.srv.AppService().Get(c, id, metav1.GetOptions{})
	if err != nil {
		core.WriteResponse(c, err, nil)
	}
	core.WriteResponse(c, nil, svc)
}
