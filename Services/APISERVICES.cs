using POS_OLDWAY_SALOON.MVVM.MODELS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace POS_OLDWAY_SALOON.Services
{
    public class APISERVICES
    {
        HttpClient Client;

        public APISERVICES()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri("https://69c4da218a5b6e2dec2b3d33.mockapi.io/");
        }

        //Get all
        public async Task<User> Login(string email, string password)
        {
            var response = await Client.GetAsync("oldsaloon");

            if (!response.IsSuccessStatusCode) return null;

            var users = await response.Content.ReadFromJsonAsync<List<User>>();

            return users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }
    }
}
