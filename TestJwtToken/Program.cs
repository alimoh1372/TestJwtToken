using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Tokens;
using TestJwtToken.Models.Utility;
using TestJwtToken.Models.ViewModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthHelper, AuthHelper>();
//Adding jwtBearer as default authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(jwtOption =>
    {
        jwtOption.RequireHttpsMetadata=false;
        jwtOption.SaveToken = true;
        jwtOption.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Constants.JWT_SECURITY_KEY_FOR_SIGNETURE)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapGet("getToken", [AllowAnonymous] (LoginFieldRequest user,IAuthHelper _authHelper) =>
{
    if (user.UserName=="ali" && user.Password=="password")
    {
        var tokenExpire = DateTime.Now.AddMinutes(Constants.JWT_TOKEN_EXPIRE_TIME_MINUTE);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Constants.JWT_SECURITY_KEY_FOR_SIGNETURE);

        var signingKey = new SymmetricSecurityKey(key);
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, "Alimoha@gmail.com")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = tokenExpire,
            IssuedAt = DateTime.Now,
            NotBefore = DateTime.Now.AddSeconds(-10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha512Signature),
            Subject = new ClaimsIdentity(claims),
        };
        var securityTokenFactory = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityTokenFactory);

        return Results.Ok(token);

    }

    return Results.NotFound();
});

app.MapGet("getMessage", [Authorize] () => Results.Ok("Hello to You"));
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


