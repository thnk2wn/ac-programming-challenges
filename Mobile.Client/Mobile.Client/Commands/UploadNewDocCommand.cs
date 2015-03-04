using Android.Content;
using AvantCredit.Uploader.Activities;
using AvantCredit.Uploader.Core.Command;
using AvantCredit.Uploader.Core.Security;

namespace AvantCredit.Uploader.Commands
{
    class UploadNewDocCommand : Command
    {

        public UploadNewDocCommand(Context context) 
            : base(context)
        {
        }

        protected override void OnExecution(ExecuteCommandEventArgs e)
        {
            base.OnExecution(e);

            var intent = new Intent(Context, typeof (UploadNewDocActivity));
            Context.StartActivity(intent);
        }
    }
}