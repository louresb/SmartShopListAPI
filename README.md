# SmartShopList 

[![licence mit](https://img.shields.io/badge/licence-MIT-blue.svg)](https://github.com/louresb/SmartShopListApp/blob/main/LICENSE)
![Development Status Badge](https://img.shields.io/badge/Status-Completed-green)

This is a shopping list application developed using C#, .NET, and ASP.NET. It allows users to manage their shopping lists, products, and shopping carts.

## Features

- Create, read, update, and delete shopping lists.
- Create, read, update, and delete products.
- Create, read, update, and delete shopping carts.
- Add and remove products from shopping carts.
- Calculate the total price of shopping lists and shopping carts.

## Technologies Used

- C#
- .NET
- ASP.NET
- Microsoft.AspNetCore.Mvc
- Entity Framework Core
- SQLite

## Installation

1. Clone the repository: 

```powershell

git clone https://github.com/louresb/SmartShopListApp

```

2. Navigate to the project directory:

```powershell

cd SmartShopListApp

```

3. Build the project:

```powershell

dotnet build

```


4. Run the project:

```powershell

dotnet run

```

5. Access the application through the URL: https://localhost: &lt;port&gt;

## API Endpoints

### ProductController

- `GET /api/product` - Get all products.
- `GET /api/product/{id}` - Get a product by ID.
- `POST /api/product` - Create a new product.
- `PUT /api/product/{id}` - Update a product by ID.
- `DELETE /api/product/{id}` - Delete a product by ID.

### ShoppingCartController

- `GET /api/shoppingcart` - Get all shopping carts.
- `GET /api/shoppingcart/{id}` - Get a shopping cart by ID.
- `POST /api/shoppingcart` - Create a new shopping cart.
- `POST /api/shoppingcart/{cartId}/addproduct/{productId}` - Add a product to a shopping cart.
- `PUT /api/shoppingcart/{id}` - Update a shopping cart by ID.
- `PUT /api/shoppingcart/{cartId}/updateproduct/{productId}` - Update the quantity of a product in a shopping cart.
- `DELETE /api/shoppingcart/{id}` - Delete a shopping cart by ID.
- `DELETE /api/shoppingcart/{cartId}/removeproduct/{productId}` - Remove a product from a shopping cart.

### ShoppingListController

- `GET /api/shoppinglist` - Get all shopping lists.
- `GET /api/shoppinglist/{id}` - Get a shopping list by ID.
- `POST /api/shoppinglist` - Create a new shopping list.
- `POST /api/shoppinglist/{listId}/addcart` - Add a shopping cart to a shopping list.
- `PUT /api/shoppinglist/{id}` - Update a shopping list by ID.
- `DELETE /api/shoppinglist/{id}` - Delete a shopping list by ID.
- `DELETE /api/shoppinglist/{listId}/removecart/{cartId}` - Remove a shopping cart from a shopping list.

## Examples

### Creating a new shopping cart

To create a new shopping cart, send a POST request to `/api/shoppingcart` with the following JSON payload:

```json
{
"name": "My Shopping Cart",
"shoppingListId": 1
}
```
### Getting all lists

To get all products, send a GET request to `/api/list`.


### Adding a product to the shopping cart

To add a product to the shopping cart send a Post request to `/api/shoppingcart/{shoppingcartId}/addproduct/{productId}` with the following JSON payload: 

```json
{
"quantity": 3
}
```
## Screenshots

<div align="center">

### Post / addproduct / addcart
<img src="https://github.com/louresb/SmartShopListApp/assets/103293696/c80f0513-7acd-4d4e-b988-eb5b9e90d739" width="400" height="380"> <img src="https://github.com/louresb/SmartShopListApp/assets/103293696/4ea1d31f-741d-4dad-9558-8c0143ba1ac7" width="400" height="380">

### Get / GetbyId / Put / updateproduct / updateshoppinglist
<img src="https://github.com/louresb/SmartShopListApp/assets/103293696/97fa6a24-6fec-4897-b555-fd131cc3a5b9" width="400" height="380"> <img src="https://github.com/louresb/SmartShopListApp/assets/103293696/8ec3f2a5-5406-484b-88f1-57851cc5f817" width="400" height="380">

### Delete / removeproduct / removecart
<img src="https://github.com/louresb/SmartShopListApp/assets/103293696/bd383253-a6cd-4e6d-9223-0df8188cb5e3" width="400" height="380">

</div>

## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvements, please open an issue or submit a pull request.

## License
[MIT License](https://github.com/louresb/SmartShopListApp/blob/main/LICENSE) © [Bruno Loures](https://github.com/louresb)
