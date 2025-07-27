# Pizzeria Microservice System Example

This project demonstrates a microservice architecture for a pizzeria management system using **ASP.NET** and **Vue.js**. It includes services for catalog, media, accounting, Redis caching, and observability tools like OpenSearch and Grafana.

## Architecture Overview

![Architecture Diagram](https://github.com/maximbtw/simple-microservices/blob/main/docs/sd.png)

The `Management.Web` frontend communicates with the `PizzeriaApi`, which orchestrates calls to:

- `Catalog` (PostgreSQL)
- `Media` (PostgreSQL + Cloudinary)
- `PizzeriaAccounting`(PostgreSQL)
- Redis (for caching)

All services export log and traces (metrics in progress) via collectors to:
- **OpenSearch** (logs)
- **Tempo** (traces)

## Requirements

- **Docker** and **Docker Compose**

## Quick Start

1. Clone:

   ```bash
   git clone https://github.com/maximbtw/simple-microservices.git
   cd simple-microservices

2. Build:

   ```bash
   openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout opensearch.key -out opensearch.crt
   docker-compose up --build
   
## Access
- Frontend: http://localhost:8080
- Grafana: http://localhost:3000
- OpenSearch: http://localhost:9200

## 🔐 Cloudinary Configuration

To enable media upload functionality, you must create a [Cloudinary](https://cloudinary.com/) account and provide your credentials.

In `Media` configuration (e.g., `appsettings.json`), add the following:

```json
"Configuration": {
  "CloudinaryOptions": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  }
}
