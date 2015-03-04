using Android.Content;
using Newtonsoft.Json;

namespace AvantCredit.Uploader.Core.Activity
{
    public static class IntentExtensions
    {
        private const string DEFAULT_KEY = "IntentData";

        public static void PutAsJson(this Intent intent, object data, string key = null)
        {
            intent.PutExtra(key ?? DEFAULT_KEY, JsonConvert.SerializeObject(data));
        }

        public static T GetAsJson<T>(this Intent intent, string key = null)
            where T: class 
        {
            var json = intent.GetStringExtra(key ?? DEFAULT_KEY);

            if (string.IsNullOrWhiteSpace(json))
                return null;

            var data = JsonConvert.DeserializeObject<T>(json);
            return data;
        }
    }
}