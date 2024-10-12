using ApproveAppApi;
using ApproveAppApi.Data;
using ApproveAppApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(gen =>
{
    gen.DocumentFilter<CustomerSwaggerFilter>();
    gen.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
}
);
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi); // will be ignored if run locally
builder.Services.AddScoped<PermitService>();
// Dependency Injection of DbContext Class
//builder.Services.AddDbContext<ApproveAPIDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Get the encrypted connection string from appsettings.json
var secretsHelper = new SecretsManagerHelper();
var secretJson = await secretsHelper.GetSecretAsync();
var encryptedConnectionString = secretJson.Count > 0 ? secretJson["connectionString"] : builder.Configuration.GetConnectionString("DefaultConnection");

// Decrypt the connection string
var decryptor = new StringDecryptor("Group6CampersitePassword");
var decryptedConnectionString = decryptor.Decrypt(encryptedConnectionString);

// Configure the DbContext with the decrypted connection string 
builder.Services.AddDbContext<ApproveAPIDbContext>(options =>
    options.UseSqlServer(decryptedConnectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
