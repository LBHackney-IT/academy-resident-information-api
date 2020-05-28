.PHONY: setup
setup:
	docker-compose build

.PHONY: build
build:
	docker-compose build academy-api

.PHONY: serve
serve:
	docker-compose build academy-api && docker-compose up academy-api

.PHONY: shell
shell:
	docker-compose run academy-api bash

.PHONY: test
test:
	docker-compose build academy-api-test && docker-compose up academy-api-test

.PHONY: lint
lint:
	-dotnet tool install -g dotnet-format
	dotnet tool update -g dotnet-format
	dotnet format
