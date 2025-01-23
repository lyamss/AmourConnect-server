# Projet AmourConnect - BackEND - API in ASP.NET Core

![Welcome Page](./assets/welcome_page.png)

<p align="center">
  <img src="./assets/logo_amourconnect.ico" width="50" height="50">
</p>

The AmourConnect Server project contains the APIs, database, and other basic infrastructure elements needed for the backend of all AmourConnect client applications.

The server project is written in C# using .NET Core with ASP.NET Core.

# Schema DevOps

![Schema DevOps](./assets/InfraDeployementAmourConnect.drawio.png)

# Schema Cache

![Schema Cache](./assets/redisD.png)

# I use mediatR for CQRS

<p align="center">
  <img src="./assets/mediatR.png" width="50" height="50">
</p>

# UML Class diagram
```mermaid
classDiagram

class User {
  <<class>>
    + Id_User : int
    + userIdGoogle : string
    + Pseudo : string
    + Description : string
    + EmailGoogle : string
    + Profile_picture : byte[]?
    + date_token_session_expiration : DateTime?
    + token_session_user : string?
    + city : string
    + sex : string
    + date_of_birth : DateTime
    + account_created_at : DateTime
}

class RequestFriends {
  <<class>>
    + Id_RequestFriends : int
    + IdUserIssuer : int
    + UserIssuer : User
    + Id_UserReceiver : int
    + UserReceiver : User
    + Status : RequestStatus
    + Date_of_request : DateTime
}

class RequestStatus {
  <<enum>>
    Accepted
    Onhold
}

class Message {
  <<class>>
    + Id_Message : int
    + IdUserIssuer : int
    + UserIssuer : User
    + Id_UserReceiver : int
    + UserReceiver : User
    + message_content : string
    + Date_of_request : DateTime
}

RequestStatus --|> RequestFriends
User "1" -- "*" RequestFriends : RequestsSent
User "1" -- "*" RequestFriends : RequestsReceived
User "1" -- "*" Message : MessagesSent
User "1" -- "*" Message : MessagesReceived
```
# To start API

## Deploy

You can deploy AmourConnect using Docker containers on Windows, macOS, and Linux distributions.

<p align="center">
    <img src="https://i.imgur.com/SZc8JnH.png" alt="docker" />
  </a>
</p>

### Requirements

- [Docker](https://www.docker.com/community-edition#/download)
- [Docker Compose](https://docs.docker.com/compose/install/) (already included with some Docker installations)

### *⛔ Start the Database And Cache first before (in the folder DataBase)*


Start API .NET Core with Docker
```
docker compose -f .\compose.yaml up -d
```

**Clean the caches if that doesn't work :**

```
docker builder prune --force
```

*In prod, when you setup the secrets in Github Action, don't forget to set the connection DB escape special characters in your connection strings to prevent them from being interpreted as command separators in the shell*
```
ConnectionDB="Host=postgresdbsqlamourconnect\;Port=5432\;Username=tchoulo\;Password=123tchoulo123\;Database=amourconnect_dev\;"
```

## Contribute

Code contributions are welcome! Please commit all pull requests to the "main" branch. Please also commit all your development code to the "dev" branch.

Security audits and feedback are welcome. Please open an issue or email us privately if the report is sensitive. You can read our security policy in the [`SECURITY.md`](SECURITY.md) file.

No grant of rights in AmourConnect's trademarks, service marks, or logos is granted (except as may be necessary to comply with applicable notice requirements).
