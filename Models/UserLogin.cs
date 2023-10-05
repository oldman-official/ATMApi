namespace ATM.Models;

public class UserLogin {
    public string CardNumber { get; set; }
    public string Password { get; set; }
    
    public UserLogin() {
        if (CardNumber == null) {
            CardNumber = "";
        }
        if (Password == null) {
            Password = "";
        }
    }

}