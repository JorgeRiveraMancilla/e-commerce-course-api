# E-Commerce Course API

[![CI/CD](https://github.com/JorgeRiveraMancilla/e-commerce-course-api/actions/workflows/ci.yml/badge.svg)](https://github.com/JorgeRiveraMancilla/e-commerce-course-api/actions/workflows/ci.yml)

A comprehensive e-commerce API built with ASP.NET Core 8, featuring user authentication, product management, shopping cart functionality, payment processing with Stripe, and image upload capabilities via Cloudinary.

## ✨ Features

- **Authentication & Authorization**: JWT-based user authentication with role-based access control
- **Product Management**: CRUD operations for products with search and filtering capabilities
- **Shopping Cart**: Persistent basket functionality with Redis
- **Order Management**: Complete order lifecycle with status tracking
- **Payment Processing**: Secure payment integration with Stripe
- **Image Upload**: Cloud storage for product images via Cloudinary
- **Database**: PostgreSQL with Entity Framework Core

## 🚀 Quick Start

### Prerequisites

- .NET 8 SDK
- Docker (for PostgreSQL)
- Git

### Local Development Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/JorgeRiveraMancilla/e-commerce-course-api.git
   cd e-commerce-course-api
   ```

2. **Start PostgreSQL database**
   ```bash
   docker run --name db-postgres -e POSTGRES_PASSWORD=Pass1234 -p 5432:5432 -d postgres:latest
   ```

3. **Configure application settings**
   ```bash
   cp appsettings.Example.json appsettings.Development.json
   ```

   Edit `appsettings.Development.json` with your configuration values:
   - JWT secret key
   - Cloudinary credentials
   - Stripe API keys
   - Database connection string

4. **Run database migrations**
   ```bash
   dotnet ef database update
   ```

5. **Start the application**
   ```bash
   dotnet run
   ```

### Access Points

- **API Base URL**: `http://localhost:5000`
- **Swagger Documentation**: `http://localhost:5000/swagger`

## 🚀 Production Deployment

### Deploy to Render (Recommended)

#### Step 1: Prepare GitHub Secrets

Navigate to your repository: **Settings → Secrets and variables → Actions**

Add the following secrets:
- `DOCKERHUB_USERNAME`: Your Docker Hub username
- `DOCKERHUB_TOKEN`: Docker Hub access token
- `RENDER_SERVICE_ID`: Your Render service ID (optional)
- `RENDER_API_KEY`: Your Render API key (optional)

#### Step 2: Deploy to Render

1. Fork this repository to your GitHub account
2. Connect your GitHub account to [Render](https://render.com)
3. Create a new **Blueprint** service
4. Select your forked repository
5. Configure the service settings in Render dashboard

#### Step 3: Configure Environment Variables

In your Render service dashboard, add these environment variables:

```env
# JWT Configuration
JWTSettings__TokenKey=your-production-jwt-key-here

# Cloudinary Configuration
Cloudinary__CloudName=your-cloud-name
Cloudinary__ApiKey=your-api-key
Cloudinary__ApiSecret=your-api-secret

# Stripe Configuration
StripeSettings__SecretKey=your-stripe-secret-key
StripeSettings__PublishableKey=your-stripe-publishable-key
StripeSettings__WhSecret=your-stripe-wh-secret

# Admin User Setup
AdminUser__Name=your-admin-name
AdminUser__Email=your-admin-email
AdminUser__Password=your-admin-password
```

## 📖 Learning Resources

This project is based on the [Udemy course](https://www.udemy.com/course/learn-to-build-an-e-commerce-store-with-dotnet-react-redux) by Neil Cummings, which provides comprehensive guidance on building full-stack e-commerce applications.

## 📄 License

This project is for educational purposes. Please refer to the original course materials for licensing information.
