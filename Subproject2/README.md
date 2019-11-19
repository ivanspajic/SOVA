# <img src="https://ruc.dk/sites/default/files/2017-05/ruc_logo_download_en.png" width=500px>


## RAWDATA Portfolio Subproject 2

The goal of this portfolio subproject 2 is to add a RESTful web service interface to the SOVA application (Stack Overflow Viewer Application) and to extend the functionality.

### [Subproject2 Requirements](Resources/Subproject2_Requiments.pdf)

This project is developed by group **raw4** of course RAWDATA (Master's in Computer Science, Roskilde University):
- [Özge Yaşayan](https://github.com/ozgey99)
- [Shushma Devi Gurung](https://github.com/shus0001)
- [Ivan Spajić](https://github.com/ivanspajic)
- [Manish Shrestha](https://github.com/shrestaz)

----

## Current status:
![#008000](https://placehold.it/15/008000/000000?text=+) Project was completed on November 18, 2019 and submitted for review.

----

## Submitted documents and files:
- [Project report](https://github.com/ivanspajic/SOVA/blob/master/Subproject2/Resources/SOVA%20raw4-subproject2.pdf)

----

## Steps to reproduce

> Prerequisites: You must have [.NET Core](https://dotnet.microsoft.com/download) installed. These steps presumes you are running on Windows with properly set "Environment Variables" for the command line tools. If running any other OS, the only difference is file path.

1. Clone the project:

    `git clone https://github.com/ivanspajic/SOVA.git`

2. Navigate to Subproject1 folder

    `cd .\Subproject2\`

3. Setup the connection to the database

    - For local database, follow this [Wiki](https://github.com/ivanspajic/SOVA/wiki/Set-up-db-connection)
    - For database hosted in RUC's server, in file `.\SOVA\Startup.cs`, comment out line 40, comment in line 43.

----

### Starting the server

#### Terminal:

4. From the path `.\dotnet watch run`

#### Visual Studio:

5. "Debug" menu > "Start without Debugging" or `Ctrl` + `F5`

----

### Making requests using [Postman](https://www.getpostman.com/downloads/) 📬

6. API Description is hosted on [Google Docs](https://docs.google.com/document/d/1AfG9K0IxgiY30jRNSCGHRwNQfBxcFw-7KoB3vqehtLU/edit?usp=sharing) for detailed steps.

----

### Starting the server

#### Terminal:

7. From the project root folder `.\Subproject2` run the dotnet cli command `dotnet test`.

#### Visual Studio GUI:

8. Go to "Test" menu > "Run All Tests"

## 🔌 Connecting to local database

The server connects to the database by reading the connection string from a local JSON file. It is **not** checked in to GitHub as it contains passwords. [Follow this wiki to create the json file for connection string.](https://github.com/ivanspajic/SOVA/wiki/Set-up-db-connection)

## Knows issues ⚡:

- Not all tests pass. Currently 111 out of 133 tests pass.
- Fetching linked posts fail. Work in progress.