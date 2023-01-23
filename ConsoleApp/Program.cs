using Azure.Identity;
using ConsoleApp.Data;
using ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

await using var db = new FmidDb();

var app1 = new FmidApplication { Name = "FMID App 1" };
app1.Roles.Add(new Role { Name = "FMID App 1 Role 1", Description = "First role", Value = "fmid-app-1-role-1" });
app1.Roles.Add(new Role { Name = "FMID App 1 Role 2", Description = "Second role", Value = "fmid-app-1-role-2" });
db.Applications.Add(app1);

// var app2 = new FmidApplication { Name = "FMID App 2" };
// app2.Roles.Add(new Role { Name = "FMID App 2 Role 1", Description = "First role", Value = "fmid-app-2-role-1" });
// db.Applications.Add(app2);

db.SaveChanges();

// Read appsettings.json and from secrets.json
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddUserSecrets<Program>()
    .Build();

// Retrieve environment variables from appsettings.json
var tenantId = config["TenantId"];
var clientId = config["ClientId"];
var clientSecret = config["ClientSecret"];

// Register Microsoft Graph client using client credentials
var clientSecretCredential = new ClientSecretCredential(
    tenantId, clientId, clientSecret);
        
var graphServiceClient = new GraphServiceClient(clientSecretCredential);

// Retrieve all fmid  applications with roles
var fmidApplications = db.Applications
    .Include(a => a.Roles)
    .ToList();
    
// Create a new App Registration for each fmid application

 foreach (var fmidApplication in fmidApplications)
 {
     var appRegistrationToCreate = fmidApplication.ToAppRegistration();
     var newApp = await graphServiceClient
         .Applications
         .Request()
         .AddAsync(appRegistrationToCreate);
     Console.WriteLine($"Created {newApp.DisplayName} with {newApp.AppRoles.Count()} roles");
 }
 