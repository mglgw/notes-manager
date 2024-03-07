
# Notes Manager

### Introduction
It's my first project apart from some console apps I wrote before.
By design it's just a notes manager with basic editing functions.
I used Identity for account creation and management and SQlite for keeping users and their notes safe.

## Used Technologies

- .NET 6.0
- EF Core
- SQlite
- ReactJS
- Tailwind

## To Do

- [ ] Design rework
- [ ] Code cleanup and some optimization 
- [ ] More minor fixes 

# Getting Started

## Prerequisites

Developed and tested on:
-  Node.js - version 18.16.0
-  Yarn - version 1.22.19
-  .NET 6.0

You can replace Yarn with NPM or PNPM

### First step is to run front:

You can find all instructions in frontend repo [here](https://github.com/mglgw/notes-manager-frontend)

### Second step is to run back

Make sure u set up SQlite database and passed ConnectionString in DataBaseContext.cs

Run: 
```
dotnet ef database update
```

to create required tables and relations in database, next:

```
dotnet run
```

to start backend server app and wait till hosting started.

## That's it, everything should work smoothly right now!