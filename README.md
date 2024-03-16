# EndPointFinder Console Application

This is a console application for scanning web sites to find endpoints and APIs.

## Installation

To run this application, you need to have the .NET SDK installed on your machine. You can download it from [here](https://dotnet.microsoft.com/download).

## Usage

1. Clone this repository to your local machine.
2. Open a terminal or command prompt and navigate to the directory where you cloned the repository.
3. Run the following command to build the application:
    ```
    dotnet build
    ```
4. After successful build, run the following command to execute the application:
    ```
    dotnet run
    ```
5. Follow the prompts to enter a number between 1 - 9 and select an option:
    - **1**: Scan a website to find endpoints.
    - **2**: Scan a website to find APIs.
    - **3**: Exit the application.

## Features

- Scan a website for endpoints.
- Scan a website for APIs.

## Structure

The application consists of two main classes:
- `Program.cs`: Contains the entry point and user interface logic.
- `MainMethods.cs`: Implements methods for scanning websites to find endpoints and APIs.

## Dependencies

- [EndPointFinder.Repository.Interfaces](#)
- [EndPointFinder.Repository.Implementation](#)

## License

This project is licensed under the [MIT License](LICENSE).
