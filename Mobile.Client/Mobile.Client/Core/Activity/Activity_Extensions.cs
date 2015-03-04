using Android.Widget;

namespace AvantCredit.Uploader.Core.Activity
{
    public static class ActivityExtensions
    {
        public static TextView TextView(this Android.App.Activity activity, int id)
        {
            return activity.FindViewById<TextView>(id);
        }

        public static EditText EditText(this Android.App.Activity activity, int id)
        {
            return activity.FindViewById<EditText>(id);
        }
    }
}