package config

import "github.com/seacraft/internal/apiserver/options"

// Config is the running configuration structure of the seacraft pump service.
type Config struct {
	*options.Options
}

// CreateConfigFromOptions creates a running configuration instance based
// on a given seacraft pump command line or configuration file option.
func CreateConfigFromOptions(opts *options.Options) (*Config, error) {
	return &Config{opts}, nil
}
