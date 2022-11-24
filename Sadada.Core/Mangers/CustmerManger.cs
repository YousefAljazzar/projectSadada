using AutoMapper;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using Sadada.Core.Mangers.MagersInterface;
using SadadDbModel.dbContext;
using SadadDbModel.ModelViews;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ToDoList.Common.Extensions;


namespace Sadada.Core.Mangers
{
    public class CustmerManger : ICustmerManger
    {
        private readonly sadaddbContext _sadaddbContext;
        private IMapper _mapper;

        public CustmerManger(sadaddbContext sadaddbContext, IMapper mapper)
        {
            _sadaddbContext = sadaddbContext;
            _mapper = mapper;
        }

        public RegisterCompleteView CreateCustmer(CreateCustmerView custmer)
        {
            if (_sadaddbContext.Custmers.Any(s => s.Email.Equals(custmer.Email)&&s.FirstName.Equals(custmer.FirstName)))
            {
                throw new SadadaException("Already Existed");
            }

            var hashpassword = HashPassword(custmer.Password);

            var newCustmer = _sadaddbContext.Custmers.Add(new Custmer
            {
                FirstName=custmer.FirstName,
                LastName=custmer.LastName,
                Email=custmer.Email,
                Password=hashpassword,
            }).Entity;

            _sadaddbContext.SaveChanges();

            var res = _mapper.Map<RegisterCompleteView>(newCustmer);

            res.Token = $"Bearer {GenerateJWTToken(newCustmer)}";

            return res;

        }







        #region private 

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
