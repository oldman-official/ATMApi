namespace ATM.DTOs;

public class AuthDataDto {
    public string CardNumber { get; set; }
    public string Password { get; set; }
    
    public AuthDataDto(string _cardNumber , string _password) {
        CardNumber = _cardNumber;
        Password = _password;
    }

}