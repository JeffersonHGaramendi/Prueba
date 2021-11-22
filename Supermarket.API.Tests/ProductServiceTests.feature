Feature: ProductServiceTests
As a Developer
I want to add new Product through API
So that It can be available for applications.

    Background:
        Given the Endpoint https://localhost:5001/api/v1/products is available
        And A category is already stored
          | Id | Name                  |
          | 1  | Fruits and Vegetables |

    @product-adding
    Scenario: Add Product
        When A Post Request is sent
          | Name  | QuantityInPackage | UnitOfMeasurement | CategoryId |
          | Apple | 3                 | 1                 | 1          |
        Then A Response with Status 200 is received
        And A Product Resource is included in Response
          | Name  | QuantityInPackage | UnitOfMeasurement | CategoryId |
          | Apple | 3                 | UN                | 1          |

    Scenario: Add Product with Invalid Category
        When A Post Request is sent
          | Name   | QuantityInPackage | UnitOfMeasurement | CategoryId |
          | Orange | 1                 | 1                 | -1         |
        Then A Response with Status 400 is received
        And A Message of "Invalid Category." is included in Response Body

    Scenario: Add Product with existing Name
        Given A Product is already stored
          | Name   | QuantityInPackage | UnitOfMeasurement | CategoryId |
          | Banana | 2                 | 4                 | 1          |
        When A Post Request is sent
          | Name   | QuantityInPackage | UnitOfMeasurement | CategoryId |
          | Banana | 1                 | 4                 | 1          |
        Then A Response with Status 400 is received
        And A Message of "Product Name already exists." is included in Response Body