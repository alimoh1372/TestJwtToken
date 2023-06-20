using System.Buffers.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TestJwtToken.Models.ViewModel;

namespace TestJwtToken.Models.Utility;

public class AuthHelper : IAuthHelper
{
    public JwtTokenField Authenticate(LoginFieldRequest request)
    {
        if (request.UserName.ToLower() != "ali".ToLower()!)
            return null;


        if (request.Password != "password")
            return null;

        var tokenExpireTimeStamp = DateTime.Now.AddMinutes(Constants.JWT_TOKEN_EXPIRE_TIME_MINUTE);
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var tokenSignutureSecurityKey = Encoding.ASCII.GetBytes(Constants.JWT_SECURITY_KEY_FOR_SIGNETURE);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, request.UserName),
            new Claim("UserGroup 01", "GroupNumber 1")
        };

        var securityTokenDescriptor = new SecurityTokenDescriptor
        {

            Expires = tokenExpireTimeStamp,
            IssuedAt = DateTime.Now,
            NotBefore = DateTime.Now.AddSeconds(-20),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenSignutureSecurityKey),
                SecurityAlgorithms.HmacSha256Signature),

            Subject = new ClaimsIdentity(claims),

        };
        var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
        var token = jwtSecurityTokenHandler.WriteToken(securityToken);
        return new JwtTokenField
        {
            Token = token,
            User_Name = request.UserName,
            expireTime =(int) tokenExpireTimeStamp.Subtract(DateTime.Now).TotalSeconds
        };
    }
}