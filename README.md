# Trendline E-Commerce Backend

## Overview

This is the backend service for the **Trendline** e-commerce platform. It is built using ASP.NET Core and serves as the main API for managing products, orders, customers, discounts, catalogs, and reports. The service provides secure authentication and authorization through Identity Framework and JWT tokens and integrates with PostgreSQL for database management.

## Technologies Used

- **ASP.NET Core:** The primary framework for building the backend API.
- **Entity Framework Core:** An ORM for database access and management.
- **PostgreSQL:** The relational database for storing application data.
- **Identity Framework:** For managing user identities, roles, and authentication.
- **JWT Tokens:** For secure, token-based authentication.
- **Swagger:** For API documentation and testing.

## Getting Started

### Prerequisites

- **.NET SDK:** Make sure you have the .NET SDK installed. You can download it from the [official .NET website](https://dotnet.microsoft.com/download).
- **PostgreSQL:** Install PostgreSQL and set up a database. You can download it from the [official PostgreSQL website](https://www.postgresql.org/download/).
- **Visual Studio or VS Code:** For development and running the application.

### Setup Instructions

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/VullnetA/TrendLine.git
   cd Trendline

2. **Configure the Database:**

Create a PostgreSQL database for the application.
Update the connection string in appsettings.json with your PostgreSQL database details:

    "ConnectionStrings": {
      "DefaultConnection": "Host=localhost;Database=your_db_name;Username=your_username;    Password=your_password"
    }

3. **Apply Migrations:**

Run the following command to apply database migrations:

    dotnet ef database update

4. **Run the application:**

Use the following command to run the application:

    dotnet run

Access the API:

The API will be available at http://localhost:5000

## Usage Guidelines

***API Endpoints***

**Auth Management**

```POST /api/Auth/register``` - Register a new user.

```POST /api/Auth/login``` - Log in and obtain a JWT token.

```POST /api/Auth/role``` - Create a new role.

```POST /api/Auth/assign``` - Assign a role to a user.

```POST /api/Auth/registerCustomer``` - Register a new customer.

**Catalog Management**

Brand

```GET /api/Catalog/brands``` - List all brands.

```POST /api/Catalog/brands``` - Add a new brand.

```PUT /api/Catalog/brands/{id}``` - Update a brand.

```DELETE /api/Catalog/brands/{id}``` - Delete a brand.

Category

```GET /api/Catalog/categories``` - List all categories.

```POST /api/Catalog/categories``` - Add a new category.

```PUT /api/Catalog/categories/{id}``` - Update a category.

```DELETE /api/Catalog/categories/{id}``` - Delete a category.

Color

```GET /api/Catalog/colors``` - List all colors.

```POST /api/Catalog/colors``` - Add a new color.

```PUT /api/Catalog/colors/{id}``` - Update a color.

```DELETE /api/Catalog/colors/{id}``` - Delete a color.

Size

```GET /api/Catalog/sizes``` - List all sizes.

```POST /api/Catalog/sizes``` - Add a new size.

```PUT /api/Catalog/sizes/{id}``` - Update a size.

```DELETE /api/Catalog/sizes/{id}``` - Delete a size.

***Customer Management***

```GET /api/Customer``` - List all customers.

```GET /api/Customer/{id}``` - Get customer by ID.

```DELETE /api/Customer/{id}``` - Delete a customer.

***Discount Management***

```GET /api/Discount``` - List all discounts.

```POST /api/Discount``` - Add a new discount.

```PUT /api/Discount/{id}``` - Update a discount.

```DELETE /api/Discount/{id}``` - Delete a discount.

***Order Management***

```GET /api/Order``` - List all orders.

```POST /api/Order``` - Create a new order.

```GET /api/Order/{id}``` - Get order by ID.

```PUT /api/Order/{id}/status``` - Update order status.

```DELETE /api/Order/{id}``` - Delete an order.

```GET /api/Order/status/{status}``` - Get orders by status.

```GET /api/Order/dateRange``` - Get orders within a date range.

```GET /api/Order/{orderId}/items``` - Get items in an order.

***Product Management***

```GET /api/Product``` - List all products.

```POST /api/Product``` - Add a new product.

```GET /api/Product/{id}``` - Get product by ID.

```PUT /api/Product/{id}``` - Update a product.

```DELETE /api/Product/{id}``` - Delete a product.

```GET /api/Product/byCategory/{category}``` - List products by category.

```GET /api/Product/byBrand/{brand}``` - List products by brand.

```GET /api/Product/byGender/{gender}``` - List products by gender.

```GET /api/Product/byPriceRange``` - List products within a price range.

```GET /api/Product/bySize/{size}``` - List products by size.

```GET /api/Product/byColor/{color}``` - List products by color.

```GET /api/Product/countByCategory/{category}``` - Count products by category.

```GET /api/Product/countByBrand/{brand}``` - Count products by brand.

```GET /api/Product/countAvailable``` - Count available products.

```GET /api/Product/countOutOfStock``` - Count out-of-stock products.

```GET /api/Product/quantity/{id}``` - Get product quantity.

```PUT /api/Product/quantity/{id}``` - Update product quantity.

```GET /api/Product/search``` - Advanced product search with filters.

***Report Generation***

```GET /api/Report/daily-sales``` - Generate a daily sales report.

```GET /api/Report/monthly-sales``` - Generate a monthly sales report.

```GET /api/Report/top-products``` - Generate a report of top products.

## Contributing

We welcome contributions! If you want to contribute to this project, please follow these steps:

1. Fork the repository.
2. Create a new branch (git checkout -b feature-branch).
3. Commit your changes (git commit -m "commit message").
4. Push to the branch (git push origin feature-branch).
5. Open a pull request.

## Contact
For any inquiries or support, please contact me at vullnetazizi9@gmail.com.
