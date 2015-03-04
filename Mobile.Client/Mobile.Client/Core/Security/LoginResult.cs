namespace AvantCredit.Uploader.Core.Security
{
    enum LoginStatus
    {
        InvalidCredentials,
        Success,
        NoCredentials
    }

    class LoginResult
    {
        public LoginResult(LoginStatus status, User user = null, string message = null)
        {
            this.Status = status;
            this.User = user;
            this.Message = message;
        }

        public LoginStatus Status { get; private set; }

        public User User { get; private set; }

        public string Message { get; private set; }

        public bool Success { get { return this.Status == LoginStatus.Success; } }
    }
}