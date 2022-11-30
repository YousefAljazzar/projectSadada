using Microsoft.AspNetCore.Mvc;
using Sadada.Core.Mangers.MagersInterface;
using SadadDbModel.dbContext;
using SadadDbModel.ModelViews;
using System.Collections.Generic;


namespace projectSadada.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustmerController : ControllerBase
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

    }
}
