namespace AvantCredit.Uploader.Core.Security
{
    interface IUserRepository
    {
        User GetByEmailAddress(string emailAddress);
    }
}