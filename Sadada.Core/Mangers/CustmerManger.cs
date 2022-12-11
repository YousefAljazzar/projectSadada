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
using Sadada.Common.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Sadada.Core.Mangers
{
    public class CustmerManger : ICustmerManger
    {
        private readonly sadaddbContext _sadaddbContext;
        private IMapper _mapper;
        private readonly IEmailSender _emailSender;


        public CustmerManger(sadaddbContext sadaddbContext, IMapper mapper, IEmailSender emailSender)
        {
            _sadaddbContext = sadaddbContext;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        public RegisterCompleteView CreateCustmer(CreateCustmerView custmer)
        {
            if (_sadaddbContext.Custmers.Any(s => s.Email.Equals(custmer.Email) && s.FirstName.Equals(custmer.FirstName)))
            {
                throw new SadadaException("Already Existed");
            }
            var password = CreateRandomPassword(9);

            var hashpassword = HashPassword(password);

            var newCustmer = _sadaddbContext.Custmers.Add(new Custmer
            {
                FirstName = custmer.FirstName,
                LastName = custmer.LastName,
                Email = custmer.Email,
                Password = hashpassword,
                ConfirmationLink = String.Empty
            }).Entity;

            _sadaddbContext.SaveChanges();
            var builder = new EmailBuilder(ActionInvocationTypeEnum.EmailConfirmation,
                    new Dictionary<string, string>
                    {
                                    { "AssigneeName", $"{custmer.FirstName} {custmer.LastName}" },
                                    { "Link", $"{newCustmer.ConfirmationLink}" }
                    }, "https://localhost:44309");

            var message = new Message(new string[] { custmer.Email }, builder.GetTitle(), builder.GetBody(password));
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
            var custmersList = _sadaddbContext.Custmers.Select(a => new GetCustmersView
            {
                Id = a.Id,
                FullName = $"{a.FirstName} {a.LastName}",
                TotalDept = a.TotalDept
            }).ToList();

            return custmersList;
        }

        public PaginationView GetAllCustmersWithPagination(int page = 1, int pageSize = 10, string sortColumn = "", string sortDirection = "ascending", string searchText = "")
        {
            var queryRes = _sadaddbContext.Custmers
                                        .Where(a => string.IsNullOrWhiteSpace(searchText)
                                                    || (a.FirstName.Contains(searchText)
                                                        || a.LastName.Contains(searchText)));

            if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.Equals("ascending", StringComparison.InvariantCultureIgnoreCase))
            {
                queryRes = queryRes.OrderBy(sortColumn);
            }
            else if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.Equals("descending", StringComparison.InvariantCultureIgnoreCase))
            {
                queryRes = queryRes.OrderByDescending(sortColumn);
            }

            var res = queryRes.GetPaged(page, pageSize);

            var todoIds = res.Data
                             .Select(a => a.Id)
                             .Distinct()
                             .ToList();

            var todos = _sadaddbContext.Transactions.ToDictionary(a => a.Id, x => _mapper.Map<TransactionModelView>(x));

            var data = new PaginationView()
            {
                Custmer = _mapper.Map<PagedResult<CustmerModel>>(res),
                Transactions = todos
            };

            data.Custmer.Sortable.Add("Email", "Email");
            data.Custmer.Sortable.Add("CreatedDate", "Created Date");

            return data;
        }


        public void RegisterDebt(AddDeptToCustmerView deptCustmer)
        {
            var custmer = _sadaddbContext.Custmers.FirstOrDefault(a => a.FirstName == deptCustmer.FirstName && a.LastName == deptCustmer.LastName)
                                                ?? throw new SadadaException(300, "Not Found");

            var product = _sadaddbContext.Products.FirstOrDefault(a => a.Name.Equals(deptCustmer.Product));

            var trans = _sadaddbContext.Transactions.Add(new Transaction
            {
                ProductId = product.Id,
                UserId = custmer.Id,
                Quantity = deptCustmer.Quantity
            }).Entity;

            custmer.TotalDept = custmer.TotalDept + (product.Price * deptCustmer.Quantity);
            _sadaddbContext.Custmers.Update(custmer);
            _sadaddbContext.SaveChanges();

        }

        public ForgetCustmerView ForgetPassword(FrogetPasswordModel payload)
        {
            var custmer = _sadaddbContext.Custmers.FirstOrDefault(a => a.Email.Equals(payload.Email))
                                                    ?? throw new SadadaException("Not Found");

            custmer.ConfirmationLink = Guid.NewGuid().ToString().Replace("-", "").ToString();
            custmer.IsConfirmed = false;

            var builder = new EmailBuilder(ActionInvocationTypeEnum.ResetPassword,
             new Dictionary<string, string>
             {
                                    { "AssigneeName", $"{custmer.FirstName} {custmer.LastName}" },
                                    { "Link", $"{custmer.ConfirmationLink}" }
             }, "http://127.0.0.1:5500/newpassword.html");


            var message = new Message(new string[] { custmer.Email }, builder.GetTitle(), builder.GetBody(""));
            _emailSender.SendEmail(message);
            var mapped = _mapper.Map<ForgetCustmerView>(custmer);
            mapped.Token = $"Bearer {GenerateJWTToken(custmer)}";
            _sadaddbContext.Update(custmer);
            _sadaddbContext.SaveChanges();
            return mapped;
        }

        public CustmerModel ConfiremPassword(ConfirmModel confirmation)
        {
            var user = _sadaddbContext.Custmers
               .FirstOrDefault(a => a.ConfirmationLink
                                        .Equals(confirmation.Code)
                                         && !a.IsConfirmed)
           ?? throw new ServiceValidationException("Invalid or expired confirmation link received");

            user.IsConfirmed = true;
            user.ConfirmationLink = string.Empty;
            _sadaddbContext.SaveChanges();
            return _mapper.Map<CustmerModel>(user);
        }

        public CustmerModel ResetPassword(CustmerModel forgetenCustemr, ResetPasswordView passwordView)
        {
            var custmer = _sadaddbContext.Custmers.FirstOrDefault(a => a.Id == forgetenCustemr.Id)
                                                 ?? throw new SadadaException("Not found");
            if (!custmer.IsConfirmed)
            {
                throw new SadadaException("Not Confirmed");
            }
            if (passwordView.NewPassword == passwordView.ConfirmPassword)
            {
                custmer.Password = HashPassword(passwordView.ConfirmPassword);
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
