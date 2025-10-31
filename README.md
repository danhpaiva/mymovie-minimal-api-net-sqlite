# MyMovieAPI

## Overview
**MyMovieAPI** is a RESTful web service developed using **.NET 8 Minimal API** architecture.  
It provides endpoints for managing entities related to movies, such as actors, films, and ratings.  
The project demonstrates best practices in API design using Entity Framework Core and a SQLite database for persistence.
---

## Features
- CRUD operations for **Actors**, **Movies**, and **Ratings**
- Relational mapping between entities (Actors ↔ Movies)
- Built with **.NET 8 Minimal API**
- Database persistence using **SQLite**
- Code-first migrations with Entity Framework Core
- Dependency Injection and repository patterns
- Swagger documentation enabled by default

---

## Project Structure

```

MyMovieAPI/
│
├── Data/
│   └── MyMovieAPIContext.cs
│
├── EndPoints/
│   ├── AtorEndpoints.cs
│   ├── AvaliacaoEndpoints.cs
│   ├── FilmeEndpoints.cs
│   └── FilmeAtorEndpoints.cs
│
├── Models/
│   ├── Ator.cs
│   ├── Avaliacao.cs
│   ├── Filme.cs
│   ├── FilmeAtor.cs
│   └── EntidadeBase.cs
│
├── appsettings.json
├── Program.cs
└── MyMovieAPI.csproj

```

---

## Technologies

| Component     | Technology            |
| ------------- | --------------------- |
| Framework     | .NET 8                |
| Language      | C#                    |
| ORM           | Entity Framework Core |
| Database      | SQLite                |
| API Style     | Minimal API           |
| Documentation | Swagger (Swashbuckle) |

---

## Installation and Setup

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQLite (included by EF Core provider)

### Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/MyMovieAPI.git
   cd MyMovieAPI
  ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Apply migrations**

   ```bash
   dotnet ef database update
   ```

4. **Run the API**

   ```bash
   dotnet run
   ```

The API will be available at:

```
https://localhost:5001
```

Swagger UI:

```
https://localhost:5001/swagger
```

---

## Entity Descriptions

### 1. `Ator` (Actor)

Represents an actor that can participate in one or more movies.

| Property     | Type     | Description                                     |
| ------------ | -------- | ----------------------------------------------- |
| `Id`         | int      | Unique identifier (inherited from EntidadeBase) |
| `Nome`       | string   | Actor's full name                               |
| `Nascimento` | DateTime | Date of birth                                   |
| `Genero`     | string   | Gender                                          |

---

### 2. `Filme` (Movie)

Represents a movie entity.

| Property         | Type   | Description                            |
| ---------------- | ------ | -------------------------------------- |
| `Id`             | int    | Unique identifier (inherited)          |
| `Titulo`         | string | Movie title                            |
| `Genero`         | string | Movie genre                            |
| `Duracao`        | int    | Duration in minutes                    |
| `AnoLancamento`  | int    | Release year                           |
| `AvaliacaoMedia` | double | Average rating calculated from reviews |

---

### 3. `Avaliacao` (Rating)

Represents a user rating for a specific movie.

| Property     | Type   | Description                     |
| ------------ | ------ | ------------------------------- |
| `Id`         | int    | Unique identifier               |
| `FilmeId`    | int    | Foreign key referencing `Filme` |
| `Comentario` | string | Optional comment                |
| `Nota`       | double | Rating value (e.g., 0–10)       |

---

### 4. `FilmeAtor` (MovieActor)

Represents a many-to-many relationship between movies and actors.

| Property  | Type | Description            |
| --------- | ---- | ---------------------- |
| `FilmeId` | int  | Foreign key to `Filme` |
| `AtorId`  | int  | Foreign key to `Ator`  |

---

## API Endpoints

### **Actors**

| Method   | Route          | Description                    |
| -------- | -------------- | ------------------------------ |
| `GET`    | `/atores`      | Returns all actors             |
| `GET`    | `/atores/{id}` | Returns a specific actor by ID |
| `POST`   | `/atores`      | Creates a new actor            |
| `PUT`    | `/atores/{id}` | Updates an existing actor      |
| `DELETE` | `/atores/{id}` | Deletes an actor by ID         |

---

### **Movies**

| Method   | Route          | Description               |
| -------- | -------------- | ------------------------- |
| `GET`    | `/filmes`      | Returns all movies        |
| `GET`    | `/filmes/{id}` | Returns a specific movie  |
| `POST`   | `/filmes`      | Adds a new movie          |
| `PUT`    | `/filmes/{id}` | Updates an existing movie |
| `DELETE` | `/filmes/{id}` | Deletes a movie by ID     |

---

### **Ratings**

| Method   | Route              | Description            |
| -------- | ------------------ | ---------------------- |
| `GET`    | `/avaliacoes`      | Returns all ratings    |
| `GET`    | `/avaliacoes/{id}` | Returns a rating by ID |
| `POST`   | `/avaliacoes`      | Creates a new rating   |
| `PUT`    | `/avaliacoes/{id}` | Updates a rating       |
| `DELETE` | `/avaliacoes/{id}` | Deletes a rating       |

---

### **Movie-Actor Relations**

| Method   | Route                               | Description                       |
| -------- | ----------------------------------- | --------------------------------- |
| `GET`    | `/filmes-atores`                    | Returns all movie-actor relations |
| `POST`   | `/filmes-atores`                    | Associates an actor with a movie  |
| `DELETE` | `/filmes-atores/{filmeId}/{atorId}` | Removes an actor from a movie     |

---

## Error Handling

The API returns standard HTTP status codes:

| Code                        | Meaning                       |
| --------------------------- | ----------------------------- |
| `200 OK`                    | Successful operation          |
| `201 Created`               | Resource successfully created |
| `400 Bad Request`           | Validation or format error    |
| `404 Not Found`             | Resource not found            |
| `500 Internal Server Error` | Unhandled server exception    |

---

## Logging and Configuration

Application configuration files:

* `appsettings.json`: Contains connection string and application-level settings.
* `appsettings.Development.json`: Environment-specific overrides.

The API uses `ILogger` for request tracing and error logging.

---

## Running with Docker

A `Dockerfile` is included in the root directory.

```bash
docker build -t mymovieapi .
docker run -p 8080:8080 mymovieapi
```

Access at:

```
http://localhost:8080/swagger
```

---

## Author

**Daniel Paiva**
Software Engineer & University Professor
[GitHub](https://github.com/danhpaiva)

---

## License

This project is distributed under the MIT License.
