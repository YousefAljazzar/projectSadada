using Microsoft.AspNetCore.Mvc;
using Sadada.Core.Mangers.MagersInterface;
using SadadDbModel.ModelViews;


namespace projectSadada.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IUserManger _userManger;

        public UserController(IUserManger userManger)
        {
            _userManger = userManger;
        }

        [HttpPost]
        [Route("UserLogin")]
        public IActionResult UserLogin(UserLoginRequest user)
        {
            var res = _userManger.LoginInUser(user);

            return Ok(res);
        }

        [HttpGet]
        [Route("GetAllTranstationsById")]
        public IActionResult GetAllTranstationsById(int Id)
        {
            var res = _userManger.GetAllTranstationsById(Id);

            return Ok(res);
        }


    }
}
