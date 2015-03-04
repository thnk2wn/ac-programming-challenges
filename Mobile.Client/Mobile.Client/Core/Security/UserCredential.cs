using System;

namespace AvantCredit.Uploader.Core.Security
{
    class UserCredential
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public DateTime ExpiresDate { get; set; }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(EmailAddress) 
                    && !string.IsNullOrWhiteSpace(Password)
                    && DateTime.Now < ExpiresDate;
            }
        }
    }
}