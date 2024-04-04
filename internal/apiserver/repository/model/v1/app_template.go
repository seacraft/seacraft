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

	metav1 "github.com/seacraft/component-base/pkg/meta/v1"
	"github.com/seacraft/component-base/pkg/util/idutil"
)

// AppTemplate represents a secret restful resource.
// It is also used as gorm model.
type AppTemplate struct {
	// Standard object's metadata.
	metav1.ObjectMeta `                 json:"metadata,omitempty"`
	// Required: true
	Name            string           `json:"name"               gorm:"column:name;not null"        validate:"omitempty"`
	Description     string           `json:"description"        gorm:"column:description;not null" validate:"omitempty"`
	AppServiceItems []AppServiceItem `json:"app_service_items"  gorm:"foreignKey:AppTemplateId"`
}

// AppTemplateList is the whole list of all app template which have been stored in stroage.
type AppTemplateList struct {
	// Standard list metadata.
	// +optional
	metav1.ListMeta `json:",inline"`

	Items []*AppTemplate `json:"items"`
}

// TableName maps to postgresql table name.
func (u *AppTemplate) TableName() string {
	return "app_template"
}

// AfterCreate run after create database record.
func (u *AppTemplate) AfterCreate(tx *gorm.DB) error {
	u.InstanceID = idutil.GetInstanceID(u.ID, "app_template-")

	return tx.Save(u).Error
}
