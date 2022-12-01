using AutoMapper;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using Sadada.Core.Mangers.MagersInterface;
using Sadada.Models.Static;
using Sadada.ModelViews.Enums;
using Sadadad.EmailService;
using Sadadad.Notifications;
using SadadDbModel.dbContext;
using SadadDbModel.ModelViews;
using System;
using System.Collections.Generic;
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
        private readonly IEmailSender _emailSender;


        public CustmerManger(sadaddbContext sadaddbContext, IMapper mapper,IEmailSender emailSender)
        {
            _sadaddbContext = sadaddbContext;
            _mapper = mapper;
            _emailSender=emailSender;
        }

        public RegisterCompleteView CreateCustmer(CreateCustmerView custmer)
        {
            if (_sadaddbContext.Custmers.Any(s => s.Email.Equals(custmer.Email)&&s.FirstName.Equals(custmer.FirstName)))
            {
                throw new SadadaException("Already Existed");
            }
            custmer.Password = CreateRandomPassword(9);
            var temp = custmer.Password;
            var hashpassword = HashPassword(custmer.Password);

            var newCustmer = _sadaddbContext.Custmers.Add(new Custmer
            {
                FirstName=custmer.FirstName,
                LastName=custmer.LastName,
                Email=custmer.Email,
                Password=hashpassword,
                ConfirmationLink = ""
             }).Entity;

            _sadaddbContext.SaveChanges();
            var builder = new EmailBuilder(ActionInvocationTypeEnum.EmailConfirmation,
                    new Dictionary<string, string>
                    {
                                    { "AssigneeName", $"{custmer.FirstName} {custmer.LastName}" },
                                    { "Link", $"{newCustmer.ConfirmationLink}" }
                    }, "https://localhost:44309");

            var message = new Message(new string[] { custmer.Email }, builder.GetTitle(), builder.GetBody(temp));
            _emailSender.SendEmail(message);

            var res = _mapper.Map<RegisterCompleteView>(newCustmer);

            res.Token = $"Bearer {GenerateJWTToken(newCustmer)}";

            return res;

        }

        public LoginReponseView LogInCustmer(CustmerLoginModel custmer)
        {
            var custmerdb = _sadaddbContext.Custmers
                                 .FirstOrDefault(a => a.Email
                                                         .Equals(custmer.Email));

            if (custmerdb == null || !VerifyHashPassword(custmer.Password, custmerdb.Password))
            {
                throw new ServiceValidationException(300, "Invalid user name or password received");
            }

            var res = _mapper.Map<LoginReponseView>(custmerdb);
            res.Token = $"Bearer {GenerateJWTToken(custmerdb)}";
            return res;
        }



        public List<GetCustmersView> GetAllCustmers()
        {
            var custmersList = _sadaddbContext.Custmers.Select(a=>new GetCustmersView
            {
                FullName=$"{a.FirstName} {a.LastName}",
                TotalDept=a.TotalDept
            }).ToList();

            return custmersList;
        }

        public void RegisterDebt(int custmerId,string productName)
        {
            var custmer=_sadaddbContext.Custmers.FirstOrDefault(a=>a.Id==custmerId)
                                                ??throw new SadadaException("Not Found");

            var product = _sadaddbContext.Products.FirstOrDefault(a => a.Name.Equals(productName));

            var trans = _sadaddbContext.Transactions.Add(new Transaction
            {
                ProductId=product.Id,
                UserId=custmer.Id
            }).Entity;

            custmer.TotalDept = custmer.TotalDept + product.Price;
            _sadaddbContext.Custmers.Update(custmer);
            _sadaddbContext.SaveChanges();
        }

        public ForgetCustmerView ForgetPassword(string email)
        {
            var custmer = _sadaddbContext.Custmers.FirstOrDefault(a => a.Email.Equals(email))
                                                    ?? throw new SadadaException("Not Found");

            custmer.ConfirmationLink = Guid.NewGuid().ToString().Replace("-", "").ToString();
            custmer.IsConfirmed = false;

            var builder = new EmailBuilder(ActionInvocationTypeEnum.ResetPassword,
             new Dictionary<string, string>
             {
                                    { "AssigneeName", $"{custmer.FirstName} {custmer.LastName}" },
                                    { "Link", $"{custmer.ConfirmationLink}" }
             }, "https://localhost:44309");

            var message = new Message(new string[] { custmer.Email }, builder.GetTitle(), builder.GetBody(""));
            _emailSender.SendEmail(message);
           
            var mapped = _mapper.Map<ForgetCustmerView>(custmer);
            mapped.Token = $"Bearer {GenerateJWTToken(custmer)}";
            _sadaddbContext.SaveChanges();
            return mapped;
        }

        public CustmerModel ConfiremPassword(string confirmation)
        {
            var user = _sadaddbContext.Custmers
               .FirstOrDefault(a => a.ConfirmationLink
                                        .Equals(confirmation)
                                         && !a.IsConfirmed)
           ?? throw new ServiceValidationException("Invalid or expired confirmation link received");

            user.IsConfirmed = true;
            user.ConfirmationLink = string.Empty;
            _sadaddbContext.SaveChanges();
            return _mapper.Map<CustmerModel>(user);
        }

        public CustmerModel ResetPassword(CustmerModel forgetenCustemr,ResetPasswordView passwordView)
        {
            var custmer=_sadaddbContext.Custmers.FirstOrDefault(a=>a.Id == forgetenCustemr.Id)
                                                 ??throw new SadadaException("Not found");
            if (!custmer.IsConfirmed)
            {
                throw new SadadaException("Not Confirmed");
            }
            if (passwordView.NewPassword == passwordView.ConfirmPassword)
            {
                custmer.Password=HashPassword(passwordView.ConfirmPassword);
                _sadaddbContext.Update(custmer);
                _sadaddbContext.SaveChanges();
            }
            return _mapper.Map<CustmerModel>(custmer);
            
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
