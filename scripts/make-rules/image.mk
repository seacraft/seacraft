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
# Makefile helper functions for docker image
#

DOCKER := docker
DOCKER_SUPPORTED_API_VERSION ?= 1.32

REGISTRY_PREFIX ?=seacraft
BASE_IMAGE = centos:centos8

EXTRA_ARGS ?= --no-cache
_DOCKER_BUILD_EXTRA_ARGS :=

ifdef HTTP_PROXY
_DOCKER_BUILD_EXTRA_ARGS += --build-arg HTTP_PROXY=${HTTP_PROXY}
endif

ifneq ($(EXTRA_ARGS), )
_DOCKER_BUILD_EXTRA_ARGS += $(EXTRA_ARGS)
endif

IMAGES_DIR ?= $(wildcard ${ROOT_DIR}/build/docker/*)
IMAGES_ALL ?= $(filter-out tools,$(foreach image,${IMAGES_DIR},$(notdir ${image})))
IMAGES_GO ?= $(filter-out ui tools,$(foreach image,${IMAGES_DIR},$(notdir ${image})))
IMAGES_UI ?=$(filter ui,$(foreach image,${IMAGES_DIR},$(notdir ${image})))

ifeq (${IMAGES_GO},)
  $(error Could not determine IMAGES, set ROOT_DIR or run in source dir)
endif
ifeq (${IMAGES_UI},)
  $(error Could not determine IMAGES_UI, set ROOT_DIR or run in source dir)
endif

.PHONY: image.verify
image.verify:
	$(eval API_VERSION := $(shell $(DOCKER) version | grep -E 'API version: {1,6}[0-9]' | head -n1 | awk '{print $$3} END { if (NR==0) print 0}' ))
	$(eval PASS := $(shell echo "$(API_VERSION) > $(DOCKER_SUPPORTED_API_VERSION)" | bc))
	@if [ $(PASS) -ne 1 ]; then \
		$(DOCKER) -v ;\
		echo "Unsupported docker version. Docker API version should be greater than $(DOCKER_SUPPORTED_API_VERSION)"; \
		exit 1; \
	fi

.PHONY: image.daemon.verify
image.daemon.verify:
	$(eval PASS := $(shell $(DOCKER) version | grep -q -E 'Experimental: {1,5}true' && echo 1 || echo 0))
	@if [ $(PASS) -ne 1 ]; then \
		echo "Experimental features of Docker daemon is not enabled. Please add \"experimental\": true in '/etc/docker/daemon.json' and then restart Docker daemon."; \
		exit 1; \
	fi

.PHONY: image.build
image.build: image.verify go.build.verify
	$(addprefix $(MAKE) image.go.build., $(addprefix $(IMAGE_PLAT)., $(IMAGES_GO)))
	#$(addprefix $(MAKE) image.ui.build., $(addprefix $(IMAGE_PLAT)., $(IMAGES_UI)))

.PHONY: image.build.multiarch
image.build.multiarch: image.verify go.build.verify
	$(foreach p,$(PLATFORMS),$(addprefix $(MAKE) image.go.build., $(addprefix $(p)., $(IMAGES_GO))))
	$(foreach p,$(PLATFORMS),$(addprefix $(MAKE) image.ui.build., $(addprefix $(p)., $(IMAGES_UI))))

.PHONY: image.go.build.%
image.go.build.%: go.build.% image.gen.args.%
	$(eval BUILD_FILE := $(ROOT_DIR)/build/docker/$(IMAGE)/build.sh)
	@echo "===========> Building docker image $(IMAGE) $(VERSION) for $(IMAGE_PLAT)"
	@mkdir -p $(TMP_DIR)/$(IMAGE)
	@cat $(ROOT_DIR)/build/docker/$(IMAGE)/Dockerfile\
		| sed "s#BASE_IMAGE#$(BASE_IMAGE)#g" >$(TMP_DIR)/$(IMAGE)/Dockerfile
	@if [ -e $(OUTPUT_DIR)/platforms/$(IMAGE_PLAT)/$(IMAGE) ]; then \
		cp $(OUTPUT_DIR)/platforms/$(IMAGE_PLAT)/$(IMAGE) $(TMP_DIR)/$(IMAGE)/; \
	fi
	@if [ -e $(BUILD_FILE) ]; then \
		bash $(ROOT_DIR)/build/docker/$(IMAGE)/build.sh $(TMP_DIR)/$(IMAGE); \
	fi
	$(eval BUILD_SUFFIX := $(_DOCKER_BUILD_EXTRA_ARGS) --pull -t $(REGISTRY_PREFIX)/$(IMAGE)-$(ARCH):$(VERSION) $(TMP_DIR)/$(IMAGE))
	$(eval GOARCH := $(shell $(GO) env GOARCH))
	@if [ $(shell $(GO) env GOARCH) != $(ARCH) ] ; then \
		$(MAKE) image.daemon.verify ;\
		$(DOCKER) build --platform $(IMAGE_PLAT) $(BUILD_SUFFIX) ; \
	else \
		$(DOCKER) build $(BUILD_SUFFIX) ; \
	fi
	@rm -rf $(TMP_DIR)/$(IMAGE)

.PHONY: image.ui.build.%
image.ui.build.%: ng.build image.gen.args.%
	@echo "===========> Building docker image $(IMAGE) $(VERSION)"
	@mkdir -p $(TMP_DIR)/$(IMAGE)
	@cat $(ROOT_DIR)/build/docker/$(IMAGE)/Dockerfile\
		| sed "s#BASE_IMAGE#$(BASE_IMAGE)#g" >$(TMP_DIR)/$(IMAGE)/Dockerfile
	@if [ -e $(OUTPUT_DIR)/$(IMAGE) ]; then \
		cp -r $(OUTPUT_DIR)/$(IMAGE) $(TMP_DIR)/$(IMAGE)/; \
	fi
	$(eval BUILD_SUFFIX := $(_DOCKER_BUILD_EXTRA_ARGS) --pull -t $(REGISTRY_PREFIX)/$(IMAGE)-$(ARCH):$(VERSION) $(TMP_DIR)/$(IMAGE))
	$(DOCKER) build  $(BUILD_SUFFIX)
	@rm -rf $(TMP_DIR)/$(IMAGE)

.PHONY: image.push
image.push: image.verify go.build.verify image.build
	$(addprefix $(MAKE) image.push., $(addprefix $(IMAGE_PLAT)., $(IMAGES_ALL)))

.PHONY: image.push.multiarch
image.push.multiarch: image.verify go.build.verify image.build
	$(foreach p,$(PLATFORMS),$(addprefix $(MAKE) image.push., $(addprefix $(p)., $(IMAGES_ALL))))

.PHONY: image.push.%
image.push.%: image.gen.args.%
	$(eval ARCH := $(word 2,$(subst _, ,$(PLATFORM))))
	@echo "===========> Pushing image $(IMAGE) $(VERSION) to $(REGISTRY_PREFIX)"
	$(DOCKER) push $(REGISTRY_PREFIX)/$(IMAGE)-$(ARCH):$(VERSION)

.PHONY: image.gen.args.%
image.gen.args.%:
	$(eval IMAGE := $(word 2,$(subst ., ,$*)))
	$(eval PLATFORM := $(word 1,$(subst ., ,$*)))
	$(eval ARCH := $(word 2,$(subst _, ,$(PLATFORM))))
	$(eval IMAGE_PLAT := $(subst _,/,$(PLATFORM)))