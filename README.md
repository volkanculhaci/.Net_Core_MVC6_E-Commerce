# E-Commerce Web Application

This is an E-Commerce web application built with ASP.NET Core MVC 6 and Entity Framework Core. It allows users to browse and purchase products, manage their shopping cart, and save multiple shipping addresses. Admin users have access to product and category management.

## Features

### User-Facing Features

- **Product Catalog:** Users can browse and view product details, including name, description, price, and category.

- **Shopping Cart:** Users can add products to their shopping cart, adjust quantities, and proceed to checkout.

- **User Authentication:** User accounts are protected by authentication. Users can register, log in, and reset their passwords.

- **Multiple Shipping Addresses:** Users can save and manage multiple shipping addresses for order delivery.

### Admin Features

- **Product Management:** Admin users can create, edit, and delete products. Each product includes a name, description, price, and category.

- **Category Management:** Admin users can create, edit, and delete product categories.

## Technology Stack

- **ASP.NET Core MVC 6:** Used for building the web application.

- **Entity Framework Core:** Used for database interactions and data modeling.

- **ASP.NET Core Identity:** Provides user authentication and management.

## Video Demo
[![Watch the video](https://img.youtube.com/vi/D6tP9oSECLg/0.jpg)](https://www.youtube.com/watch?v=D6tP9oSECLg)

## Entity Relationship (ER) Diagram
![ER Diagram](/ecommerce/gitImages/er_diagram.png)
## Getting Started

1. Clone this repository to your local machine.
2. Configure your database connection in the `appsettings.json` and `appsettings.Development.json` files.
3. Run database migrations using the Entity Framework CLI or the Package Manager Console in Visual Studio:
4. Launch the application:
5. 
## Usage

- Visit the homepage to browse products.
- Register or log in to manage your shopping cart and shipping addresses.
- Admin users can access the admin dashboard to manage products and categories.

## Contributing

Contributions are welcome. If you'd like to improve this project, please fork the repository and create a pull request.

## License

This project is licensed under the [MIT License](LICENSE).
