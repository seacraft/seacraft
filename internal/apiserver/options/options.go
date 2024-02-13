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

// Package options contains flags and options for initializing an apiserver
package options

import (
	cliflag "github.com/seacraft/component-base/pkg/cli/flag"
	"github.com/seacraft/component-base/pkg/json"
	"github.com/seacraft/component-base/pkg/util/idutil"

	genericoptions "github.com/seacraft/internal/pkg/options"
	"github.com/seacraft/internal/pkg/server"
	"github.com/seacraft/pkg/log"
)

// Options runs an sea api server.
type Options struct {
	GenericServerRunOptions *genericoptions.ServerRunOptions `json:"server"   mapstructure:"server"`
	// GRPCOptions             *genericoptions.GRPCOptions            `json:"grpc"     mapstructure:"grpc"`
	InsecureServing    *genericoptions.InsecureServingOptions `json:"insecure" mapstructure:"insecure"`
	SecureServing      *genericoptions.SecureServingOptions   `json:"secure"   mapstructure:"secure"`
	PostgresSQLOptions *genericoptions.PostgresSQLOptions     `json:"postgres" mapstructure:"postgres"`
	// RedisOptions    *genericoptions.RedisOptions           `json:"redis"    mapstructure:"redis"`
	JwtOptions     *genericoptions.JwtOptions     `json:"jwt"      mapstructure:"jwt"`
	Log            *log.Options                   `json:"log"      mapstructure:"log"`
	FeatureOptions *genericoptions.FeatureOptions `json:"feature"  mapstructure:"feature"`
}

// NewOptions creates a new Options object with default parameters.
func NewOptions() *Options {
	o := Options{
		GenericServerRunOptions: genericoptions.NewServerRunOptions(),
		// GRPCOptions:             genericoptions.NewGRPCOptions(),
		InsecureServing:    genericoptions.NewInsecureServingOptions(),
		SecureServing:      genericoptions.NewSecureServingOptions(),
		PostgresSQLOptions: genericoptions.NewPostgresSQLOptions(),
		// RedisOptions:            genericoptions.NewRedisOptions(),
		JwtOptions:     genericoptions.NewJwtOptions(),
		Log:            log.NewOptions(),
		FeatureOptions: genericoptions.NewFeatureOptions(),
	}

	return &o
}

// ApplyTo applies the run options to the method receiver and returns self.
func (o *Options) ApplyTo(c *server.Config) error {
	return nil
}

// Flags returns flags for a specific APIServer by section name.
func (o *Options) Flags() (fss cliflag.NamedFlagSets) {
	o.GenericServerRunOptions.AddFlags(fss.FlagSet("generic"))
	o.JwtOptions.AddFlags(fss.FlagSet("jwt"))
	// o.GRPCOptions.AddFlags(fss.FlagSet("grpc"))
	o.PostgresSQLOptions.AddFlags(fss.FlagSet("postgres"))
	// o.RedisOptions.AddFlags(fss.FlagSet("redis"))
	o.FeatureOptions.AddFlags(fss.FlagSet("features"))
	o.InsecureServing.AddFlags(fss.FlagSet("insecure serving"))
	o.SecureServing.AddFlags(fss.FlagSet("secure serving"))
	o.Log.AddFlags(fss.FlagSet("logs"))

	return fss
}

func (o *Options) String() string {
	data, _ := json.Marshal(o)

	return string(data)
}

// Complete set default Options.
func (o *Options) Complete() error {
	if o.JwtOptions.Key == "" {
		o.JwtOptions.Key = idutil.NewSecretKey()
	}

	return o.SecureServing.Complete()
}
