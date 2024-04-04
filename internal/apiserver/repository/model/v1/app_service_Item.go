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
	"gorm.io/gorm"
)

// AppServiceItem application template service item, It is also used as gorm model.
type AppServiceItem struct {
	ID uint64 `json:"id,omitempty" gorm:"primary_key;AUTO_INCREMENT;column:id"`

	AppServiceId  uint64 `json:"app_service_id"  gorm:"type:varchar(36);column:application_service_id;not null"`
	AppTemplateId uint64 `json:"app_template_id" gorm:"type:varchar(36);column:application_template_id;not null"`
}

// NewAppServiceItem create a new application template service item.
func NewAppServiceItem(appTemplateId, appServiceId uint64) AppServiceItem {
	return AppServiceItem{
		AppServiceId:  appServiceId,
		AppTemplateId: appTemplateId,
	}
}

// TableName maps to postgresql table name.
func (u *AppServiceItem) TableName() string {
	return "app_service_item"
}

// AfterCreate run after create database record.
func (u *AppServiceItem) AfterCreate(tx *gorm.DB) error {
	return tx.Save(u).Error
}
