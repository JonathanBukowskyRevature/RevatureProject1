# RevatureProject1

## Project Description
The Project1.StoreApplication project delivers an e-commerce website to purchase products from a store. The website provides the administration functionality to manage store locations, their respective inventories, and the list of all products available from this organization. From a customer's perspective, this site will allow them to look at products, add products to their cart, and checkout to place an order through a specified store location.

## Technologies Used
* ASP.NET Core WebApi
* EntityFramework Core
* Microsoft SQL Server DB
* .NET Framework - 5.0
* C#
* Javscript
* HTML
* CSS
* xUnit

## Features
Current features:
* Login Required
* Manage list of customers
* Manage list of products
* Manage store locations
* Manage inventory at each store location separately
* Place an order at a specific location (adjusts inventory accordingly)

Features to be implemented:
* Provide better interface for updating quantities
* Reorganize page layouts to move functionalities to more appropriate pages
* Allow user to select a default store location
* Improve UI to allow user to sort displays by date, name, etc.
* Add functionality to suggest products based on user's history

## Getting Started
First, clone the repo
> git clone git@github.com:JonathanBukowskyRevature/RevatureProject1.git

Next, configure environment
> cd RevatureProject1

> dotnet restore

Set up a local copy of the database, update the code with your connection string or supply it through user secrets

Start the web server
> dotnet run

Finally, navigate to the testing url (usually set to http://localhost:5000 in the dev environment unless you configure it differently). You should see a working version of this application.

## Usage
After setting up your environment, you can deploy your code to any popular hosting provider using their specific deployment instructions. You could also set up a web server yourself to host this site -- just follow the instructions in Getting Started to set up your environment on the server.

## License
[MIT License](https://github.com/JonathanBukowskyRevature/RevatureProject1/blob/main/LICENSE)
