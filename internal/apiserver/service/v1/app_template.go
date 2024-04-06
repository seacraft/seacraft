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
	"context"

	metav1 "github.com/seacraft/component-base/pkg/meta/v1"
	"github.com/seacraft/errors"
	"github.com/seacraft/internal/apiserver/repository"
	v1 "github.com/seacraft/internal/apiserver/repository/model/v1"
	"github.com/seacraft/pkg/db"
)

type AppTemplateSrv interface {
	Create(ctx context.Context, appTemplate *v1.AppTemplate, opts metav1.CreateOptions) error
	Update(ctx context.Context, appTemplate *v1.AppTemplate, ops metav1.UpdateOptions) error
	Delete(ctx context.Context, id uint64, ops metav1.DeleteOptions) error
	Get(ctx context.Context, id uint64, ops metav1.GetOptions) (*v1.AppTemplate, error)
	List(ctx context.Context, ops metav1.ListOptions) (*v1.AppTemplateList, error)
}

var _ AppTemplateSrv = (*appTemplateService)(nil)

type appTemplateService struct {
	repo repository.Factory
}

func newAppTemplates(srv *service) *appTemplateService {
	return &appTemplateService{repo: srv.repo}
}

func (a *appTemplateService) Create(ctx context.Context, appTemplate *v1.AppTemplate, opts metav1.CreateOptions) error {
	if _, err := a.repo.AppTemplates().Insert(ctx, appTemplate); err != nil {
		return errors.WithCode(db.ErrDatabase, err.Error())
	}
	return nil
}

func (a *appTemplateService) Update(ctx context.Context, appTemplate *v1.AppTemplate, ops metav1.UpdateOptions) error {
	if _, err := a.repo.AppTemplates().Update(ctx, appTemplate); err != nil {
		return errors.WithCode(db.ErrDatabase, err.Error())
	}
	return nil
}

func (a *appTemplateService) Delete(ctx context.Context, id uint64, ops metav1.DeleteOptions) error {
	if err := a.repo.AppTemplates().DeleteById(ctx, id); err != nil {
		return errors.WithCode(db.ErrDatabase, err.Error())
	}
	return nil
}

func (a *appTemplateService) Get(ctx context.Context, id uint64, ops metav1.GetOptions) (*v1.AppTemplate, error) {
	template, err := a.repo.AppTemplates().GetById(ctx, id, false)
	if err != nil {
		return nil, err
	}
	return template, nil
}

func (a *appTemplateService) List(ctx context.Context, ops metav1.ListOptions) (*v1.AppTemplateList, error) {
	exp := db.NewExpression()
	templates, err := a.repo.AppTemplates().GetList(ctx, exp, "", false)
	if err != nil {
		return nil, errors.WithCode(db.ErrDatabase, err.Error())
	}

	return &v1.AppTemplateList{
		ListMeta: metav1.ListMeta{
			TotalCount: int64(len(templates)),
		},
		Items: templates,
	}, nil
}
