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

package postgresql

import (
	"gorm.io/gorm"

	v1 "github.com/seacraft/internal/apiserver/repository/model/v1"
	repo "github.com/seacraft/pkg/db/gorm"
)

type appTemplateRepository struct {
	db *gorm.DB
	*repo.Repository[v1.AppTemplate, uint64]
}

func newAppTemplates(ds *datastore) *appTemplateRepository {
	return &appTemplateRepository{
		db: ds.db,
		Repository: repo.NewRepository[v1.AppTemplate, uint64](ds.db.Scopes(FilterDelete), func(db *gorm.DB) *gorm.DB {
			return db.Preload("AppServiceItems")
		}),
	}
}
