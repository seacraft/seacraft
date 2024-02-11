package apiserver

import (
	"github.com/seacraft/internal/apiserver/config"
	"github.com/seacraft/internal/apiserver/options"
	"github.com/seacraft/pkg/app"
	"github.com/seacraft/pkg/log"
)

const commandDesc = `The Seacraft API server  validates and configures data
for the api deploy  package which include CI/CD, product version info, gitlab project, and
others. The API Server services REST operations to do the api objects management.
`

func NewApp(basename string) *app.App {
	opts := options.NewOptions()
	application := app.NewApp("Seacraft API Server",
		basename,
		app.WithOptions(opts),
		app.WithDescription(commandDesc),
		app.WithDefaultValidArgs(),
		app.WithRunFunc(run(opts)))
	return application
}

func run(opts *options.Options) app.RunFunc {
	return func(basename string) error {
		log.Init(opts.Log)
		defer log.Flush()

		cfg, err := config.CreateConfigFromOptions(opts)
		if err != nil {
			return err
		}
		return Run(cfg)
	}
}
