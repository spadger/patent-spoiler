namespace PatentSpoiler.App.ExternalInfrastructure.PasswordReset
{
    public interface IPasswordResetMailAdapter
    {
        void Send(string email, string name, string link);
    }
}