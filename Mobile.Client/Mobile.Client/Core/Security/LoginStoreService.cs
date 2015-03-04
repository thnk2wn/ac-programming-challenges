using System;
using System.Globalization;
using AvantCredit.Uploader.Core.Settings;

namespace AvantCredit.Uploader.Core.Security
{
    class LoginStoreService : ILoginStoreService
    {
        private const string PREF_FILENAME = "UserDetails";

        public void Store(UserCredential credential)
        {
            EmailAddress.SetSetting(credential.EmailAddress);
            Password.SetSetting(credential.Password);
            ExpiresDate.SetSetting(credential.ExpiresDate.ToString("O"));
        }

        public UserCredential Get()
        {
            var credential = new UserCredential
                {
                    EmailAddress = EmailAddress.GetSetting(),
                    Password = Password.GetSetting()
                };
            var expiresText = ExpiresDate.GetSetting();
            credential.ExpiresDate = null == ExpiresDate.GetSetting() 
                ? DateTime.MinValue
                : DateTime.Parse(expiresText, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            return credential.IsValid ? credential : null;
        }

        public void ClearPassword()
        {
            Password.SetSetting(null);
        }

        public void ClearCredentials()
        {
            Password.SetSetting(null);
            EmailAddress.SetSetting(null);
        }

        private static SettingsKey<string> EmailAddress
        {
            get
            {
                return new SettingsKey<string>("User_EmailAddress", PREF_FILENAME, null);
            }
        }

        //TODO: remember me security considerations such as encrypted / secure password
        private static SettingsKey<string> Password
        {
            get
            {
                return new SettingsKey<string>("User_Password", PREF_FILENAME, null);
            }
        }

        private static SettingsKey<string> ExpiresDate 
        {
            get
            {
                return new SettingsKey<string>("User_RememberMeExpires", PREF_FILENAME, null);
            }
        } 
    }
}