using System;
using EnsureThat;

namespace AvantCredit.Uploader.Core.Security
{
    class UserAuthService : IUserAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoginStoreService _loginStoreService;

        public UserAuthService(ILoginStoreService loginStoreService, IUserRepository userRepository)
        {
            _userRepository = Ensure.That( userRepository, "userRepository").IsNotNull().Value;
            _loginStoreService = Ensure.That(loginStoreService, "loginStoreService").IsNotNull().Value;
        }

        public LoginResult Authenticate(string emailAddress, string password, bool rememberMe)
        {
            Ensure.That(emailAddress, "customerId").IsNotNullOrWhiteSpace();
            Ensure.That(password, "password").IsNotNullOrWhiteSpace();

            var user = _userRepository.GetByEmailAddress(emailAddress);

            if (user == null || !PasswordUtil.IsMatch(password, user.Password, user.Salt))
            {
                _loginStoreService.ClearPassword();
                CurrentUser = null;
                return new LoginResult(LoginStatus.InvalidCredentials, message: "Invalid username/password");
            }

            if (rememberMe)
            {
                var credential = new UserCredential
                {
                    EmailAddress = user.EmailAddress, Password = password,
                    ExpiresDate = DateTime.Now.AddMinutes(30)
                };
                _loginStoreService.Store(credential);
            }

            CurrentUser = user;

            return new LoginResult(LoginStatus.Success, user);
        }

        public User CurrentUser { get; private set; }

        /// <summary>
        /// Authenticate using saved credentials
        /// </summary>
        /// <returns></returns>
        public LoginResult Authenticate()
        {
            var credential = _loginStoreService.Get();

            if (null == credential)
                return new LoginResult(LoginStatus.NoCredentials);

            var result = Authenticate(credential.EmailAddress, credential.Password, rememberMe: true);
            return result;
        }

        public void Logout()
        {
            _loginStoreService.ClearCredentials();
            CurrentUser = null;
        }
    }
}