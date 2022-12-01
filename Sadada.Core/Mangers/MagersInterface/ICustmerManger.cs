using SadadDbModel.dbContext;
using SadadDbModel.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sadada.Core.Mangers.MagersInterface
{
    public interface ICustmerManger
    {
        public RegisterCompleteView CreateCustmer(CreateCustmerView custmer);

        public List<GetCustmersView> GetAllCustmers();

        public void RegisterDebt( int custmerId, string productName);

        public LoginReponseView LogInCustmer(CustmerLoginModel custmer);

        public ForgetCustmerView ForgetPassword(string email);

        public CustmerModel ConfiremPassword(string confirmation);

        public CustmerModel ResetPassword(CustmerModel forgetenCustemr, ResetPasswordView passwordView);
    }
}
