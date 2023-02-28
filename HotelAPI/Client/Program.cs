// See https://aka.ms/new-console-template for more information

using Client.Implementations;

Console.Write("Username:");
var username = Console.ReadLine();
Console.Write("Password:");
var password = Console.ReadLine();

Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine();
Console.WriteLine(new string('-', 80));
Console.WriteLine("With Nuget Package: IdentityServer4.Contrib.HttpClientService");
HttpClientServiceImplementation.Login(username, password).GetAwaiter().GetResult();
Console.WriteLine(new string('-', 80));
Console.WriteLine();

Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine();
Console.WriteLine(new string('-', 80));
Console.WriteLine("With Nuget Package: IdentityModel");
NativeImplementation.Login(username, password).GetAwaiter().GetResult();
Console.WriteLine(new string('-', 80));
Console.WriteLine();