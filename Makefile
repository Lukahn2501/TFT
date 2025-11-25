.PHONY: help build up down logs clean dev-up dev-down rebuild

help: ## Show this help
	@grep -E '^[a-zA-Z_-]+:.*?## .*$$' $(MAKEFILE_LIST) | sort | awk 'BEGIN {FS = ":.*?## "}; {printf "\033[36m%-20s\033[0m %s\n", $$1, $$2}'

build: ## Build all Docker images
	docker compose build

up: ## Start all services (PostgreSQL + DataLoader + API)
	docker compose up -d

down: ## Stop all services
	docker compose down

logs: ## View logs from all services
	docker compose logs -f

logs-api: ## View API logs
	docker compose logs -f api

logs-dataloader: ## View DataLoader logs
	docker compose logs dataloader

clean: ## Remove all containers, volumes, and images
	docker compose down -v
	docker rmi tft-api tft-dataloader 2>/dev/null || true

dev-up: ## Start only PostgreSQL for local development
	docker compose -f docker-compose.dev.yml up -d

dev-down: ## Stop development PostgreSQL
	docker compose -f docker-compose.dev.yml down

rebuild: down build up ## Rebuild and restart all services

run-loader: ## Run DataLoader manually (requires PostgreSQL running)
	cd src/TFT.DataLoader && dotnet run

run-api: ## Run API locally (requires PostgreSQL running)
	cd src/TFT.Api && dotnet run

restore: ## Restore NuGet packages
	dotnet restore

test: ## Run tests
	dotnet test --verbosity normal
