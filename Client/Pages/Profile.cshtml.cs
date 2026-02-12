using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Client.Pages
{
    public class ProfileModel(IHttpClientFactory httpClientFactory) : PageModel
    {
        //Without loss of sockets
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public string? DataJson { get; private set; }
        public string? Error { get; private set; }

        public async Task OnGetAsync()
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            if (string.IsNullOrWhiteSpace(token))
            {
                Response.Redirect("/Login");
                return;
            }

            var client = _httpClientFactory.CreateClient("Api");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await client.GetAsync("api/data");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    Error = "The token is invalid or has expired. Sign in again.";
                    return;
                }

                Error = $"API Error: {(int)response.StatusCode} {response.ReasonPhrase}";
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            //Formating
            DataJson = JsonSerializer.Serialize(
                JsonSerializer.Deserialize<JsonElement>(json),
                new JsonSerializerOptions { WriteIndented = true }
            );
        }
    }
}
