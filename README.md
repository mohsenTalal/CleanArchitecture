# CleanArchitecture
Clean Architecture Solution Template for **.Net Core 3.1 API, This Template is a light one**.

**Enterprise Application Integration - EAI**
**(It has three layers)** 

**First is API or host**

this layer has included things 

1- Controllers

2- Middlewares has included many of classes (
 **Basic Authentication Middleware**,
 **Custom Validation Result**,
 **Exception Middleware**,
 **Localization Middleware**,
 **Logging Middleware** and 
 **Middleware Extensions** )

**Second is the Application**

This layer has waiting for business logic and it's responsible HTTP client (connect with the provider) here I merge infrastructure layer where it makes sense 

**there is the Core** 

This layer includes many repositories 

1- Converters

2- DTOs (Data transference Object)

3- Enums or lookups 

4- HTTP ( this repository interfaces for an Http client implementation in application layer )

5- Logging

6- Mapping (Auto mapping transference)

7- Settings ( this is using for any configuration want is configurable) 


## Support

**Need help or wanna share your thoughts?** Don't hesitate to join us on Gitter or ask your question on StackOverflow:

- **StackOverflow: [https://stackexchange.com/users/13936221/abdul-mohsen-al-enazi](https://stackexchange.com/users/13936221/abdul-mohsen-al-enazi)**

## Contributors

**CleanArchitecture** is actively maintained by **[Mohsen Talal](https://github.com/mohsenTalal)**. Contributions are welcome and can be submitted using pull requests.
