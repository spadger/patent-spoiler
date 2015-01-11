using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using PatentSpoiler.App.Domain.Security;
using PatentSpoiler.App.DTOs;
using PatentSpoiler.App.Security;
using Raven.Client;

namespace PatentSpoiler.App.Commands.User
{
    public interface IRegisterNewUserCommand
    {
        Task<DomainResult> RegisterAsync(RegistrationViewModel user);
        PatentSpoilerUser User { get; }
    }

    public class RegisterNewUserCommand : IRegisterNewUserCommand
    {
        private readonly IAsyncDocumentSession session;
        private readonly PatentSpoilerUserManager userManager;

        private PatentSpoilerUser user;

        public RegisterNewUserCommand(IAsyncDocumentSession session, PatentSpoilerUserManager userManager)
        {
            this.session = session;
            this.userManager = userManager;
        }

        public async Task<DomainResult> RegisterAsync(RegistrationViewModel request)
        {
            var domainResult = new DomainResult();

            var validations = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, new ValidationContext(request), validations, true))
            {
                domainResult.AddError(validations);
                return domainResult;
            }

            var existing = await session.LoadAsync<PatentSpoilerUser>(request.Username);
            
            if (existing != null)
            {
                domainResult.AddError("username", "This username has been taken");
                return domainResult;
            }

            user = request.ToUser();
            var identityResult = await userManager.CreateAsync(user);
            
            foreach (var error in identityResult.Errors)
            {
                domainResult.AddGeneralError(error);
            }

            return domainResult;
        }

        public PatentSpoilerUser User
        {
            get { return user; }
        }
    }
}
