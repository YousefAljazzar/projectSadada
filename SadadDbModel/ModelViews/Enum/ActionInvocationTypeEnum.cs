using System.ComponentModel;

namespace Sadada.ModelViews.Enums
{
    public enum ActionInvocationTypeEnum
    {
        [Description("Email Confirmation")]
        EmailConfirmation = 1,
        [Description("Reset Password")]
        ResetPassword = 2
    }
}
