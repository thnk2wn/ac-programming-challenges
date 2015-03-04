using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AvantCredit.Uploader.Activities.Main;
using AvantCredit.Uploader.Core.Activity;
using AvantCredit.Uploader.Core.DI;
using AvantCredit.Uploader.Core.Security;
using AvantCredit.Uploader.Core.Window;

namespace AvantCredit.Uploader.Activities
{
    [Activity(Label = "AvantCredit Uploader", Icon = "@drawable/avant_icon_logo")]
    public class LoginActivity : Activity
    {
        private readonly IUserAuthService _userAuthService;
        private readonly IMessageBoxService _messageBoxService;

        public LoginActivity()
        {
            _userAuthService = IoC.Get<IUserAuthService>();
            _messageBoxService = IoC.Get<IMessageBoxService>();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Login);

            FindViewById<ImageButton>(Resource.Id.signInButton).Click += (sender, args) => SignIn();
            FindViewById<ImageButton>(Resource.Id.forgotPasswordButton).Click += (sender, args) => ForgotPassword();
        }

        private void ForgotPassword()
        {
            var sb = new StringBuilder();
            sb.AppendLine("For the purposes of this demo use either:");
            sb.AppendLine();
            sb.AppendLine("demo@domain.com / demo");
            sb.AppendLine();
            sb.AppendLine("guest@domain.com / pass");
            _messageBoxService.ShowOk(sb.ToString(), this);
        }

        private void SignIn()
        {
            var rememberMeCheckbox = FindViewById<CheckBox>(Resource.Id.rememberMeCheckBox);
            var authResult = _userAuthService.Authenticate(this.EditText(Resource.Id.customerId).Text, 
                this.EditText(Resource.Id.password).Text, rememberMeCheckbox.Checked);

            if (!authResult.Success)
            {
                //TODO: consider DialogService w/interface and methods to show diff types of messages
                var builder = new AlertDialog.Builder(this);
                builder.SetMessage(authResult.Message);
                builder.SetPositiveButton("OK", (s, e) => { /* do something on OK click */ });
                builder.Create().Show();
                return;
            }

            var intent = new Intent(this, typeof(MainActivity));
            intent.PutAsJson(authResult.User);
            StartActivity(intent);
            Finish();
        }
    }
}

