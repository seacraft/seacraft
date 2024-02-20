# Copyright 2024 The seacraft Authors
#
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
# http:www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.

# ==============================================================================
# Makefile helper functions for angular
#

UI ?= $(filter-out %.md, $(wildcard ${ROOT_DIR}/ui/))

.PHONY: ng.build
ng.build:
	$(eval NODE_MODULES := $(UI)node_modules)
	$(eval NGINX := $(UI)nginx)
	@echo "===========> Building binary ui"
	@cd $(UI); \
	if [ ! -d $(NODE_MODULES) ]; then \
		npm install; \
	fi; \
	if [ -z $(BUILD_RUNTIME) ]; then \
	  	npm run build; \
	else \
	  	npm run $(BUILD_RUNTIME); \
	fi; \
	cp -r $(NGINX) $(OUTPUT_DIR)/ui

.PHONY: ng.clean
ng.clean:
	@echo "===========> Cleaning ui build output"
	@-rm -vrf $(OUTPUT_DIR)/ui