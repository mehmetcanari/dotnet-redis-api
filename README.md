## 📋 Overview

This project is a lightweight, scalable HTTP API built on .NET 9 and ASP.NET Core. It leverages an in-memory caching layer powered by Redis to speed up responses, and includes Swagger/OpenAPI support for quick exploration and testing of its endpoints.

## 🚀 Features

* 🗃️ **Redis in-memory caching**
* 📄 Swagger/OpenAPI documentation
* 🐘 PostgreSQL database support

## 🛠️ Technologies Used

| Technology              | Description                             |
| ----------------------- | --------------------------------------- |
| **.NET 9**              | Core platform for the API               |
| **ASP.NET Core**        | Web framework                           |
| **StackExchange.Redis** | Redis client for .NET                   |
| **Swagger / OpenAPI**   | API documentation and testing interface |
| **PostgreSQL**          | Relational database management system   |


## 📁 Solution Structure

```
Redis.API.sln
├── 📁 Redis.WebAPI/            
│   ├── Controllers
│   ├── API
│   ├── DiContainer
│   └── Program.cs
│
├── 📁 Redis.Application/       
│   ├── DTO
│   ├── Abstract 
│   ├── Services 
│   └── Configuration
│
├── 📁 Redis.Domain/            
│   └── Entities
│
├── 📁 Redis.Infrastructure/   
│   ├── Abstract
│   ├── Migrations
│   ├── Repositories
│
├── 📁 Redis.Persistence/       
│   ├── Context
```

## 🔧 Installation

```bash
# Clone the repository
git clone https://github.com/mehmetcanari/dotnet-redis-api.git
cd dotnet-redis-api
dotnet restore
cd Redis.WebAPI
dotnet watch run
```

## 📚 API Documentation
After running the API, you can explore and test all endpoints through the Swagger UI interface.

Swagger UI: [http://localhost:5001/swagger](http://localhost:5001/swagger)

## 🌟 Basic API Usage

```http
@baseUrl = http://localhost:5001/api

POST {{baseUrl}}/player/create
Content-Type: application/json

{
  "nickname": "Player1092",
  "country": "TR"
}

###

GET {{baseUrl}}/player

###

GET {{baseUrl}}/player/1

###

PUT {{baseUrl}}/player/1
Content-Type: application/json

{
  "nickname": "Player1092",
  "country": "TR"
}

###

DELETE {{baseUrl}}/player/1
```

## 📧 Contact

Project Owner - [bsn.mehmetcanari@gmail.com](mailto:bsn.mehmetcanari@gmail.com)

[![GitHub](https://img.shields.io/badge/github-%23121011.svg?style=for-the-badge&logo=github&logoColor=white)](https://github.com/mehmetcanari)
