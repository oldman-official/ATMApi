using System.Data;
using System.Security.Cryptography;
using ATM.Data;
using ATM.DTOs;
using ATM.Models;
using ATM.Helpers;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace ATM_Project_API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserAccountController : ControllerBase
{
        private readonly DataContextDapper _dapper;
        private readonly UserAccountAuthHelper _authHelper;
    public UserAccountController(IConfiguration config) {
        _dapper = new DataContextDapper(config);
        _authHelper = new UserAccountAuthHelper(config);
    }
    [HttpPost("register")]
    public IActionResult RegisterUser(UserRegisteration user) {
        if (user.Password == user.ConfirmPassword) {
            Random rng = new Random();
            //TODO : We may have duplicated values for card number , needs to be fixed
            user.CardNumber = "6037997" + rng.Next(100000000 , 999999999).ToString();
            Console.WriteLine(user.FirstName + "  " + user.LastName + "  " + user.Birthday + "  " + user.Password + "  " + user.Balance + "   " + user.CardNumber);
            string registerSql = "EXEC ATMCore.spRegister_User @FirstName = @FirstNameParam , @LastName = @LastNameParam , @NationalId = @NationalIdParam , @Birthday = @BirthdayParam , @CardNumber = @CardNumberParam";
            DynamicParameters registerParameters = new DynamicParameters();
            registerParameters.Add("@FirstNameParam" , user.FirstName , DbType.String);
            registerParameters.Add("@LastNameParam" , user.LastName , DbType.String);
            registerParameters.Add("@NationalIdParam" , user.NationalId , DbType.String);
            registerParameters.Add("@BirthdayParam" , user.Birthday , DbType.Date);
            registerParameters.Add("@CardNumberParam" , user.CardNumber , DbType.String);
            // string authSql = "EXEC ATMCore.spAuth_Insert_User @Password , @PasswordSalt";
            if (_dapper.ExecuteBoolWithParam(registerSql , registerParameters)) {
                AuthDataDto authDataDto = new(user.CardNumber , user.Password);
                if (_authHelper.SetPassword(authDataDto)) {
                    return Ok();
                }
                throw new Exception ("Failed To Add Auth Data");
            }
            throw new Exception("Failed To Register User");
        }
        throw new Exception("Your Passwords Don't Match!");

    }
    [HttpPost("login")]
    public IActionResult LoginUser(UserLogin user) {
        string sql = $"SELECT PasswordHash , PasswordSalt FROM ATMCore.Auth WHERE CardNumber = {user.CardNumber}";
        // Console.WriteLine(sql);
        UserLoginConfirmationDto passAndSalt = _dapper.LoadDataSingle<UserLoginConfirmationDto>(sql);
        byte[] inputedPass = _authHelper.GetHashedPassword(user.Password , passAndSalt.PasswordSalt);
        for (int i = 0 ; i < inputedPass.Length ; i++) {
            if (inputedPass[i] != passAndSalt.PasswordHash[i]) {
                return StatusCode(401 , "Incorrect Password!");
            }
        }
            string getUserIdSql = $@" SELECT [Users].[UserId]     
                                    FROM ATMCORE.Users AS Users LEFT JOIN ATMCORE.Auth AS Auth ON Users.CardNumber = Auth.CardNumber WHERE Users.CardNumber = {user.CardNumber}";
            int userId = _dapper.LoadDataSingle<int>(getUserIdSql);
            return Ok(
                new Dictionary<string , string> {
                    {"Token" , _authHelper.CreateToken(userId)}
                }
            );
        }
    }

