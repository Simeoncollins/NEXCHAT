using Microsoft.JSInterop;
using System.Text.Json;
using Shared.DTOS;

namespace NEXCHAT.Client.Services
{
    public class TokenService
    {
        private readonly IJSRuntime _js;
        public TokenService(IJSRuntime js) => _js = js;
        private const string StorageKey = "auth";

        public async Task SaveAsync(TokenResponseDto dto) =>
          await _js.InvokeVoidAsync("localStorage.setItem", "auth", JsonSerializer.Serialize(dto));

        public async Task<TokenResponseDto?> GetAsync()
        {
            var json = await _js.InvokeAsync<string>("localStorage.getItem", "auth");
            return json == null ? null : JsonSerializer.Deserialize<TokenResponseDto>(json);
        }

        public async Task RemoveAsync()
        {
            await _js.InvokeVoidAsync("localStorage.removeItem", StorageKey);
        }
    }
}
