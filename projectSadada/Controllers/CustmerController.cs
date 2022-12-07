using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sadada.Core.Mangers.MagersInterface;
using SadadDbModel.dbContext;
using SadadDbModel.ModelViews;
using System.Collections.Generic;


namespace projectSadada.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustmerController : ApiBaseController
    {
        private ICustmerManger _custmerManger;

        private IHttpContextAccessor httpContextAccessor;

        public CustmerController(ICustmerManger custmerManger, IHttpContextAccessor httpContextAccessor)
        {
            _custmerManger = custmerManger;
            this.httpContextAccessor = httpContextAccessor;
        }


        [Route("Register")]
        [HttpPost]
        public IActionResult RegisterCustmer(CreateCustmerView custmer)
        {
            var res = _custmerManger.CreateCustmer(custmer);
            return Ok(res);
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [Route("GetAllCustmers")]
        [HttpGet]
        [Authorize]
        public IActionResult GetAllCustmers()
        {
            var res = _custmerManger.GetAllCustmers();

            return Ok(res);
        }

        [Route("RegisterDept")]
        [HttpPost]

        public IActionResult RegisterDebt(int custmerId,string productName)
        {
            _custmerManger.RegisterDebt(custmerId,productName);

            return Ok();
        }

        [Route("LoginInCustmer")]
        [HttpPost]
        public IActionResult LoginInCustmer(CustmerLoginModel custmer)
        {
            var res = _custmerManger.LogInCustmer(custmer);

            return Ok(res);
        }

        [Route("ForgetPassword")]
        [HttpPost]
        public IActionResult ForgetPassword(ForgetPasswordModel payload)
        {
            var result = this.httpContextAccessor;
            var keyValue = result.HttpContext.Request.Headers["User-Agent"].ToString();
            var custmer=_custmerManger.ForgetPassword(payload.Email);

            return Ok(custmer);

        }
        [Route("ConfiremPassword")]
        [HttpPost]
        public IActionResult ConfiremPassword(ConfirmCodeModel payload)
        {
            var res = _custmerManger.ConfiremPassword(payload.Code);

            return Ok(res);
        }
        [Route("ResetPassword")]
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordView reset)
        {
            var res = _custmerManger.ResetPassword(ForgetpasswordUser, reset);

            return Ok(res);
        }

        


    }
}
