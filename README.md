# BookLibrary

## About the Project

BookLibrary is a web application built with ASP.NET Core MVC (.NET 8).
The idea of the project is to allow registered users to create and
manage books, write reviews, and keep a list of favorite books.


------------------------------------------------------------------------

## Technologies Used

-   .NET 8
-   ASP.NET Core MVC
-   Entity Framework Core
-   SQL Server
-   ASP.NET Core Identity
-   Bootstrap
-   Git / GitHub

------------------------------------------------------------------------

## Project Structure

The solution is separated into several layers:

-   **BookLibrary.Web** -- Controllers, Views, ViewModels, Identity
-   **BookLibrary.Services.Core** -- Business logic
-   **BookLibrary.Services.Core.Contracts** -- Service interfaces
-   **BookLibrary.Data** -- DbContext and entity models
-   **BookLibrary.Common** -- Validation constants

Controllers do not access the database directly. All business logic is
handled through services.

------------------------------------------------------------------------

## Main Functionalities

### Books

-   Create a book
-   View all books
-   View book details
-   Edit book (only by its owner)
-   Delete book (only by its owner)

### Reviews

-   Add a review to a book
-   Rating validation (1 to 5)
-   Reviews are shown in the book details page

### Favorites

-   Add or remove a book from favorites
-   View personal favorites list
-   Users cannot favorite their own books

------------------------------------------------------------------------

## Authentication & Authorization

The application uses ASP.NET Core Identity.

-   Users can register and log in
-   Only authenticated users can access books and favorites
-   Only the owner of a book can edit or delete it
-   UI elements are conditionally rendered based on authentication

------------------------------------------------------------------------

## Validation

Validation is implemented using:

-   DataAnnotations
-   Range attributes
-   Required attributes
-   Shared validation constants
-   Client-side validation

All important checks are also validated on the server side.

------------------------------------------------------------------------

## Database Design

The main entities are:

-   Book
-   Author
-   Genre
-   Review
-   Favorite
-   IdentityUser

Entity Framework Core is used for managing relationships and database
operations.

------------------------------------------------------------------------

## How to Run the Project

1.  Clone the repository.
2.  Open the solution in Visual Studio.
3.  Open **appsettings.json** (or user secrets if configured).
4.  Replace the `DefaultConnection` connection string with your own
    local SQL Server connection string.

Example:

    "ConnectionStrings": {
      "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=BookLibrary;Trusted_Connection=True;TrustServerCertificate=True;"
    }

5.  Open Package Manager Console and run:


    `Update-Database`

6.  Run the project.

After that, you can register a new user or use the demo user and start using the application.

To log in with the demo user, use the email "demo@booklibrary.com" and the password "Demo123!".

------------------------------------------------------------------------