using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http;

namespace NEXCHAT.Client.Services
{
    public class CookieAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _http;
        public CookieAuthStateProvider(HttpClient http) => _http = http;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try {
                var res = await _http.GetAsync("api/auth/me");
                if (res.IsSuccessStatusCode) {
                    var userStr = await res.Content.ReadAsStringAsync();
                    var user = System.Text.Json.JsonSerializer.Deserialize<UserInfoDto>(userStr, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (user != null && !string.IsNullOrEmpty(user.UserId)) {
                        var claims = new[] {
                            new Claim(ClaimTypes.NameIdentifier, user.UserId),
                            new Claim(ClaimTypes.Name, user.Username ?? "User")
                        };
                        var identity = new ClaimsIdentity(claims, "CookieAuth");
                        return new AuthenticationState(new ClaimsPrincipal(identity));
                    }
                }
            } catch { }

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public void NotifyUserAuthentication(string username, string userId)
        {
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username)
            };
            var identity = new ClaimsIdentity(claims, "CookieAuth");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            var anon = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anon)));
        }
    }

    public class UserInfoDto {
        public string UserId { get; set; }
        public string Username { get; set; }
    }
}
