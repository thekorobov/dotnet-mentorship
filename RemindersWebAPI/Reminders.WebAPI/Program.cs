using System.Text;
using Hangfire;
using Hangfire.PostgreSql;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Reminders.BLL.CQS;
using Reminders.BLL.CQS.Reminders.Commands.CreateReminder;
using Reminders.BLL.CQS.Reminders.Commands.DeleteReminder;
using Reminders.BLL.CQS.Reminders.Commands.UpdateReminder;
using Reminders.BLL.CQS.Reminders.Queries.GetAllReminders;
using Reminders.BLL.CQS.Reminders.Queries.GetReminderById;
using Reminders.BLL.CQS.Users.Commands.CreateUser;
using Reminders.BLL.CQS.Users.Commands.DeleteUser;
using Reminders.BLL.CQS.Users.Commands.ResetUserEmail;
using Reminders.BLL.CQS.Users.Commands.UpdateUser;
using Reminders.BLL.CQS.Users.Queries.GetAllUsers;
using Reminders.BLL.CQS.Users.Queries.GetAuthToken;
using Reminders.BLL.CQS.Users.Queries.GetUserById;
using Reminders.BLL.CQS.VerificationCodes.Commands.CreateVerificationCode;
using Reminders.BLL.CQS.VerificationCodes.Commands.ResetVerificationCode;
using Reminders.BLL.CQS.VerificationCodes.Commands.VerifyVerificationCode;
using Reminders.BLL.CQS.VerificationCodes.Queries.GetVerificationCode;
using Reminders.BLL.DTO;
using Reminders.BLL.Interfaces;
using Reminders.BLL.Services;
using Reminders.BLL.Utils;
using Reminders.DAL.Data;
using Reminders.DAL.Entities;
using Reminders.DAL.Repositories;
using Reminders.DAL.Interfaces;
using Reminders.WebAPI.Extensions;
using Reminders.WebAPI.HealthChecks;
using Reminders.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("AZURE_POSTGRESQL_CONNECTIONSTRING")));

builder.Services.AddIdentity<User, IdentityRole<int>>() 
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// DAL
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.RegisterGenericTypes(
    AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains("BLL")), "Repository");

// BLL
builder.Services.AddSingleton<IMediator, Mediator>();
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddSingleton<ISmtpClientWrapper, SmtpClientWrapper>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
builder.Services.AddTransient<ReminderJob>();

builder.Services.AddTransient<ICommandHandler<DeleteReminderCommand>, DeleteReminderHandler>();
builder.Services.AddTransient<ICommandHandler<CreateUserCommand, int>, CreateUserHandler>();
builder.Services.AddTransient<ICommandHandler<DeleteUserCommand>, DeleteUserHandler>();

builder.Services.AddTransient<ICommandHandler<CreateVerificationCodeCommand>, CreateVerificationCodeHandler>();
builder.Services.AddTransient<ICommandHandler<ResetVerificationCodeCommand>, ResetVerificationCodeHandler>();
builder.Services.AddTransient<ICommandHandler<VerifyVerificationCodeCommand>, VerifyVerificationCodeHandler>();

builder.Services.AddTransient<IQueryHandler<GetAuthTokenQuery, string>, GetAuthTokenHandler>();
builder.Services.AddTransient<IQueryHandler<GetVerificationCodeQuery, string>, GetVerificationCodeHandler>();
builder.Services.AddTransient<IQueryHandler<GetReminderByIdQuery, ReminderDto>, GetReminderByIdHandler>();
builder.Services.AddTransient<IQueryHandler<GetAllRemindersQuery, List<ReminderDto>>, GetAllRemindersHandler>();
builder.Services.AddTransient<IQueryHandler<GetUserQuery, UserDto>, GetUserHandler>();
builder.Services.AddTransient<IQueryHandler<GetAllUsersQuery, List<UserDto>>, GetAllUsersHandler>();

builder.Services.AddCommandWithValidationAndResult<CreateReminderCommand, int, CreateReminderHandler, CreateReminderValidator>();
builder.Services.AddCommandWithValidation<UpdateReminderCommand, UpdateReminderHandler, UpdateReminderValidator>();
builder.Services.AddCommandWithValidation<ResetUserEmailCommand, ResetUserEmailHandler, ResetUserEmailValidator>();
builder.Services.AddCommandWithValidation<ResetUserPasswordCommand, ResetUserPasswordHandler, ResetUserPasswordValidator>();

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Logging.AddConsole();

builder.Services.AddHealthChecks().AddCheck<DatabaseHealthCheck>(nameof(DatabaseHealthCheck));

builder.Services.AddHealthChecksUI().AddInMemoryStorage();

builder.Services.AddHangfire(configuration => configuration
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("AZURE_POSTGRESQL_CONNECTIONSTRING"))); 
builder.Services.AddHangfireServer();

var configuration = builder.Configuration;
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin",
        builder => {
            builder.WithOrigins(configuration["Cors:AllowedOrigins"])
                .AllowAnyMethod()  
                .AllowAnyHeader()  
                .AllowCredentials(); 
        });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:SecretKey")))
        };
    })
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    });

builder.Services.BuildServiceProvider().GetService<ApplicationDbContext>().Database.Migrate();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
    await RoleInitializer.RoleInitializeAsync(roleManager).ConfigureAwait(false);
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHealthChecksUI();

app.UseHangfireDashboard("/dashboard");

RecurringJob.AddOrUpdate<ReminderJob>("check-reminders", job => job.ExecuteAsync(), Cron.Daily);

app.UseMiddleware<ResponseWrapperMiddleware>();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowMyOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();