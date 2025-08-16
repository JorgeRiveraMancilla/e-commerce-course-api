# E-Commerce Course API - Development Tools
# Usage: make [command]

.PHONY: help build run test clean docker-build docker-run docker-stop logs migrate seed

# Default target
help:
	@echo "Available commands:"
	@echo "  help          Show this help message"
	@echo "  build         Build the .NET application"
	@echo "  run           Run the .NET application locally"
	@echo "  test          Run all tests"
	@echo "  clean         Clean build artifacts"
	@echo "  docker-build  Build Docker image"
	@echo "  docker-run    Run with Docker Compose (database only)"
	@echo "  docker-full   Run full stack with Docker Compose"
	@echo "  docker-stop   Stop all Docker containers"
	@echo "  logs          Show Docker container logs"
	@echo "  migrate       Run database migrations"
	@echo "  seed          Seed database with initial data"
	@echo "  reset-db      Reset database (clean + migrate + seed)"

# .NET commands
build:
	@echo "Building .NET application..."
	dotnet build --configuration Release

run:
	@echo "Running .NET application..."
	dotnet run --configuration Development

test:
	@echo "Running tests..."
	dotnet test --verbosity normal

clean:
	@echo "Cleaning build artifacts..."
	dotnet clean
	rm -rf bin obj

# Docker commands
docker-build:
	@echo "Building Docker image..."
	docker build -t e-commerce-course-api:latest .

docker-run:
	@echo "Starting PostgreSQL with Docker Compose..."
	docker-compose up -d postgres
	@echo "PostgreSQL is starting... waiting for health check..."
	@sleep 5
	@echo "Database is ready! You can now run 'make run' to start the API."

docker-full:
	@echo "Starting full stack with Docker Compose..."
	docker-compose --profile full-stack up -d

docker-admin:
	@echo "Starting with pgAdmin..."
	docker-compose --profile admin up -d

docker-stop:
	@echo "Stopping all Docker containers..."
	docker-compose down

logs:
	@echo "Showing Docker container logs..."
	docker-compose logs -f

# Database commands
migrate:
	@echo "Running database migrations..."
	dotnet ef database update

seed:
	@echo "Seeding database..."
	dotnet run --configuration Development -- seed

reset-db:
	@echo "Resetting database..."
	docker-compose down postgres
	docker volume rm $$(docker volume ls -q | grep postgres) 2>/dev/null || true
	docker-compose up -d postgres
	@sleep 10
	$(MAKE) migrate
	$(MAKE) seed

# Development workflow
dev-setup:
	@echo "Setting up development environment..."
	cp .env.example .env
	@echo "Please edit .env file with your configuration"
	docker-compose up -d postgres
	@sleep 10
	$(MAKE) migrate
	$(MAKE) seed
	@echo "Development environment is ready!"
	@echo "Run 'make run' to start the API"

# Production commands
docker-push:
	@echo "Pushing to Docker Hub..."
	@if [ -z "$(DOCKER_TAG)" ]; then \
		echo "Usage: make docker-push DOCKER_TAG=yourusername/e-commerce-course-api:tag"; \
		exit 1; \
	fi
	docker tag e-commerce-course-api:latest $(DOCKER_TAG)
	docker push $(DOCKER_TAG)

# Utility commands
check-health:
	@echo "Checking application health..."
	curl -f http://localhost:5000/health || echo "Application is not running"

watch:
	@echo "Running with hot reload..."
	dotnet watch run
