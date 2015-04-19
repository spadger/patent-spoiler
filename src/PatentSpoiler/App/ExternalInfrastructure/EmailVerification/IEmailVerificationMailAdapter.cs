namespace PatentSpoiler.App.ExternalInfrastructure.EmailVerification
{
    public interface IEmailVerificationMailAdapter
    {
        void Send(string email, string name, string link);
    }
}