namespace ATM.Models;

public class UserRegisteration {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalId { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string CardNumber { get; set; }
    public decimal Balance { get; set;}
    public DateTime Birthday { get; set; }
    
    public UserRegisteration() {
        if (FirstName == null) {
            FirstName = "";
        }
        if (LastName == null) {
            LastName = "";
        }
        if (NationalId == null) {
            NationalId = "";
        }
        if (Password == null) {
            Password = "";
        }
        if (ConfirmPassword == null) {
            ConfirmPassword = "";
        }
        if (CardNumber == null) {
            CardNumber = "";
        }
    }

}