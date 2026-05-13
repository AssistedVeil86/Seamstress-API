using System.Text.Json.Serialization;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using Seamstress.VSA.Features.Expenses;
using Seamstress.VSA.Features.Orders;
using Seamstress.VSA.Features.WeeklyReports;
using Seamstress.VSA.Infrastructure.BackgroundJobs;
using Seamstress.VSA.Infrastructure.BackgroundJobs.Implementation;
using Seamstress.VSA.Infrastructure.Data;
using Seamstress.VSA.Infrastructure.Email;
using Seamstress.VSA.Infrastructure.Extensions;
using Seamstress.VSA.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Configure JSON Serialization for Numbers
builder.Services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddPersistence(builder.Configuration);
builder.Services.ConfigureHangfire(builder.Configuration);

// Configure CORS
builder.Services.ConfigureCors();

// Identity and Token Scheme Configuration
builder.Services.ConfigureIdentity();

// Smtp Configuration
builder.Services.AddOptions<SmtpOptions>()
    .BindConfiguration(SmtpOptions.SectionName);

builder.Services.AddTransient<IEmailSender<IdentityUser>, SmtpEmailSender<IdentityUser>>();

// Add Validators to Container
builder.Services.RegisterValidators();

// Register Handlers
builder.Services.AddOrderHandlers();
builder.Services.AddWeeklyReportHandlers();
builder.Services.AddExpenseHandlers();

// Register Jobs
builder.Services.AddScoped<IWeeklyReportJob, WeeklyReportJob>();

// Configure QuestPDF
builder.Services.ConfigureQuestPdf();

// Add Auth
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference("scalar", options => options.Layout = ScalarLayout.Classic);
}

using (var scope = app.Services.CreateScope())
{
    // Schedule RecuringJob with Hangfire
    var recurringJobManager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    recurringJobManager.AddOrUpdate<IWeeklyReportJob>(
        "weekly-earnings-report", job => job.ExecuteAsync(), "59 5 * * 1");

    // Seed IdentityUsers
    var services = scope.ServiceProvider;
    await IdentitySeeder.SeedAsync(services, app.Configuration);

    //var backgroundJobs = scope.ServiceProvider.GetRequiredService<IBackgroundJobClient>();
    //backgroundJobs.Enqueue<IWeeklyReportJob>(job => job.ExecuteAsync()); 
}

app.UseHttpsRedirection();

app.UseCors("AllowReact");
app.UseHangfireDashboard();

app.MapGroup("api/auth")
    .MapCustomIdentityApi<IdentityUser>()
    .WithTags("Auth");

app.MapOrderEndpoints();
app.MapWeeklyReportEndpoints();
app.MapExpenseEndpoints();

app.UseAuthentication();
app.UseAuthorization();

app.Run();