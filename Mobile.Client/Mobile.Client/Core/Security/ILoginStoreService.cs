

namespace AvantCredit.Uploader.Core.Security
{
    interface ILoginStoreService
    {
        void Store(UserCredential user);
        UserCredential Get();
        void ClearPassword();
        void ClearCredentials();
    }
}