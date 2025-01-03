# E-Commerce Course API

This is an E-Commerce API built with ASP.NET Core 8 Web API that integrates with Cloudinary for image management and Stripe for payment processing.

This project is based on the Udemy course [Learn to build an e-commerce store with .Net, React & Redux](https://www.udemy.com/course/learn-to-build-an-e-commerce-store-with-dotnet-react-redux) by Neil Cummings.


## Technologies & Features

- **Framework**: ASP.NET Core 8.0
- **Database**: 
  - PostgreSQL
  - Entity Framework Core 8.0
  - Identity Framework Core 8.0
- **Authentication & Authorization**:
  - JWT (JSON Web Tokens)
  - Role-based authorization
- **Payment Processing**:
  - Stripe integration
  - Payment intent management
- **Image Storage**: 
  - Cloudinary integration
  - Photo moderation capabilities
- **Other Tools & Libraries**:
  - AutoMapper for object mapping

## Prerequisites

Before you begin, ensure you have the following installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/downloads)
- [Postman](https://www.postman.com/downloads/) (for testing)
- [Visual Studio Code](https://code.visualstudio.com/) or preferred IDE

## Getting Started

Follow these steps to get the project up and running on your local machine:

### 1. Clone the Repository

```bash
# Clone the project
git clone https://github.com/JorgeRiveraMancilla/e-commerce-course-api.git

# Navigate to the project directory
cd e-commerce-course-api
```

### 2. Configure Application Settings

Update the `appsettings.Development.json` file with your Cloudinary credentials. You'll need to:
1. Create a Cloudinary account at [cloudinary.com](https://cloudinary.com)
2. Get your credentials from your Cloudinary Dashboard
3. Replace the CloudinarySettings section with your actual credentials

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "User ID=appuser;Password=secret;Host=host.docker.internal;Port=5432;Database=e-commerce-course;"
  },
  "JWTSettings": {
    "TokenKey": "5Kwvp7Vj4Y3^fA^Rn)JzRhjb%%gR&AncyV)%)efFmGB@qwe%M&bsm(4Uj&VeJJHM"
  },
  "Cloudinary": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  },
  "AdminUser": {
    "Name": "Admin",
    "Email": "jiriveramancilla@gmail.com",
    "Password": "CopTig221016!"
  }
}
```

### 3. Configure Stripe

You'll need to set up Stripe for payment processing:

1. Create a Stripe account at [stripe.com](https://stripe.com)
2. Once logged in, go to the Developers section
3. Get your API keys from the API keys tab:
   - Get your Secret key (starts with `sk_test_`)
   - Get your Publishable key (starts with `pk_test_`)
4. For webhook testing:
   - Install the [Stripe CLI](https://stripe.com/docs/stripe-cli)
   - Run `stripe login` to get your webhook signing secret
   - Start webhook forwarding with `stripe listen --forward-to localhost:5000/api/payments/webhook`

Initialize the Secret Manager and configure your Stripe keys:

```bash
dotnet user-secrets init
dotnet user-secrets set "StripeSettings:SecretKey" "your-stripe-secret-key"
dotnet user-secrets set "StripeSettings:PublishableKey" "your-stripe-publishable-key"
dotnet user-secrets set "StripeSettings:WhSecret" "your-stripe-webhook-secret"
```

You can verify your secrets are properly set by running:

```bash
dotnet user-secrets list
```

### 4. Start PostgreSQL Database

Launch PostgreSQL using Docker:

```bash
docker run --name dev -e POSTGRES_USER=appuser -e POSTGRES_PASSWORD=secret -p 5432:5432 -d postgres:latest
```

### 5. Run the Application

```bash
# Restore dependencies
dotnet restore

# Run the application
dotnet run
```

The API will be available at:
- HTTP: `http://localhost:5000`

Swagger documentation will be available at:
- `http://localhost:5000/swagger`

## Testing the API

A Postman collection file (`Dating Course.postman_collection.json`) is provided in the repository for testing all available endpoints.

## Frontend Requirements

To use this API with the frontend application, you will need to set up the E-Commerce Course Web Client. You can find the frontend repository here: [E-Commerce Course Web Client](https://github.com/JorgeRiveraMancilla/e-commerce-course-web-client)