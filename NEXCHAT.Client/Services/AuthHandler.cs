using Shared.DTOS;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;

namespace NEXCHAT.Client.Services
{
    public class AuthHandler : DelegatingHandler
    {
        private readonly TokenService _tokens;
        public AuthHandler(TokenService tokens) => _tokens = tokens;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage req, CancellationToken ct)
        {
            var dto = await _tokens.GetAsync();
            if (dto != null)
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", dto.AccessToken);

            var res = await base.SendAsync(req, ct);
            if (res.StatusCode == HttpStatusCode.Unauthorized && dto != null)
            {
                // request refresh
                var refreshRes = await base.SendAsync(
                  new HttpRequestMessage(HttpMethod.Post, "api/auth/refresh")
                  {
                      Content = JsonContent.Create(new RefreshDto { UserId = dto.UserId, RefreshToken = dto.RefreshToken })
                  }, ct);

                if (refreshRes.IsSuccessStatusCode)
                {
                    var newDto = await refreshRes.Content.ReadFromJsonAsync<TokenResponseDto>();
                    await _tokens.SaveAsync(newDto);
                    req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newDto.AccessToken);
                    return await base.SendAsync(req, ct);
                }
            }
            return res;
        }
    }
}
