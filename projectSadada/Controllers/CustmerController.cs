﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sadada.Core.Mangers.MagersInterface;
using SadadDbModel.dbContext;
using SadadDbModel.ModelViews;
using System.Collections.Generic;


namespace projectSadada.Controllers
{
    [Route("[controller]")]
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

        [HttpGet("GetCustmerDetilesByID")]
        public IActionResult GetCustmerDetilesByID(int Id)
        {
            var res = _custmerManger.GetCustmerDetilesByID(Id);

            return Ok(res);
        }

        [Route("GetAllCustmers")]
        [HttpGet]
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
        public IActionResult ForgetPassword(FrogetPasswordModel paylod)
        {
            var custmer = _custmerManger.ForgetPassword(paylod);

            return Ok(custmer);

        }
        [Route("ConfiremPassword")]
        [HttpPost]
        public IActionResult ConfiremPassword(ConfirmModel paylod)
        {
            var res = _custmerManger.ConfiremPassword(paylod);

            return Ok(res);
        }
        [Route("ResetPassword")]
        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordView reset)
        {
            var res = _custmerManger.ResetPassword(reset);

            return Ok(res);
        }

        [Route("GetAllTranstations")]
        [HttpGet]
        public IActionResult GetAllTranstations()
        {
            var res = _custmerManger.GetAllTranstations();

            return Ok(res);
        }

        [Route("EditCustmerDept")]
        [HttpPut]
        public IActionResult EditCustmerDept(EditCustmerDeptView cus)
        {
            _custmerManger.EditCustmerDept(cus);

            return Ok("Done!");
        }


    }
}
