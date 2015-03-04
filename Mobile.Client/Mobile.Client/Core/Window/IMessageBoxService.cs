using Android.Content;

namespace AvantCredit.Uploader.Core.Window
{
    interface IMessageBoxService
    {
        void ShowOk(string text);
        void ShowOk(string text, Context context);
        void ShowAlert(string text, Context context);
        void ShowAlert(string text);
    }
}