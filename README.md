# BankNyBank
## Description
BankNyBank (Bank-New-Bank) is a console app built in C# that emulates a simple bank-app/ATM. The program was a school group project.

In the program the user is able to:
- View their accounts and their accounts balances
- Withdraw and deposit money
- Transfer money between the users own accounts, and to other users accounts
- Open new accounts of different types
- Log out

There is an admin page, where the admin can perform some simple tasks. The admin is of the “User” class, but is hard coded in the system. The admin can:
- View all users,
- Create new users,
- Remove users,
- Log out

## Requirements
The program is written in C# using .NET 6.0 and uses SQL Server for database handling and storage. The packs Microsoft.EntityFrameWork.Core, Microsoft.EntityFrameWork.Tools and Microsoft.EntityFrameWork.SqlServer are used for interacting with the SQL server via the code. 

You are required to have SQL Server with localhost installed to use the database functions.

## Before you run the program
Create a database called “BankNyBank”. Run the command “Update-Database” in the Package Manager Console. In the BankContext datafile (in the folder “Data”) make sure to use the correct connection string for your database.

> optionsBuilder.UseSqlServer(" INPUT CONNECTION STRING HERE ");

## Structure
This is a short description of the program flow and the methods. When main is started the program checks if the user database is empty. If it is, an admin is added and the program then moves on to the welcome screen.

### Welcome screen and login
The program opens on the welcome screen, where the user is prompted to log in. This is handled in the WelcomeScreen, CheckLogin, CheckPin and DisplayMenu methods in the Program and MenuManager classes.

### Menu choices and error handling
Menu choices are handled via the DisplayAndGetMenuChoices method. This method takes a string as a header, and a string array for the menu options, as parameters. These are then printed with an arrow pointing to the first choice. Using the arrow keys allows the user to move between the available options and the enter key to select. 

This eliminates the need for a lot of the error handling in the system by forcing the user inputs to be in the format the program wants it to be. The menu choice method can only return an int larger than 0, and never return an int larger than the desired largest number for the method which calls the menu choice method.

### Admin menu
Logging in as the admin gives access to the admin methods. The admin functions are contained in the AdminFunctions class.

### User menu
The user menu is constructed in separate classes and methods, under the folder Utilities.
During the development, we found it to be very helpful and easier to work with separate classes and defined methods for each desired functionality.

This structure provides easy access and overview to all code, related to the functions for the user.

The user menu itself is located in the User class under Models.

### DbHelper
All interactions with the values in the database are handled through the DbHelper class.

## Credits
- **Pontus Ahlbäck** - [PAhlback](https://github.com/PAhlback)
- **Emil Ejderklev** - [SurrealEmil](https://github.com/SurrealEmil)
- **Dennis Briffa** - [Balos87](https://github.com/Balos87)
- **Filip Nilsson** - [filip-io](https://github.com/filip-io)
