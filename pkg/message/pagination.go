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

package message

const (
	defaultPagesize  = 10
	defaultPageindex = 1
)

type PagedListResult struct {
	// Example:10
	PageIndex int `json:"page_index"`
	// Example:1
	PageSize       int   `json:"page_size"`
	PageCount      int   `json:"page_count"`
	TotalItemCount int64 `json:"total_item_count"`
}

func (r *PagedListResult) SetTotal(total int64) {
	if total < 1 {
		return
	}
	r.TotalItemCount = total
	if r.PageSize <= 1 {
		r.PageCount = int(total)
		return
	}
	r.PageCount = int(total / int64(r.PageSize))
	if total%int64(r.PageSize) > 0 {
		r.PageCount++
	}
}

type PagedListRequest struct {
	PageIndex int `json:"page_index" form:"page_index"`
	PageSize  int `json:"page_size" form:"page_size"`
	Skip      int `json:"skip" form:"skip"`
}

func (r *PagedListRequest) InitPage() {
	if r.PageSize <= 0 {
		r.PageSize = defaultPagesize
	}
	if r.PageIndex <= 0 {
		r.PageIndex = defaultPageindex
	}
	r.Skip = (r.PageIndex - 1) * r.PageSize

}
