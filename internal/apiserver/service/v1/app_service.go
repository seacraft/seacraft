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
	"fmt"

	"github.com/seacraft/internal/pkg/code"
	"github.com/seacraft/pkg/util"

	metav1 "github.com/seacraft/component-base/pkg/meta/v1"
	"github.com/seacraft/errors"
	"github.com/seacraft/internal/apiserver/repository"
	v1 "github.com/seacraft/internal/apiserver/repository/model/v1"
	msg "github.com/seacraft/internal/apiserver/service/message/v1"
	"github.com/seacraft/pkg/db"
)

type AppServiceSrv interface {
	Create(ctx context.Context, req *msg.CreateAppServiceRequest, opts metav1.CreateOptions) error
	Update(ctx context.Context, req *msg.UpdateAppServiceRequest, ops metav1.UpdateOptions) error
	Delete(ctx context.Context, id uint64, ops metav1.DeleteOptions) error
	Get(ctx context.Context, id uint64, ops metav1.GetOptions) (*v1.AppService, error)
	List(ctx context.Context, ops metav1.ListOptions) (*v1.AppServiceList, error)
}

var _ AppServiceSrv = (*appSvcService)(nil)

type appSvcService struct {
	repo repository.Factory
}

func newAppServices(srv *service) *appSvcService {
	return &appSvcService{repo: srv.repo}
}

func (a *appSvcService) Create(ctx context.Context, req *msg.CreateAppServiceRequest, opts metav1.CreateOptions) error {
	exp := db.NewExpression()
	if util.Trim(req.Name) != "" {
		exp.And("name = ?", req.Name)
	}
	exit, _ := a.repo.AppServices().Exist(ctx, exp, false)
	if exit {
		return errors.WithCode(
			code.ErrValidation,
			fmt.Sprintf("%s The application service name already exists! Please change another name!", req.Name),
		)
	}
	svc := &v1.AppService{
		Name:        req.Name,
		Description: req.Description,
	}
	if _, err := a.repo.AppServices().Insert(ctx, svc); err != nil {
		return errors.WithCode(db.ErrDatabase, err.Error())
	}
	return nil
}

func (a *appSvcService) Update(ctx context.Context, req *msg.UpdateAppServiceRequest, ops metav1.UpdateOptions) error {
	svc, err := a.repo.AppServices().GetById(ctx, req.Id, false)
	if err != nil {
		return errors.WithCode(code.ErrPageNotFound, err.Error())
	}
	exp := db.NewExpression()
	if util.Trim(req.Name) != "" {
		exp.And("name = ?", req.Name)
		exp.And("if not in (?)", req.Id)
	}
	exit, _ := a.repo.AppServices().Exist(ctx, exp, false)
	if exit {
		return errors.WithCode(
			code.ErrValidation,
			fmt.Sprintf("%s The application service name already exists! Please change another name!", req.Name),
		)
	}
	svc.Description = req.Description
	svc.Name = req.Name
	if _, err := a.repo.AppServices().Update(ctx, svc); err != nil {
		return errors.WithCode(db.ErrDatabase, err.Error())
	}
	return nil
}

func (a *appSvcService) Delete(ctx context.Context, id uint64, ops metav1.DeleteOptions) error {
	if err := a.repo.AppServices().DeleteById(ctx, id); err != nil {
		return errors.WithCode(db.ErrDatabase, err.Error())
	}
	return nil
}

func (a *appSvcService) Get(ctx context.Context, id uint64, ops metav1.GetOptions) (*v1.AppService, error) {
	appService, err := a.repo.AppServices().GetById(ctx, id, false)
	if err != nil {
		return nil, err
	}
	return appService, nil
}

func (a *appSvcService) List(ctx context.Context, ops metav1.ListOptions) (*v1.AppServiceList, error) {
	exp := db.NewExpression()
	appServices, err := a.repo.AppServices().GetList(ctx, exp, "", false)
	if err != nil {
		return nil, errors.WithCode(db.ErrDatabase, err.Error())
	}
	return &v1.AppServiceList{
		ListMeta: metav1.ListMeta{
			TotalCount: int64(len(appServices)),
		},
		Items: appServices,
	}, nil
}
