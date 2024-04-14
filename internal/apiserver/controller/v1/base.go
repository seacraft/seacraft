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

import (
	"strconv"

	"github.com/gin-gonic/gin"

	"github.com/seacraft/component-base/pkg/core"
	"github.com/seacraft/errors"
	"github.com/seacraft/internal/pkg/code"
)

type BaseController struct{}

// ParseUint64 Parse the Id on the URL and convert it to uint64 bits.
func (b *BaseController) ParseUint64(c *gin.Context, id string) (uint64, bool) {
	uid, err := strconv.ParseUint(id, 10, 64)
	if err != nil {
		core.WriteResponse(c, errors.WithCode(code.ErrBind, err.Error()), nil)
		return 0, false
	}
	return uid, true
}
