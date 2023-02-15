# AstralSafeClient

![GitHub](https://img.shields.io/github/license/LuMarans30/AstralSafeClient)
![GitHub repo size](https://img.shields.io/github/repo-size/LuMarans30/AstralSafeClient)
![Lines of code](https://img.shields.io/tokei/lines/github/LuMarans30/AstralSafeClient)
![GitHub issues](https://img.shields.io/github/issues/LuMarans30/AstralSafeClient)
![GitHub last commit](https://img.shields.io/github/last-commit/LuMarans30/AstralSafeClient)

A .NET MAUI application that encrypts and decrypts an exe file for anti-piracy demonstration.

To run the application, you must first run the server ([AstralSafeServer repo](https://github.com/LuMarans30/AstralSafeServer)).

The program encrypts and decrypts a file called main.exe, which is located in the Resources/Raw folder of the project.

## Usage

The GUI has two functions: the first uses the key generation API endpoint, while the second asks the server to validate the license.

In the first case, the client sends a uid in the format "xxxx-xxxx-xxxx-xxxx" to the server, which returns the license and a key.

In the second case, the client sends a uid and license and the server returns the key used to encrypt the license.
