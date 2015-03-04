using Android.Content;
using AvantCredit.Uploader.Activities;
using AvantCredit.Uploader.Core.Command;
using AvantCredit.Uploader.Core.Security;

namespace AvantCredit.Uploader.Commands
{
    class LogoutCommand : Command
    {
        private readonly IUserAuthService _userAuthService;

        public LogoutCommand(IUserAuthService userAuthService, Context context) 
            : base(context)
        {
            _userAuthService = userAuthService;
        }

        protected override void OnExecution(ExecuteCommandEventArgs e)
        {
            base.OnExecution(e);

            _userAuthService.Logout();

            var intent = new Intent(Context, typeof (LoginActivity));

            // close all activities
            intent.AddFlags(ActivityFlags.ClearTop);
            intent.AddFlags(ActivityFlags.NewTask);
            Context.StartActivity(intent);
        }
    }
}