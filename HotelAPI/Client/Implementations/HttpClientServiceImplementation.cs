using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Contrib.HttpClientService;
using IdentityServer4.Contrib.HttpClientService.Extensions;
using IdentityServer4.Contrib.HttpClientService.Models;
using System.Text;
using System.Threading.Tasks;

namespace Client.Implementations
{
    internal class HttpClientServiceImplementation
    {
        public static async Task Login(string username, string password)
        {

            var responseObject = await HttpClientServiceFactory.Instance.CreateHttpClientService().SetIdentityServerOptions<PasswordOptions>(
                x =>
                {
                    x.Address = "https://localhost:7294/connect/token";
                    x.ClientId = "ConsoleApp_ClientId";
                    x.ClientSecret = "secret_for_the_consoleapp";
                    x.Scope = "hotelApi";
                    x.Username = username;
                    x.Password = password;
                })
                .GetAsync("https://localhost:7294/api/user");


            if (!responseObject.HasError)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine();
                Console.WriteLine("SUCCESS!!");
                Console.WriteLine();
                Console.WriteLine("Access Token: ");
                Console.WriteLine(responseObject.HttpRequestMessge.Headers.Authorization.Parameter);

                Console.WriteLine();
                Console.WriteLine("API response:");
                Console.WriteLine(responseObject.BodyAsString);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine("Failed to login with error:");
                Console.WriteLine(responseObject.Error);
            }
        }
    }
}
