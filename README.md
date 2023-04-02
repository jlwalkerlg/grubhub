## Getting started

- Static assets (photos) stored in S3 eu-west-1.

## DDD concepts
- Value objects
- Aggregates
- Always-valid entities
- Encapsulation
- Repository pattern

## Hexagonal Architecture concepts
- Core business logic decoupled from infrastructure via interfaces
- Pluggable dependencies
- Unit tests drive application logic

## Clean Architecture concepts
- Inversion of dependencies to follow the dependency rule
- Application logic decoupled from delivery mechanism (web controllers)
- Application logic decoupled from infrastructure via interfaces and adapters
- Application use cases modelled explicitly as Handler objects
- Simple DTOs passed between layers

## CQRS concepts
- Command side involves DDD-style domain model w/encapsulation, whereas query side involves simple DTO models
- Query side reads data from database directly through Dapper using hand-written SQL queries
- Command side loads aggregate roots wholesale through Entity Framework Core

## Event-driven architecture concepts
- Asynchronous workflows via messaging with AWS SNS
- Outbox pattern
- Idempotent consumers
- Websockets for live updates using SignalR

## Vertical slice architecture concepts
- Use cases packaged by feature rather than by layer
- Request/response objects not shared between features
