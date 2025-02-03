using DesafioTeste.Models;
using Newtonsoft.Json;

namespace DesafioTeste.Services
{
    public class RandomUserService
    {
        private readonly HttpClient _httpClient;

        public RandomUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<User> GetRandomUserAsync()
        {
            var response = await _httpClient.GetStringAsync("https://randomuser.me/api/");
            var userData = JsonConvert.DeserializeObject<dynamic>(response);

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = userData.results[0].name.first,
                LastName = userData.results[0].name.last,
                Email = userData.results[0].email,
                Gender = userData.results[0].gender,
                Avatar = userData.results[0].picture.large
            };

            return user;
        }
    }
}
