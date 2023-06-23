// Copyright(c) 2023 Seacraft. All Rights Reserved.
// The Seacraft licenses 'Program.cs' file under the MIT license.
// See the 'LICENSE' file in the project repository for more information.

using Seacraft.Server.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddOptions().AddHttpContextAccessor().AddHttpClient();
builder.Services.AddControllers(optipns => 
{
    optipns.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}).AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration(builder.Configuration);
builder.Services.AddAuthorizationConfiguration(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseIdentityServer();

app.UseAuthorization();

app.MapControllers();

app.Run();
