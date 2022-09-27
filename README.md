# Task 1

Solution consists of 4 class libraries projects.

1. Data Access Layer.

- allow manipulate entities using ADO.NET;
- Repository pattern was used;
- only repository interfaces and domain entities are public;
- stored procedures are used for Event Create, Update and Delete entities.

2. Business Logic Layer.
   Validate input data.

- Venue
- - unique Description.
- Layout
- - unique Description in Venue;
- Area
- - unique Description in Layout;
- Seat
- - unique row and number for area;
- Event
- - can't be created in the past;
- - can't be created two events in the same venue at the same time

3. Use the application.

- The application provides for running tests only.

4. Test the application.

Tests use NUnit framework, AtoFixture and FluentAssertions libraries.

- Unit tests - testing services (buisness logick)

- - Run: choose Test -> Test Explorer -> Choose TicketManagement.UnitTests -> Click Run.

- Integration tests - repositories are testing.

- Microsoft.SqlServer.DacFx package uses to take dacpac for the database and deploy it to SQL LocalDB instance. Tests use a deployed database. To run integration tests on the computer, you must have installed MS SQL LocalDB 2019. Connection string to MSSQLLocalDB and path to .dacpac file can be changed by editing the file "appsettings.json" located in TicketManagement.IntegrationTests project.

- - Run: choose Test -> Test Explorer -> Choose TicketManagement.IntegrationTests -> Click Run.

# Task 2

1. The project is ASP.NET Core MVC application.

- Application supported multilingualism (Belarasian, English, Russian), authorization and authentication.
- Multilingualism realases by using RequestLocalizationMiddleware and adding in user claims new claim that contain culture info.
- Application uses such technologies as:
- - DAL - Entity Frameworc Core;
- - authorization and authentication - Microsoft Identity;
- - DI container - Microsoft DependencyInjection.

2. Setup the application.

- Publish the database (TicketManagement.Database -> Publish)
- write the database connection string in appsettings.json
- At the first run the database will be seeded by some Events and default users and roles
- _role_ user _name_ user@user.com _password_ **\_Aa12345**
- _role_ moderator _name_ moderator@moderator.com _password_ **\_Aa12345**
- _role_ admin _name_ admin@admin.com _password_ **\_Aa12345**

3. Use the application

- purchase flow (only authenticated user) - Events -> More info -> Buy tickets -> select Area -> select seats -> Buy tickets.
- **user cabinet** (only authenticated user) click on user hello at navbar.
- _possibilities_ - change user data (name, email, etc), change user settings (language, time zone), top up user balance.
- **moderator cabinet** (only authenticated user in role moderator) click on user hello at navbar.
- _possibilities_ - create, update and delete events.
- **admin cabinet** (only authenticated user in role admin) click on user hello at navbar.
- _possibilities_ - change users roles.

4. Test the application.

- Unit tests - testing services (buisness logick)
- - Run: choose Test -> Test Explorer -> Choose TicketManagement.UnitTests -> Click Run.

- Integration tests - repositories and controllers are testing.

- ADO repositories from Task 1 also checked by integration tests. (to config these tests, see Task 1 paragraph 4)

- - Run: choose Test -> Test Explorer -> Choose TicketManagement.IntegrationTests -> Click Run.

NOTES:

1. Only moderator flow is complitly multilingual.

# Task 3

1. The ability to create events from a json file has been added to the main application.

ThirdPartyEventEditor solution added to project.

ThirdPartyEventEditor main abilities:

- perform CRUD operations with events
- the json file is used as a database
- can send a json DB file on request
- uses exception filter that redirects to error page and logs exceptions in the log file
- uses the procedure execution time filter, which log the procedure execution time

2. Setup the application.

### ThirdPartyEventEditor

Change all the necessary settings in the Web.config file as you need

- "DbJsonFile" - json file name where events are stored
- "LogFile" - file with logs name<br />

### TicketManagement

You need to do setups steps from previous tasks.

Change all the necessary settings in the appsettings.json file as you need

- "LogFilePath" - path to the file with logs
- "EventImagesFolder" - name of the folder in wwwroot where images are stored, when they are lodede from a json file
- "FileSizeLimit" - max json file size

3. Use the application

### ThirdPartyEventEditor

- Start the application. On the home page, you can do CRUD operations on events.
-

### TicketManagement

- Start the application -> login as moderator -> click on user hello at navbar -> choose Load events from file -> choose file from the PC -> click Upload.

# Task 4

The application has been transferred to a micro service architecture.

The application consist of:

- TicketManagement.UserManagement.API - works with user entity (CRUD), generate JWT token, verify token etc.
- TicketManagament.EventManagement.API - works with events (CRUD).
- TicketManagement.OrderManagement.API - works with orderse, buy tickets, see orders history, etc.
- TicketManagement.MVC - Presentation layer.

Services communicate with each other using api requests/responses (Refit library is used).

### Setup the application.

1. Publish TicketManagement.Database
2. Change connections strings in appsettings.json files in TicketManagement.UserManagement.API, TicketManagament.EventManagement.API and TicketManagement.OrderManagement.API.
3. Optional: Change other default settings in appsettings.json files (for example path to log file).
4. Run PowerShell script TicketManegement.ps1 in root folder. Input path to solution src folder.
5. Use the application. All functionality from the previous task is preserved.

### Test the application

1. Open solution in Visual Studio Community.
2. Test -> Run All Tests.

Requirements: .NET 6.0.

# Task 5

React UI has been added to the application.

The following libraries were used in the project:

- Mobx - for data stores,
- Axios - for requests to API,
- React Router - for routing,
- Formik - for forms binding,
- Yup - for form fields validation,
- Semantic UI react - for styling,
- date-fns - for work with dates
- and otheths.

### Setup the application.

1. Publish TicketManagement.Database
2. Change connections strings in appsettings.json files in TicketManagement.UserManagement.API, TicketManagament.EventManagement.API and TicketManagement.OrderManagement.API.
3. Optional: Change other default settings in appsettings.json files (for example path to log file).
4. Choose mode of the application **_React_** or **_MVC_** by setting featcher "React" in appsettings.json TicketManagement.MVC.
5. Run PowerShell script TicketManegement.ps1 in root folder. Input path to solution src folder.
6. Use the application. All functionality from the previous task is preserved. (React application doesn't support multilingualism).

### Test the application

1. Open solution in Visual Studio Community.
2. Test -> Run All Tests. (With features React enabled, not all tests are passed).

Requirements: .NET 6.0.
