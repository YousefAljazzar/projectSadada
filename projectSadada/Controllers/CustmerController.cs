using Microsoft.AspNetCore.Mvc;
using Sadada.Core.Mangers.MagersInterface;
using SadadDbModel.dbContext;
using SadadDbModel.ModelViews;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        [HttpPost]
        public IActionResult RegisterCustmer(CreateCustmerView custmer)
        {
            var res = _custmerManger.CreateCustmer(custmer);
            return Ok(res);
        }

        // GET api/<CustmerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

  

        // PUT api/<CustmerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CustmerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
