// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'Program.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Seacraft.Server.Configurations;
using Seacraft.Server.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddOptions().AddHttpContextAccessor().AddHttpClient();
builder.Services.AddApiRoutingConfiguration().AddControllers(optipns => 
{
    optipns.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}).AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration(builder.Configuration);
builder.Services.AddAuthorizationConfiguration(builder.Configuration);

await Task.Delay(100);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwaggerConfig();
app.UseIdentityServer();

app.UseAuthorization();

app.MapControllers();

app.Run();
