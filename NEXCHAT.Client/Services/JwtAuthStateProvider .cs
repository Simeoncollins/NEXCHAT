using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text.Json; // Add this namespace

namespace NEXCHAT.Client.Services
{
    public class JwtAuthStateProvider : AuthenticationStateProvider
    {
        private readonly TokenService _tokens;
        public JwtAuthStateProvider(TokenService tokens) => _tokens = tokens;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var dto = await _tokens.GetAsync();
            if (dto == null) return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

            var claims = ParseClaimsFromJwt(dto.AccessToken); // Manual parsing
            var identity = new ClaimsIdentity(claims, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        public void NotifyUserAuthentication(string accessToken)
        {
            var claims = ParseClaimsFromJwt(accessToken); // Manual parsing
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            var anon = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anon)));
        }

        // --- JWT Parsing Logic (No external dependencies) ---
        private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            return keyValuePairs?.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!))
                   ?? Enumerable.Empty<Claim>();
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}