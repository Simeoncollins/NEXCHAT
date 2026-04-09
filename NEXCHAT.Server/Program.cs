using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NEXCHAT.CoreBusiness;
using NEXCHAT.CoreBusiness.Interfaces;
using NEXCHAT.Server.Hubs;
using NEXCHAT.Server.Services;
using NEXCHAT.UseCases.ConversationManagement;
using NEXCHAT.UseCases.ConversationManagement.Interfaces;
using NEXCHAT.UseCases.FriendManagement;
using NEXCHAT.UseCases.FriendManagement.Interfaces;
using NEXCHAT.UseCases.MessageManagement;
using NEXCHAT.UseCases.MessageManagement.Interfaces;
using NEXCHAT.UseCases.NotificationManagement;
using NEXCHAT.UseCases.NotificationManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;
using NEXCHAT.UseCases.ReactionManagement;
using NEXCHAT.UseCases.ReactionManagement.Interfaces;
using NEXCHAT.UseCases.Users;
using NEXCHAT.UseCases.Users.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using NEXCHAT.Infrastructure;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;
using NEXCHAT.Infrastructure.Data;
using NEXCHAT.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// configure repositories
builder.Services.AddTransient<IUserRepository, UserRepositoryEfCore>();
builder.Services.AddTransient<IConversationParticipantRepository, ConversationParticipantRepositoryEfCore>();
builder.Services.AddTransient<IConversationRepository, ConversationRepositoryEfCore>();
builder.Services.AddTransient<IMessageRepository, MessageRepositoryEfCore>();
builder.Services.AddTransient<INotificationRepository, NotificationRepositoryEfCore>();
builder.Services.AddTransient<IReactionRepository, ReactionRepositoryEfCore>();

// conversation management
builder.Services.AddTransient<IAddParticipantToConversationUseCase, AddParticipantToConversationUseCase>();
builder.Services.AddTransient<IGetConversationUseCase, GetConversationUseCase>();
builder.Services.AddTransient<IGetUnreadConversationCountUseCase, GetUnreadConversationCountUseCase>();
builder.Services.AddTransient<IGetUserConversationsUseCase, GetUserConversationsUseCase>();
builder.Services.AddTransient<IRemoveParticipantFromConversationUseCase, RemoveParticipantFromConversationUseCase>();
builder.Services.AddTransient<ISetTypingIndicatorUseCase, SetTypingIndicatorUseCase>();
builder.Services.AddTransient<IStartConversationUseCase, StartConversationUseCase>();
builder.Services.AddTransient<IUpdateGroupDetailsUseCase, UpdateGroupDetailsUseCase>();

// message management
builder.Services.AddTransient<IAddReactionToMessageUseCase, AddReactionToMessageUseCase>();
builder.Services.AddTransient<IDeleteMessageUseCase, DeleteMessageUseCase>();
builder.Services.AddTransient<IEditMessageUseCase, EditMessageUseCase>();
builder.Services.AddTransient<IGetMessagesInConversationUseCase, GetMessagesInConversationUseCase>();
builder.Services.AddTransient<IMarkMessageAsDeliveredUseCase, MarkMessageAsDeliveredUseCase>();
builder.Services.AddTransient<IMarkNewMessagesAsSeenUseCase, MarkNewMessagesAsSeenUseCase>();
builder.Services.AddTransient<IRemoveReactionFromMessageUseCase, RemoveReactionFromMessageUseCase>();
builder.Services.AddTransient<ISendMessageUseCase, SendMessageUseCase>();

// friend management
builder.Services.AddTransient<IAcceptFriendRequestUseCase, AcceptFriendRequestUseCase>();
builder.Services.AddTransient<IBlockFriendUseCase, BlockFriendUseCase>();
builder.Services.AddTransient<IGetBlockedFriendsUseCase, GetBlockedFriendsUseCase>();
builder.Services.AddTransient<IGetFriendListUseCase, GetFriendListUseCase>();
builder.Services.AddTransient<IGetPendingFriendRequestsUseCase, GetPendingFriendRequestsUseCase>();
builder.Services.AddTransient<IRejectFriendRequestUseCase, RejectFriendRequestUseCase>();
builder.Services.AddTransient<ISendFriendRequestUseCase, SendFriendRequestUseCase>();
builder.Services.AddTransient<IUnBlockFriendUseCase, UnBlockFriendUseCase>();
builder.Services.AddTransient<ICheckIfBlockedByFriendUseCase, CheckIfBlockedByFriendUseCase>();

// notification management
builder.Services.AddTransient<IGetUnseenNotificationCountUseCase, GetUnseenNotificationCountUseCase>();
builder.Services.AddTransient<IGetUserNotificationsUseCase, GetUserNotificationsUseCase>();
builder.Services.AddTransient<IMarkNotificationsAsSeenUseCase, MarkNotificationsAsSeenUseCase>();
builder.Services.AddTransient<ISendNotificationUseCase, SendNotificationUseCase>();

// Reaction management
builder.Services.AddTransient<ICreateReactionUseCase, CreateReactionUseCase>();     
builder.Services.AddTransient<IGetReactionsUseCase, GetReactionsUseCase>();     
builder.Services.AddTransient<IGetReactionByIdUseCase, GetReactionByIdUseCase>();  

// users
builder.Services.AddTransient<IGetUserByIdUseCase, GetUserByIdUseCase>();
builder.Services.AddTransient<IGetUsersByNameUseCase, GetUsersByNameUseCase>();
builder.Services.AddTransient<IUpdateUserStatusUseCase, UpdateUserStatusUseCase>();

// custom Identity
builder.Services.AddScoped<IUserStore<User>, UserRepositoryEfCore>();
builder.Services.AddScoped<IUserPasswordStore<User>, UserRepositoryEfCore>();


// signalR notifier
builder.Services.AddScoped<IRealTimeNotifier, SignalRNotifier>();

//auth with jwt and identity
builder.Services
  // Core Identity services, but without EF’s built‑in stores:
  .AddIdentityCore<User>(options => {
  })
  // Tell Identity to use your custom store for IUserStore<User> + IUserPasswordStore<User>:
  .AddUserStore<UserRepositoryEfCore>()
  .AddDefaultTokenProviders();    // enables password‑reset, email confirmation, etc.

builder.Services.AddScoped<UserRepositoryEfCore>();


//cookie auth
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options => {
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Events.OnRedirectToLogin = context => {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});


// singalR
builder.Services.AddSignalR();

builder.Services.AddDbContextFactory<NEXCHATDBContext>((services, options) =>
{
    var connectionString = builder.Configuration["ConnectionStrings:NexchatConnection"];
    options.UseSqlite(connectionString);
}, ServiceLifetime.Scoped);

builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");
app.MapHub<ChatHub>("/chatHub");

using (var scope = app.Services.CreateScope())
{
    var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<NEXCHATDBContext>>();
    using var dbContext = dbFactory.CreateDbContext();
    dbContext.Database.Migrate();
}

app.Run();