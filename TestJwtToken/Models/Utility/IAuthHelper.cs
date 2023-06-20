using TestJwtToken.Models.ViewModel;

namespace TestJwtToken.Models.Utility;

public interface IAuthHelper
{
    JwtTokenField Authenticate(LoginFieldRequest request);
}