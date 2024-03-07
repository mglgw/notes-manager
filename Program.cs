using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebNotepad.Models;
using WebNotepad.Services;

const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles
    );
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<User, UserRole>()
    .AddEntityFrameworkStores<DataBaseContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(op =>
{
    op.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
    op.DefaultScheme = IdentityConstants.ApplicationScheme;
});

builder.Services.AddAuthorization(op =>
{
    op.DefaultPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.Configure<IdentityOptions>(options => { options.Password.RequiredLength = 8; });

builder.Services.ConfigureApplicationCookie(options =>
{
    // options.Cookie.MaxAge = TimeSpan.FromMinutes(15);
    options.Cookie.HttpOnly = false;
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = 401;
        return Task.CompletedTask;
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173",
                "https://localhost:7052",
                "https://127.0.0.1:5173",
                "https://localhost:5173",
                "https://localhost:5174");
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
            policy.AllowCredentials();
        });
});
builder.Services.AddDbContext<DataBaseContext>();
builder.Services.AddScoped<NoteService>();
builder.Services.AddScoped<UserService>();

#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(myAllowSpecificOrigins);
app.UseAuthorization();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.Run();