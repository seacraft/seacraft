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

package cronlog

import (
	"fmt"

	"go.uber.org/zap"
)

type logger struct {
	zapLogger *zap.SugaredLogger
}

// NewLogger create a logger which implement `github.com/robfig/cron.Logger`.
func NewLogger(zapLogger *zap.SugaredLogger) logger {
	return logger{zapLogger: zapLogger}
}

func (l logger) Info(msg string, args ...interface{}) {
	l.zapLogger.Infow(msg, args...)
}

func (l logger) Error(err error, msg string, args ...interface{}) {
	l.zapLogger.Errorw(fmt.Sprintf(msg, args...), "error", err.Error())
}

func (l logger) Flush() {
	_ = l.zapLogger.Sync()
}
