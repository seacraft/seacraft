
.PHONY: tools.install
tools.install: $(addprefix tools.install., $(TOOLS))

.PHONY: tools.install.%
tools.install.%:
	@echo "===========> Installing $*"
	@$(MAKE) install.$*

.PHONY: tools.verify.%
tools.verify.%:
	@if ! which $* &>/dev/null; then $(MAKE) tools.install.$*; fi

.PHONY: install.golangci-lint
install.golangci-lint:
	@$(GO) install github.com/golangci/golangci-lint/cmd/golangci-lint@v1.46.2
	@golangci-lint completion bash > $(HOME)/.golangci-lint.bash
	@if ! grep -q .golangci-lint.bash $(HOME)/.bashrc; then echo "source \$$HOME/.golangci-lint.bash" >> $(HOME)/.bashrc; fi

.PHONY: install.go-junit-report
install.go-junit-report:
	@$(GO) install github.com/jstemmer/go-junit-report@latest

.PHONY: install.golines
install.golines:
	@$(GO) install github.com/segmentio/golines@latest

.PHONY: install.mockgen
install.mockgen:
	@$(GO) install github.com/golang/mock/mockgen@latest


.PHONY: install.licctl
install.addlicense:
	@$(GO) install github.com/seacraft/licctl@latest

.PHONY: install.goimports
install.goimports:
	@$(GO) install golang.org/x/tools/cmd/goimports@latest

.PHONY: install.swagger
install.swagger:
	@$(GO) install github.com/go-swagger/go-swagger/cmd/swagger@latest

.PHONY: install.mockgen
install.mockgen:
	@$(GO) install github.com/golang/mock/mockgen@latest