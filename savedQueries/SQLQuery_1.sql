USE ATMDb
CREATE TABLE ATMCore.Users (
    UserId int PRIMARY KEY IDENTITY(1,1),
    FirstName varchar(255),
    LastName varchar(255),
    NationalId varchar(255),
    Balance decimal(18,4) ,
    Birthday DATE ,
    CardNumber varchar(255)
);
SELECT * FROM ATMCore.Users
-- TRUNCATE TABLE ATMCore.Users 
GO
CREATE OR ALTER PROCEDURE ATMCore.spRegister_User
    -- EXEC ATMCore.spRegister_User @FirstName = 'Seyed MohammadHosein' , @LastName = 'Farhani' , @NationalId = '0110238374' , @Birthday = 	'2002-11-11' , @CardNumber = '6037997326956748'
    @FirstName varchar(255),
    @LastName varchar(255),
    @NationalId varchar(255),
    @Balance decimal(18,4) = 0.00,
    @Birthday DATE ,
    @CardNumber varchar(255)
AS
BEGIN 
    INSERT INTO ATMCore.Users VALUES (@FirstName , @LastName , @NationalId , @Balance , @Birthday , @CardNumber)
END

GO 
CREATE TABLE ATMCore.Auth (
    CardNumber VARCHAR(255),
    PasswordHash VARBINARY(MAX) ,
    PasswordSalt VARBINARY(MAX) ,
);
SELECT * FROM ATMCore.Auth
GO 



CREATE OR ALTER PROCEDURE ATMCore.spInsert_User_Auth
    -- EXEC ATMCore.spRegister_User_Auth @CardNumber = '6037997234675012' , @Password
    @CardNumber VARCHAR(255) ,
    @Password VARBINARY(MAX) , 
    @PasswordSalt VARBINARY(MAX)
AS
BEGIN 
    INSERT INTO ATMCore.Auth VALUES (@CardNumber , @Password , @PasswordSalt);
END



