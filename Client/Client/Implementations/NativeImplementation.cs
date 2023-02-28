using Client.Models;
using IdentityModel.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Client.Implementations
{
    internal class NativeImplementation
    {
        public static async Task Login(string username, string password, string controller)
        {
                var httpClient = new HttpClient();
                
                var apiResponse = await httpClient.GetAsync("https://localhost:7294/api/" + controller);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "invalid_access_token");


                if (apiResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var identityServerResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
                    {
                        Address = "https://localhost:7279/connect/token",
                        ClientId = "cwm.client",
                        ClientSecret = "secret",
                        Scope = "hotelApi",
                        UserName = username,
                        Password = password,
                    });
                    
                    if (!identityServerResponse.IsError)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine();
                        Console.WriteLine("SUCCESS!!");
                        Console.WriteLine();
                        Console.WriteLine("Access Token: ");
                        Console.WriteLine(identityServerResponse.AccessToken);
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", identityServerResponse.AccessToken);
                        apiResponse = await httpClient.GetAsync("https://localhost:7294/api/" + controller);

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


        public static async Task CreateUser(string username, string password)
        {
            var httpClient = new HttpClient();
            string name;
            string phone;
            string identificationCard;
            string gmail;
            UserDTO user = new UserDTO()
            {
                Name = "Nam Nguyen",
                Phone = "0328515340",
                IdentificationCard = "3525-83492",
                Gmail= "string@gmail.com"
            };

            string json = JsonConvert.SerializeObject(user);

            Console.WriteLine(json);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var apiResponse = await httpClient.PostAsync("https://localhost:7294/api/user", httpContent);
            
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "invalid_access_token");
            

            if (apiResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var identityServerResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = "https://localhost:7279/connect/token",
                    ClientId = "cwm.client",
                    ClientSecret = "secret",
                    Scope = "hotelApi",
                    UserName = username,
                    Password = password,
                });

                if (!identityServerResponse.IsError)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine();
                    Console.WriteLine("SUCCESS!!");
                    Console.WriteLine();
                    Console.WriteLine("Access Token: ");
                    Console.WriteLine(identityServerResponse.AccessToken);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", identityServerResponse.AccessToken);

                    apiResponse = await httpClient.PostAsync("https://localhost:7294/api/user", httpContentn);

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
