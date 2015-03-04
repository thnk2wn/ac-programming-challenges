using System.Linq;
using Android.Content;

namespace AvantCredit.Uploader.Core.Settings
{
    /// <summary>
    /// Key Value Pair
    /// </summary>
    /// <typeparam name="T">Value Type</typeparam>
    public class SettingsKey<T>
    {
        public string Key { get; private set; }
        private T _value;
        private readonly string _preferenceName;
        private readonly T _defaultValue;

        /// <summary>
        /// Abstraction around shared preferences
        /// </summary>
        /// <param name="key">Name the key</param>
        /// <param name="preferenceName">Name the preference (should be consistant accross all settings)</param>
        /// <param name="defaultValue">Give a default value</param>
        public SettingsKey(string key, string preferenceName, T defaultValue)
        {
            Key = key;
            _preferenceName = preferenceName;
            _defaultValue = defaultValue;

        }

        /// <summary>
        /// Gets the setting
        /// </summary>
        /// <param name="context">context</param>
        /// <returns></returns>
        public T GetSetting(Context context = null)
        {
            if (null == context) context = Android.App.Application.Context;
            var shared = context.GetSharedPreferences(_preferenceName, FileCreationMode.WorldReadable);
            _value = (T)shared.All.FirstOrDefault(x => x.Key == Key).Value;
            if (_value == null) SetSetting(_defaultValue, context);

            return _value;
        }

        /// <summary>
        /// Set the setting with a new setting
        /// </summary>
        /// <param name="val"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public SettingsKey<T> SetSetting(T val, Context context = null)
        {
            if (null == context) context = Android.App.Application.Context;
            var shared = context.GetSharedPreferences(_preferenceName, FileCreationMode.WorldWriteable);
            var edit = shared.Edit();
            edit.SaveObject<T>(Key, val);
            edit.Commit();
            _value = val;
            return this;
        }
    }
}