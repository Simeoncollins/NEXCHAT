using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using NEXCHAT.Client;
using NEXCHAT.Client.Classes;
using NEXCHAT.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<ChatSignalRService>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddScoped<IToastService, ToastService>();

// auth hanlder
builder.Services.AddTransient<AuthHandler>();
builder.Services.AddScoped(sp =>
  new HttpClient(sp.GetRequiredService<AuthHandler>())
  {
      BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
  });

// authentication state
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthStateProvider>();
builder.Services.AddAuthorizationCore();


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
