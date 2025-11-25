# EaseioBackendCodingExercise

This is a simple **CRUD Web API** built with **ASP.NET Core** and **C#** for managing GUID-based records. It supports creating, reading, updating, and deleting GUID records stored in an SQLite database.

---

## Features

- **Create** a new GUID record  
- **Read** a GUID record by ID  
- **Update** the `Expires` field of an existing record  
- **Delete** a record by GUID  
- SQLite for data persistence  
- Uses ASP.NET Core, EF Core, and dependency injection

---

## Technology Stack

- ASP.NET Core Web API  
- C#  
- Entity Framework Core  
- SQLite  
- .NET 6+  

---

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- Optional: SQLite DB browser

### Running the API

**Clone the repository**

`git clone https://github.com/alvin562/EaseioBackendCodingExercise.git`  
`cd EaseioBackendCodingExercise`

Once you have the .NET SDK and dotnet CLI installed on your machine, you can start the application by running the following command in the root directory

`dotnet run --launch-profile https`

You should see a couple of info logs indicating the application is starting, eventually starting on something like https://localhost:7067 (port may vary depending on your launch.settings)

---

## API Endpoints

| Method | Route | Description |
|--------|-------|-------------|
| **GET** | `/guid/{guid}` | Get a GUID record by ID |
| **POST** | `/guid` or `/guid/{guid}` | Create a GUID record. If no GUID is passed, one is generated |
| **PUT** | `/guid/{guid}` | Update the `Expires` field of a GUID record |
| **DELETE** | `/guid/{guid}` | Delete a GUID record by ID |

---

## Request / Response Examples

### **POST /guid/{guid}**

**Request**
```json
{
  "expires": "2025-01-01T12:00:00Z",
  "user": "alice"
}
```
**Response (201 Created)**
```json
{
  "guid": "A1B2C3D4E5F6A1B2C3D4E5F6A1B2C3D4",
  "expires": "2025-01-01T12:00:00Z",
  "user": "alice"
}
```

### **GET /guid/{guid}**

**Response (200 OK)**
```json
{
  "guid": "A1B2C3D4E5F6A1B2C3D4E5F6A1B2C3D4",
  "expires": "2025-01-01T12:00:00Z",
  "user": "alice"
}
```

### **PUT /guid/{guid}**

**Request**
```json
{
  "expires": "2025-02-01T12:00:00Z"
}
```
**Response (204 No Content)**

### **DELETE /guid/{guid}**

**Response (200 OK)**

---

## Validation & Error Handling

- **Duplicate GUID on create** → `409 Conflict`
- **Invalid GUID format** → `400 Bad Request`
- **Updating or deleting a non-existent record** → `404 Not Found`
- **Missing or invalid request body** → `400 Bad Request`

