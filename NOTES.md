# MVP Features

1. Guests can apply to register a restaurant, providing required restaurant details.
2. Admin can accept/reject a restaurant application.
3. Restaurant managers can set their working hours, minimum delivery fee, minimum delivery time etc, and build a menu.
4. A menu consists of categories, with food items underneath each category. Each food item can have an attached image.
5. Customers can search for restaurants by location (city, post code, current location) and radius from that location, and can filter/sort their search.
6. Customers can add items to their basket and begin checkout to pay online by card with Stripe.
7. Customers can choose a delivery time of either ASAP, or a particular time, rounded to the nearest 5 minutes, which is greater than the restaurant's minimum delivery time but less than their closing time.
8. Restaurant managers can see a live feed of incoming orders, and can accept/reject each one. To accept, they must provide an estimated time for delivery.
9. Payment is taken only when the restaurant confirms the order.
10. Restaurant managers update the status of an order sequentially, from accepted, to out for delivery, to delivered.

# Notes

1. Customers can only order from one restaurant at a time. If they try to add food from a second restaurant, they are prompted to drop their existing order.

# Tech

1. ASP.NET Core for API.
2. Next.JS/React.JS for the front-end.
3. S3 for image uploads + AWS for image processing.
4. SignalR for real-time order tracking.
5. RabbitMQ? for background processing (emails, invoice generation, etc.).
6. Google Geolocation for searching by location.
7. Stripe for payment processing.
8. Docker for managing services.
9. Azure for CI/CD.

# Domain

## Entities

Admin

RestaurantManager

- Restaurant

Restaurant

- Name
- OpeningTimes
- Location
- ContactDetails

Menu

- MenuCategory[]

MenuCategory

- MenuItem[]

MenuItem

- Price

Order

- OrderItem[]

OrderItem

- Quantity

## Value Objects

OpeningTimes

- Monday float[2]
- Tuesday float[2]
- Wednesday float[2]
- Thursday float[2]
- Friday float[2]
- Saturday float[2]
- Sunday float[2]

Location

- Latitude
- Longitude

ContactDetails

- TelephoneNumber
- EmailAddress
- Address

Address

- StreetNumber
- StreetName
- City
- PostCode

# UseCases

## RegisterRestaurant

Data

- Name
- PostCode
- TelephoneNumber
- EmailAddress
- ManagerName
- ManagerPassword
- PaymentInformation

Exceptions

- Validation (incl. email already taken, post code doesn't exist or is outside of UK)
