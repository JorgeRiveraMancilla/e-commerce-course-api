# E-Commerce Course API

This is an E-Commerce API built with ASP.NET Core 8 Web API that integrates with Cloudinary for image management and Stripe for payment processing.

This project is based on the Udemy course [Learn to build an e-commerce store with .Net, React & Redux](https://www.udemy.com/course/learn-to-build-an-e-commerce-store-with-dotnet-react-redux) by Neil Cummings.

**⚠️ IMPORTANT: This README is for LOCAL DEVELOPMENT ONLY. For production deployment, see the deployment section below.**

**🔒 SECURITY NOTE: Never commit real credentials to version control. The appsettings files contain placeholder values that must be replaced with your actual credentials for local development.**

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
- [Stripe CLI](https://stripe.com/docs/stripe-cli) (for webhook testing)
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

**Update the `appsettings.Development.json` file with your configuration:**

Open `appsettings.Development.json` and update the placeholder values with your real credentials:

#### Required Updates:

**1. JWT Settings**: Generate a secure random string for your JWT token (minimum 256 bits)

**To generate a secure JWT token key:**
- **Windows PowerShell**: `[System.Web.Security.Membership]::GeneratePassword(128, 0)`
- **macOS/Linux**: `openssl rand -base64 64`
- **Online Generator**: Use a secure random string generator

Example:
```bash
# Run this command and copy the output to your TokenKey
openssl rand -base64 64
```

Then replace in `appsettings.Development.json`:
```json
"JWTSettings": {
  "TokenKey": "YOUR_GENERATED_KEY_HERE"
}
```

**2. Cloudinary Settings**: Create a Cloudinary account and replace with your credentials

**To get Cloudinary credentials:**
1. Create a free account at [cloudinary.com](https://cloudinary.com)
2. Go to your Dashboard
3. Copy your Cloud Name, API Key, and API Secret

Then replace in `appsettings.Development.json`:
```json
"Cloudinary": {
  "CloudName": "your-actual-cloud-name",
  "ApiKey": "your-actual-api-key",
  "ApiSecret": "your-actual-api-secret"
}
```

**3. Stripe Settings**: Create a Stripe account and configure webhooks

**To get Stripe credentials and configure webhooks:**
1. Create a Stripe account at [stripe.com](https://stripe.com)
2. Go to the Developers section and get your API keys:
   - Secret key (starts with `sk_test_`)
   - Publishable key (starts with `pk_test_`)
3. Configure webhooks using Stripe CLI:
   - Run `stripe login` to authenticate
   - Start webhook forwarding: `stripe listen --forward-to localhost:5000/api/payment/webhook`
   - Copy the webhook signing secret from the output (starts with `whsec_`)

Then replace in `appsettings.Development.json`:
```json
"StripeSettings": {
  "SecretKey": "sk_test_your_actual_secret_key",
  "PublishableKey": "pk_test_your_actual_publishable_key",
  "WhSecret": "whsec_your_actual_webhook_secret_from_stripe_cli"
}
```

**4. Admin User Settings**: Configure your admin user credentials

Then replace in `appsettings.Development.json`:
```json
"AdminUser": {
  "Name": "your-admin-name",
  "Email": "your-admin-email@example.com",
  "Password": "your-secure-admin-password"
}
```

**Important**:
- Keep the webhook forwarding running while testing payments in the application
- Use a strong password that meets ASP.NET Core Identity requirements (at least 6 characters, with uppercase, lowercase, number, and special character)

### 3. Start PostgreSQL Database

Launch PostgreSQL using Docker Compose:

```bash
# Start the database
docker-compose up -d
```

### 4. Run the Application

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

A Postman collection file is provided in the repository for testing all available endpoints. Import it into Postman to get started with API testing.

## Frontend Requirements

To use this API with the frontend application, you will need to set up the E-Commerce Course Web Client. You can find the frontend repository here: [E-Commerce Course Web Client](https://github.com/JorgeRiveraMancilla/e-commerce-course-web-client)

---

## 🚀 Production Deployment

This project is configured for deployment on **Render** with automatic CI/CD.

### Deploy to Render

1. **Fork or push this repository to GitHub**

2. **Connect to Render**:
   - Go to [render.com](https://render.com)
   - Sign up/Login with your GitHub account
   - Click "New +" → "Blueprint"
   - Connect your GitHub repository

3. **Configure Environment Variables**:
   After the initial deployment, go to your service dashboard and add these environment variables:

   **Required Variables:**
   - `JWTSettings__TokenKey`: Your JWT secret key (generate with: `openssl rand -base64 64`)
   - `Cloudinary__CloudName`: Your Cloudinary cloud name
   - `Cloudinary__ApiKey`: Your Cloudinary API key
   - `Cloudinary__ApiSecret`: Your Cloudinary API secret
   - `StripeSettings__SecretKey`: Your Stripe secret key
   - `StripeSettings__PublishableKey`: Your Stripe publishable key
   - `StripeSettings__WhSecret`: Your Stripe webhook secret
   - `AdminUser__Name`: Admin user name (e.g., "Admin")
   - `AdminUser__Email`: Admin user email (e.g., "admin@yourdomain.com")
   - `AdminUser__Password`: Admin user password (must meet password requirements)

4. **Database**: The PostgreSQL database will be automatically provisioned by Render

5. **Automatic Deployments**: Every push to the main branch will trigger a new deployment

### Deployment Features

- ✅ **Free Tier Compatible**: Configured for Render's free tier
- ✅ **Automatic CI/CD**: Deploys on every push to main branch
- ✅ **Database Integration**: PostgreSQL database automatically provisioned
- ✅ **Environment Variables**: Secure configuration management
- ✅ **Health Checks**: Built-in health monitoring

### Production URL

Once deployed, your API will be available at:
`https://your-app-name.onrender.com`

### Webhook Configuration

For Stripe webhooks in production:
1. Go to your Stripe Dashboard → Webhooks
2. Add endpoint: `https://your-app-name.onrender.com/api/payment/webhook`
3. Select events: `payment_intent.succeeded`, `payment_intent.payment_failed`
4. Copy the webhook signing secret to your environment variables
