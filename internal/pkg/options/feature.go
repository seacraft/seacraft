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

package options

import (
	"github.com/spf13/pflag"

	"github.com/seacraft/internal/pkg/server"
)

// FeatureOptions contains configuration items related to API server features.
type FeatureOptions struct {
	EnableProfiling bool `json:"profiling"      mapstructure:"profiling"`
	EnableMetrics   bool `json:"enable-metrics" mapstructure:"enable-metrics"`
}

// NewFeatureOptions creates a FeatureOptions object with default parameters.
func NewFeatureOptions() *FeatureOptions {
	defaults := server.NewConfig()

	return &FeatureOptions{
		EnableMetrics:   defaults.EnableMetrics,
		EnableProfiling: defaults.EnableProfiling,
	}
}

// ApplyTo applies the run options to the method receiver and returns self.
func (o *FeatureOptions) ApplyTo(c *server.Config) error {
	c.EnableProfiling = o.EnableProfiling
	c.EnableMetrics = o.EnableMetrics

	return nil
}

// Validate is used to parse and validate the parameters entered by the user at
// the command line when the program starts.
func (o *FeatureOptions) Validate() []error {
	return []error{}
}

// AddFlags adds flags related to features for a specific api server to the
// specified FlagSet.
func (o *FeatureOptions) AddFlags(fs *pflag.FlagSet) {
	if fs == nil {
		return
	}

	fs.BoolVar(&o.EnableProfiling, "feature.profiling", o.EnableProfiling,
		"Enable profiling via web interface host:port/debug/pprof/")

	fs.BoolVar(&o.EnableMetrics, "feature.enable-metrics", o.EnableMetrics,
		"Enables metrics on the apiserver at /metrics")
}
