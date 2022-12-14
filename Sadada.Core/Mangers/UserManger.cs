using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using Sadada.Common.Extensions;
using Sadada.Core.Mangers.MagersInterface;
using SadadDbModel.dbContext;
using SadadDbModel.ModelViews;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sadada.Core.Mangers
{
    public class UserManger : IUserManger
    {
        private readonly sadaddbContext _sadaddbContext;
        private IMapper _mapper;

        public UserManger(sadaddbContext sadaddbContext, IMapper mapper)
        {
            _sadaddbContext = sadaddbContext;
            _mapper = mapper;
        }

        public UserLoginView LoginInUser(UserLoginRequest logUser)
        {
            var user = _sadaddbContext.Custmers.FirstOrDefault(a => a.Email.Equals(logUser.Email))
                                                ?? throw new SadadaException("Invalid");

            if (!VerifyHashPassword(logUser.Password, user.Password))
            {
                throw new SadadaException("Invalid");
            }
            var res = _mapper.Map<UserLoginView>(user);
            res.Token = $"Bearer {GenerateJWTToken(user)}";
            return res;
        }













        #region private 


        private static string CreateRandomPassword(int length = 15)
        {
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        private static string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            return hashedPassword;
        }

        private static bool VerifyHashPassword(string password, string HashedPasword)
        {
            return BCrypt.Net.BCrypt.Verify(password, HashedPasword);
        }

        private string GenerateJWTToken(Custmer Custmer)
        {
            var jwtKey = "#test.key*&^vanthis%$^&*()$%^@#$@!@#%$#^%&*%^*";
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, $"{Custmer.FirstName} {Custmer.LastName}"),
                new Claim(JwtRegisteredClaimNames.Email, Custmer.Email),
                new Claim("Id", Custmer.Id.ToString()),
                new Claim("FirstName", Custmer.FirstName),
                new Claim("Email", Custmer.Email),
                new Claim("DateOfJoining", Custmer.CreatedDate.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var issuer = "test.com";

            var token = new JwtSecurityToken(
                        issuer,
                        issuer,
                        claims,
                        expires: DateTime.Now.AddDays(20),
                        signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        #endregion private 
    }
}
