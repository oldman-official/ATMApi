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
        // get user data and then compare
        // if (user.password == user.confirmPassword) {
        //     Console.WriteLine(user.firstName + "  " + user.lastName + "  " + user.dateOfBirth);
        //     return Ok();
        // }
        // throw new Exception("Your Passwords Don't Match");
        return Ok();
    }
}
