# DemoApp API Documentation

This document describes the HTTP API exposed by the DemoApp application.

---

## Base URL

- Local (development): `https://localhost:{PORT}` (Swagger runs at `/swagger`)

Replace `{PORT}` with the port used when running the API (see console output).

---

## Authentication

- The API uses JWT Bearer tokens for authentication.
- Obtain a token by calling the Login endpoint (`POST /api/auth/login`).
- Use the `Authorization` header for requests to protected endpoints:
  - `Authorization: Bearer {token}`

Notes:
- Tokens are issued with issuer and audience configured in `appsettings.json` (`Jwt:Issuer`, `Jwt:Audience`, `Jwt:Key`).
- Swagger UI provides an Authorize button to paste the token for testing.

---

## Common headers

- `Content-Type: application/json` for request bodies
- `Accept: application/json`
- `Authorization: Bearer {token}` for protected endpoints

---

## Error responses

Validation problems are returned as 400 Bad Request with details in the response body.
Authentication failures return 401 Unauthorized with a JSON payload.
Other server errors return 500 Internal Server Error (when unhandled).

Example validation response (400):

```
HTTP/1.1 400 Bad Request
{
  "errors": {
    "UserName": ["UserName is required."]
  },
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400
}
```

Example auth failure (401):

```
HTTP/1.1 401 Unauthorized
{ "StatusCode": 401, "Message": "Invalid username or password" }
```

---

## Endpoints

### Auth

#### POST /api/auth/register
- Description: Register a new user and assign the default role `Reader`.
- Access: Public
- Request body:
  ```json
  {
    "userName": "string",
    "email": "user@example.com",
    "password": "P@ssword1"
  }
  ```
- Responses:
  - `200 OK` - registration succeeded (returns roles)
  - `400 Bad Request` - validation errors or creation failures

#### POST /api/auth/login
- Description: Login and receive an authentication token.
- Access: Public
- Request body:
  ```json
  {
    "userName": "string",
    "password": "string"
  }
  ```
- Responses:
  - `200 OK` - Returns `AuthResponse` (token, userName, expiry)
  - `400 Bad Request` - validation errors
  - `401 Unauthorized` - invalid username or password

Example successful response:
```json
{
  "token": "<jwt-token>",
  "userName": "admin@example.com",
  "expiry": "2026-06-14T12:00:00Z"
}
```

---

### Users

All `Users` endpoints require authentication (Bearer token) unless otherwise noted.

#### GET /api/users
- Description: Get all users
- Access: Authenticated
- Responses:
  - `200 OK` - list of users
  - `401 Unauthorized` - no or invalid token

#### GET /api/users/{id}
- Description: Get user by id (GUID)
- Parameters:
  - `id` (path) - GUID of the user
- Responses:
  - `200 OK` - user details
  - `401 Unauthorized` - no or invalid token
  - `404 Not Found` - user not found

#### POST /api/users
- Description: Create a new domain user (not Identity user). Validated by `CreateUserDto`.
- Access: Authenticated
- Request body example:
  ```json
  {
    "name": "John Doe",
    "age": 30,
    "city": "City",
    "state": "State",
    "pincode": "123456"
  }
  ```
- Responses:
  - `200 OK` - created user
  - `400 Bad Request` - validation errors

#### PUT /api/users/{id}
- Description: Update user by id
- Access: Authenticated
- Request body: `UpdateUserDto` (same schema as create)
- Responses: `200 OK`, `400 Bad Request`, `401 Unauthorized`, `404 Not Found`

#### DELETE /api/users/{id}
- Description: Delete a user by id
- Access: Authenticated
- Responses: `200 OK` on success, `404 Not Found` if not present

---

### Test endpoints

#### GET /api/test
- Description: Protected test endpoint to verify authentication
- Access: Authenticated
- Response sample:
  ```json
  {
    "message": "API Working",
    "user": "admin@example.com",
    "isAuthenticated": true
  }
  ```

#### GET /api/test/headers
- Description: Debug endpoint (AllowAnonymous) that returns request headers — useful to verify Authorization and Origin headers during development.
- Access: Public (for debugging)
- Response: JSON object of headers

---

## Swagger UI

- Swagger is available at `/swagger` when running in Development environment.
- Use the Authorize button and paste `Bearer {token}` (include the `Bearer ` prefix) or the raw token (both are supported by the server) to call protected endpoints.

---

