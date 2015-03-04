using Android.App;
using Android.Content;
using Android.OS;
using AvantCredit.Uploader.Activities.Main;
using AvantCredit.Uploader.Core.Activity;
using AvantCredit.Uploader.Core.DI;
using AvantCredit.Uploader.Core.Security;

namespace AvantCredit.Uploader.Activities
{

    [Activity(MainLauncher = true, Label = "AvantCredit Uploader", Icon = "@drawable/avant_icon_logo")]
    public class StartupActivity : Activity
    {
        private readonly IUserAuthService _userAuthService;

        public StartupActivity()
        {
            IoC.Initialize();
            _userAuthService = IoC.Get<IUserAuthService>();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Startup);

            // to generate values for our fake user data:
            //var salt = PasswordUtil.GenerateSalt();
            //var pwd = PasswordUtil.Encrypt("the_password_here", salt);

            // authenticate with saved credentials, if any and direct to main view on success. otherwise login view
            var authResult = _userAuthService.Authenticate();

            if (authResult.Success)
            {
                var intent = new Intent(this, typeof(MainActivity));
                intent.PutAsJson(authResult.User);
                StartActivity(intent);
                Finish();
            }
            else
            {
                var intent = new Intent(this, typeof(LoginActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                StartActivity(intent);
                Finish();
            }
        }
    }

}