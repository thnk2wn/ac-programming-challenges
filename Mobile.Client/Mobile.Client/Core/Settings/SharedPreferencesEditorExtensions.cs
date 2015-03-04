using System;
using Android.Content;

namespace AvantCredit.Uploader.Core.Settings
{
    public static class SharedPreferencesEditorExtensions
    {
        public static void SaveObject<T>(this ISharedPreferencesEditor ed, string key, object value)
        {
            var refType = typeof (T);

            if (refType == typeof(string))
            {
                ed.PutString(key, (String)value);
                return;
            }
            if (refType == typeof(bool))
            {
                ed.PutBoolean(key, (bool)value);
                return;
            }
            if (refType == typeof(int))
            {
                ed.PutInt(key, (int)value);
                return;
            }
            if (refType == typeof(float))
            {
                ed.PutFloat(key, (float)value);
                return;
            }
            if (refType == typeof(long))
            {
                ed.PutLong(key, (long)value);
                return;
            }
            throw new InvalidOperationException("Type not supported, only use String, Bool, Int, Float, Long");
        }
    }
}