using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Implementations
{
    internal class NativeImplementation
    {
        public static async Task Login(string username, string password)
        {
            var httpClient = new HttpClient();

            var apiResponse = await httpClient.GetAsync("https://localhost:7294/api/user");
            httpClient.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue("Bearer", "invalid_access_token");


            if (apiResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var identityServerResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = "https://localhost:7294/connect/token",
                    ClientId = "ConsoleApp_ClientId",
                    ClientSecret = "secret_for_the_consoleapp",
                    Scope = "ApiName",
                    UserName = username,
                    Password = password
                });

                if(!identityServerResponse.IsError)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine();
                    Console.WriteLine("SUCCESS!!");
                    Console.WriteLine();
                    Console.WriteLine("Access Token: ");
                    Console.WriteLine(identityServerResponse.AccessToken);
                    httpClient.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue("Bearer", identityServerResponse.AccessToken);
                    Console.WriteLine();
                    Console.WriteLine("API response:");
                    Console.WriteLine(await apiResponse.Content.ReadAsStringAsync());
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("Failed to login with error:");
                    Console.WriteLine(identityServerResponse.ErrorDescription);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine();
                Console.WriteLine("YOU ARE NOT PROTECTED!!!");
            }






        }
    }
}
