namespace AvantCredit.Uploader.Core.Security
{
    interface IUserAuthService
    {
        LoginResult Authenticate(string customerId, string password, bool rememberMe);
        LoginResult Authenticate();
        User CurrentUser { get;  }
        void Logout();
    }
}