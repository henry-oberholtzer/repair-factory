# RepairFactory

A web application for tracking mechanics and the vehicles they're able to repair, built with C# and Razor Pages, using EFCore & MVC structure and a MySQL database.

By Henry Oberholtzer

## Technologies Used

- C#
- MySQL
- EFCore
- Razor Pages

## User Stories
- Home
    - Splash page containing a list of all vehicles and all engineers
    - (Optional) Preloaded data
- Vehicles
    * Able to see a list of all vehicles that could need to be repaired.
    * Able to add, edit and delete vehicles
    * A vehicle can hold multiple licensed engineers
    * Unable to add a new vehicle if the form data is invalid
    * Able to add new vehicles if no engineers are available
    * Able to select a vehicle, see the details about the vehicle and all engineers assigned to repair the vehicle.
    - (Optional) Add a selectable state for the condition the vehicle is in
    - (Optional) Add a field for the estimated repair time of the vehicle
- Mechanics
    * Able to see a list of all engineers available
    * Able to add, edit and delete engineers
    * A mechanic can be assigned to repair multiple vehicles
    * Unable to add an engineer if the form data is invalid
    * Able to select a mechanic, see their details and the vehicles they've been licensed for
    - (Optional) Add a selectable state for whether the engineer is actively engaged in repairs
- (Optional) Locations
    - Add a table to see locations for vehicles

## Features
- Lots!

## Upcoming Changes
- Add ViewModel
- Add pagination

## Setup/Installation Requirements

- .NET 6 or greater is required for set up, and dotnet-ef to manage migrations.
- To establish locally, [download the repository](https://github.com/henry-oberholtzer/Factory/archive/refs/heads/main.zip) to your computer.
- Open the folder with your terminal and run `dotnet restore` to gather necessary resources.
- In the production direction, `/Factory` run `$ touch appsettings.json`
- In `appsettings.json`, enter the following, replacing `USERNAME` and `PASSWORD` to match the settings of your local MySQL server.
  
```
{
    "ConnectionStrings": {
        "DefaultConnection": "Server=localhost;Port=3306;database=Factory;uid=USERNAME;pwd=PASSWORD;"
    }
}
```
- A local instance of MySQL (8.0.0 or greater) is required to be set up and running to use the project, for information on installing and configuring MySQL, [please see the official documentation.](https://dev.mysql.com/doc/mysql-installation-excerpt/8.3/en/)
- If you do not have `dotnet-ef` installed, first install it by running `dotnet tool install --global dotnet-ef --version 6.0.0`
- Run `dotnet ef database update` to create the database based on the provided database migrations.
- To start the projet, in the production directory, run the command `dotnet run` on your terminal.

## Known Bugs

- Dates will get weird when editing a submission, I think a view model will be a solution
- Can't display full names in the select lists for cars or mechanics yet, again I think a view model may solve this
- Select for assigning in Mechanics isn't filtering down, unsure why this is differnet from the vehicle one.

## License

(c) 2024 [Henry Oberholtzer](https://www.henryoberholtzer.com/)

Original code licensed under the [GNU GPLv3](https://www.gnu.org/licenses/gpl-3.0.en.html#license), other code bases and libraries as stated.
