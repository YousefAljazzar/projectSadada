using Microsoft.AspNetCore.Authorization;
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

        public CustmerController(ICustmerManger custmerManger)
        {
            _custmerManger = custmerManger;
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
        [Route("GetAllCustmersWithPagination")]
        [HttpGet]
        public IActionResult GetAllCustmersWithPagination(int page, int pagesize, string sortColum, string dircation, string serach)
        {
            var res = _custmerManger.GetAllCustmersWithPagination(page, pagesize, sortColum, dircation, serach);

            return Ok(res);
        }

        [Route("RegisterDept")]
        [HttpPost]

        public IActionResult RegisterDebt(AddDeptToCustmerView deptCustmer)
        {
            _custmerManger.RegisterDebt(deptCustmer);

            return Ok("Add Succseflly");
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
        public IActionResult ForgetPassword(string email)
        {
            var custmer = _custmerManger.ForgetPassword(email);

            return Ok(custmer);

        }
        [Route("ConfiremPassword")]
        [HttpPost]
        public IActionResult ConfiremPassword(string confirmation)
        {
            var res = _custmerManger.ConfiremPassword(confirmation);

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
