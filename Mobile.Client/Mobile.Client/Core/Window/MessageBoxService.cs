using Android.App;
using Android.Content;

namespace AvantCredit.Uploader.Core.Window
{
    class MessageBoxService : IMessageBoxService
    {
        public void ShowOk(string text)
        {
            ShowOk(text, Application.Context);
        }

        public void ShowOk(string text, Context context)
        {
            var builder = new AlertDialog.Builder(context);
            builder.SetMessage(text);
            builder.SetPositiveButton("OK", (s, e) => {});
            builder.Create().Show();
        }

        public void ShowAlert(string text)
        {
            ShowAlert(text, Application.Context);
        }

        public void ShowAlert(string text, Context context)
        {
            var builder = new AlertDialog.Builder(context);
            builder.SetMessage(text);
            builder.SetIcon(Android.Resource.Drawable.IcDialogAlert);
            builder.SetPositiveButton("OK", (s, e) => {});
            builder.Create().Show();
        }
    }
}