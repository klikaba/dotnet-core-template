# .NET 5 API Template
This template provides starting point for .NET Core API, following Klika quality guidelines, with implemented authorization following OAuth2 standard.

## Getting started

Use Klika quality guidelines for general development references.

### Configuration
To get basic idea about configuration approach read [Configuration in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0).

`appsettings.json` configuration file has to include all basic configuration required for this project to work. For any environment specific configuration use `appsettings.{EnvironmentName}.json` specific configuration file. For local specific configuration use appsettings.{Development}.json

## Tools

### Authentication using OAuth2 standard
Authentication is implemented using [IdentityServer4](https://identityserver4.readthedocs.io/en/latest/) and [JWT token standard](https://jwt.io/) implementing both password and client credentials flow. More details about [OAuth2 Standard](https://oauth.net/2/).

### Automatic API Documentation
We are generating API documentation with [Swagger UI](https://swagger.io/)

### Application Logs
[App Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) are used as sink for application logs.

### Continuous Integration
For CI/CD we are using [Azure Devops](https://azure.microsoft.com/en-us/services/devops/) and [Azure App Services](https://azure.microsoft.com/en-us/services/app-service/)

### Relational database management system
[Azure SQL Database](https://azure.microsoft.com/en-us/products/azure-sql/database/) is used as database provider.

### Cryptographic keys and secrets
[Azure Key Vault](https://azure.microsoft.com/en-us/services/key-vault/) is used as storage for secrets.

### Azure services authentication
[Managed Identity](https://docs.microsoft.com/en-us/azure/active-directory/managed-identities-azure-resources/overview) is used as mechanism for authentication to azure services without using credentials.

## Maintainers

- [Faruk Redzic](https://github.com/farukredzic)