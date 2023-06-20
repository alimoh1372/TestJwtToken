namespace TestJwtToken.Models.ViewModel;
[Serializable]
public class LoginFieldRequest 
{
    public string UserName { get; set; }
    public string Password { get; set; }

}