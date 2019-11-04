# <img src="https://ruc.dk/sites/default/files/2017-05/ruc_logo_download_en.png" width=500px>


## RAWDATA Portfolio Subproject 1

The goal of this portfolio subproject 1 is to provide a database for the SOVA application (Stack Overflow Viewer Application) and to prepare the key functionality of the application. 

#### - [Subproject1 Requirements](Resources/Subproject1_Requiments.pdf)
#### - [Stack Overflow data details](Resources/Subproject1_Data_details.pdf)

This project is developed by group **raw4** of course RAWDATA (Master's in Computer Science, Roskilde University):
- [Özge Yaşayan](https://github.com/ozgey99)
- [Shushma Devi Gurung](https://github.com/shus0001)
- [Ivan Spajić](https://github.com/ivanspajic)
- [Manish Shrestha](https://github.com/shrestaz)

----

## Current status:
![#008000](https://placehold.it/15/008000/000000?text=+) Project was completed on October 14, 2019 and submitted for review.

_By "completed", we mean the project requirementes were fulfilled._

----

## Submitted documents and files:
- [Project report](https://github.com/ivanspajic/SOVA/blob/master/Subproject1/Submission%20files/SOVA%20raw4%20-%20subproject1.pdf)
- [Data script file for complete generation of our database](https://github.com/ivanspajic/SOVA/blob/master/Subproject1/Submission%20files/data_script.sql)
- [Code script file for complete generation of the API to the database](https://github.com/ivanspajic/SOVA/blob/master/Subproject1/Submission%20files/code_script.sql)
- [Test script file testing your API](https://github.com/ivanspajic/SOVA/blob/master/Subproject1/Submission%20files/test_script.sql)
- [Test output in a text file](https://github.com/ivanspajic/SOVA/blob/master/Subproject1/Submission%20files/test_output.txt)

_ER diagrams are located in the "Resources" folder._

----

## Steps to reproduce:

> Prerequisites: You must have [Git](https://git-scm.com/downloads) and [PostgreSQL](https://www.postgresql.org/download/) installed. These steps presumes you are running on Windows with properly set "Environment Variables" for the command line tools. If running any other OS, the only difference is file path.

1. Clone the project:

    `git clone https://github.com/ivanspajic/SOVA.git`

2. Navigate to Subproject1 folder

    `cd .\Subproject1\`

3. Download the [database seed file](https://drive.google.com/open?id=11boo3SuecfRX14-45XpR3IlmOuy70wkM).

4. Create the database named "stackoverflow":

    `psql -U postgres -c 'create database stackoverflow'`

5. Seed the database: (_The following assumes the downloaded file is in the folder "Resources". Change the path to file as needed._)

    `psql -U postgres -d stackoverflow -f .\Resources\stackoverflow_universal.backup`

6. Run `data_script-sql` to update the database as the developers intended, based on the requirements. This will normalize and update the tables.

    `psql -U postgres -d stackoverflow -f '.\Submission files\data_script.sql'`

7. Run `code_script.sql` to create functions, triggers and stored procedures.

    `psql -U postgres -d stackoverflow -f '.\Submission files\code_script.sql'`

8. Run `test_script.sql` to run test functions against the database.

    `psql -U postgres -d stackoverflow -f '.\Submission files\test_script.sql'`

9. _(Optional)_ If you would like the test script's output onto a text file, append ` > test_output.txt` in the above command of point 7. This will create a new file named "test_output.txt" on your current path.

    `psql -U postgres -d stackoverflow -f '.\Submission files\test_script.sql' > test.txt`

Happy Coding! 👨‍💻 👩‍💻
