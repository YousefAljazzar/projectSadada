using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Sadada.Core.Mangers.MagersInterface;
using SadadDbModel.dbContext;
using SadadDbModel.ModelViews;
using System.Linq;
using ToDoList.Common.Extensions;

namespace projectSadada.Controllers
{
    public class ApiBaseController : Controller
    {
        private CustmerModel _loggedInUser;

        private CustmerModel _forgetpasswordUser;
        protected CustmerModel LoggedInUser
        {
            get
            {
                if (_loggedInUser != null)
                {
                    return _loggedInUser;
                }

                Request.Headers.TryGetValue("Authorization", out StringValues Token);

                if (string.IsNullOrWhiteSpace(Token))
                {
                    _loggedInUser = null;
                    return _loggedInUser;
                }

                var ClaimId = User.Claims.FirstOrDefault(c => c.Type == "Id");

                _ = int.TryParse(ClaimId.Value, out int idd);

                if (ClaimId == null || !int.TryParse(ClaimId.Value, out int id))
                {
                    throw new ServiceValidationException(401, "Invalid or expired token");
                }

                var commonManager = HttpContext.RequestServices.GetService(typeof(ICommonManger)) as ICommonManger;

                _loggedInUser = commonManager.GetUserRole(new CustmerModel { Id = id });

                return _loggedInUser;
            }
        }

        protected CustmerModel ForgetpasswordUser
        {
            get
            {
                if (_forgetpasswordUser != null)
                {
                    return _forgetpasswordUser;
                }

                Request.Headers.TryGetValue("Authorization", out StringValues Token);

                if (string.IsNullOrWhiteSpace(Token))
                {
                    _forgetpasswordUser = null;
                    return _forgetpasswordUser;
                }

                var ClaimId = User.Claims.FirstOrDefault(c => c.Type == "Email");

                var email = ClaimId.Value;
                //_ = int.TryParse(ClaimId.Value, out int idd);

                if (ClaimId == null )
                {
                    throw new ServiceValidationException(401, "Invalid or expired token");
                }

                var commonManager = HttpContext.RequestServices.GetService(typeof(ICommonManger)) as ICommonManger;

                _forgetpasswordUser = commonManager.GetUserRole(new CustmerModel { Email = email });

                return _forgetpasswordUser;
            }
        }
    }
}
