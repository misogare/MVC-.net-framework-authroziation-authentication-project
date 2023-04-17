# NeXen Technologies Business System Web Application
**This project is developed for NeXen Technologies to manage their operations using a centralized web application.
The application is developed using the C# language and the MVC framework. The Entity Framework is used as the ORM tool to interact with the database.**
## Features
    Secure login system for admins
      Admin panel to manage clients, products, pricing, and discounts
      Adding new clients, managing client details, adding new products, and updating product details
      Generating reports on business classifications
      Authorization is done with roles and policies
      Centralized database to store all data
      Cloud-based SAAS offering for B2B clients
## Installation
      Clone the repository to your local machine
      Open the project in your preferred IDE that supports C# and the MVC framework
      Restore the NuGet packages
      Run the database migration using the following command in Package Manager Console:
      ""
      Update-Database
      ""
      Run the application
## Usage
    Login with the admin credentials
    Access the admin panel to manage Users,Products and Orders
    Credentials:
        Admin :admin@mooore.com password = 1qaz!QAZ
        User : user@mooore.com password = 1qaz!QAZ
    Generate reports on business classifications
    Users can access the application based on their roles and policies
    B2B clients can choose between self-hosting or cloud-based SAAS offering
## Note
    The project does not include the order product part, but it can be easily implemented by following the product order relation comments in the code.
    Microsoft Visual Studio is used as the IDE, but any other IDE that supports C# and the MVC framework can be used.
    Feel free to contribute to the project by adding new features or improving existing ones.
