namespace TestJwtToken.Models.ViewModel;
[Serializable]
public class JwtTokenField
{
    //Base64

    public string Token { get; set; }
    public string  User_Name { get; set; }
    public int expireTime { get; set; } 

}