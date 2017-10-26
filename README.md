# dotnet-webapi-ef
A solution I developed in 2015 to get a feel of Web API 2 with Entity Framework.

## Table of Contents
- [Technology](#technology)
- [Database](#database)
- [Setup](#setup)
- [API Endpoints](#apiendpoints)
- [Dojocat](#dojocat)


## Technology <a id="technology">
This uses the following technology...

- C#
- ASP.NET Web API 2
- Entity Framework 6 (using code first approach)
- JsonPatch
- SimpleInjector
- NUnit
- NSubsitute
- NBuilder
- FluentAssertions


## Database <a id="database">
Developed against _localdb_

Connection string can be found [here](https://github.com/bidwall/dotnet-webapi-ef/blob/master/src/BooksAPI/Web.config "Web.Config file")

Database is seeded with test data, found [here](https://github.com/bidwall/dotnet-webapi-ef/blob/master/src/DataAccess/Migrations/Configuration.cs "Seeded data")


##Setup <a id="setup">
Restore nuget packages

`nuget install`

Seed database by typing the following in _Package Manager Console_

`Update-Database`

> **Note:** You can more information on seeding [here](https://docs.microsoft.com/en-us/aspnet/web-api/overview/data/using-web-api-with-entity-framework/part-3 "All you need to know about EF seeding!")


## API endpoints <a id="apiendpoints">
Replace placeholder `{}` with actual value

#### Get all Books

`http://localhost:{port}/api/library/books`

#### Get book by Id

`http://localhost:{port}/api/library/books/3`

#### Get books by Author

`http://localhost:{port}/api/library/authors/{author}/books`

#### Post new book

`http://localhost:{port}/api/library/books`

```json
{
    "Title": "Austrailia",
    "Isbn": "66666",
    "Author": {
        "Id": 1,
        "Name": "Amy Brown"
    },
    "Publisher": {
        "Id": 1,
        "Name": "O' Reilly"
    }
}
```

#### Patch title of existing book using book id

`http://localhost:{port}/api/library/books/{id}`

```
[
  { "op": "replace", "path": "Title", "value": "Australia" }
]
```

> _More info about other patching operations can be found [here](http://jsonpatch.com/ "JsonPatch")_

#### Delete existing book using book id

`http://localhost:{port}/api/library/books/{id}`


## Dojocat <a id="dojocat">
This repo would not be complete without the _Dojocat_...

![Alt text](http://octodex.github.com/images/dojocat.jpg "The Dojocat")
