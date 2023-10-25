using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ATM.Data;
using ATM.DTOs;
using Dapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace ATM.Helpers;

public class UserAccountAuthHelper {
    private readonly IConfiguration _config;
    private readonly DataContextDapper _dapper;
    public UserAccountAuthHelper(IConfiguration config) {
        _config = config;
        _dapper = new DataContextDapper(config);
    }
    public byte[] GetHashedPassword(string pass ,byte[] passSalt) {
        string saltAndStr = Convert.ToBase64String(passSalt) + _config.GetSection("AppSettings:PasswordKey").Value;
        return KeyDerivation.Pbkdf2(
            pass , 
            Encoding.ASCII.GetBytes(saltAndStr) , 
            KeyDerivationPrf.HMACSHA256 , 
            1000000 , 
            256/8
        );
    }
    public bool SetPassword(AuthDataDto userAuthData) {
        byte[] salt = new byte[128/8];
        using(RandomNumberGenerator rng = RandomNumberGenerator.Create()) {
            rng.GetNonZeroBytes(salt);
        }
        byte[] passwordHashed = GetHashedPassword(userAuthData.Password , salt);
        string sql = "EXEC ATMCore.spInsert_User_Auth @CardNumber = @CardNumberParam , @Password = @PasswordParam , @PasswordSalt = @PasswordSaltParam";
        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("@CardNumberParam" , userAuthData.CardNumber , DbType.String);
        parameters.Add("@PasswordParam" , passwordHashed , DbType.Binary);
        parameters.Add("@PasswordSaltParam" , salt , DbType.Binary);
        return _dapper.ExecuteBoolWithParam(sql , parameters);
    }
    public string CreateToken(int UserId) {
        Claim[] claims = new Claim[] {
            new Claim("UserId" , UserId.ToString()) ,
        };
        SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:symKey").Value)
        );
        SigningCredentials credentials = new SigningCredentials(
            symmetricKey ,
            SecurityAlgorithms.HmacSha512Signature
        );
        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor() {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = credentials ,
            Expires = DateTime.Now.AddHours(1)
        };
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken = tokenHandler.CreateToken(descriptor);
        return tokenHandler.WriteToken(securityToken);        
    }
}