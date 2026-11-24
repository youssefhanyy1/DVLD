# DVLD: Driving and Vehicle License Department

A C# Windows Forms learning project for driving-license workflows, built with .NET Framework 4.7.2, ADO.NET, and SQL Server.

## Areas covered in the source

- People, users, and driver records.
- Local driving-license applications and driving-test appointments.
- License issuance, renewal, and replacement.
- International-license records.
- Detained-license workflows and license-history views.

## Architecture

```text
fullProject/       Windows Forms screens and controls
DVLD_Buisness/     Business layer (repository folder spelling)
DVLD_DataAccess/   SQL Server data-access layer
```

The UI references the business layer, which calls the data-access layer.

## Local setup

Use Windows, Visual Studio with .NET desktop development, the .NET Framework 4.7.2 targeting pack, and SQL Server.

1. Open `fullProject/fullProject.csproj` in Visual Studio and allow its project references to load.
2. Review the local database connection in `DVLD_DataAccess/ClsDataAccsess.cs` and application settings before running. Never commit real credentials.
3. Supply a compatible SQL Server database.
4. Build and run the Windows Forms application.

## Current limitations

The repository does not include a database schema or backup, so a new reviewer cannot reproduce the full database-backed application from this repository alone. Automated tests are not included. The project was reviewed from source; a successful build or end-to-end run is not claimed here.

This is an educational desktop project, not a deployed government system.
