using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using PatentSpoiler.App.Data.Queries.Account;
using PatentSpoiler.App.DTOs;
using Raven.Client;

namespace PatentSpoiler.App.Commands.User
{
    public interface IConfirmForgottenPasswordCommand
    {
        Task<DomainResult> Execute(PasswordResetViewModel model);
    }

    public class ConfirmForgottenPasswordCommand : IConfirmForgottenPasswordCommand
    {
        private readonly IGetUserByTokenQuery _getUserByTokenQuery;
        private readonly IAsyncDocumentSession _session;
        private readonly IPasswordHasher _passwordHasher;


        public ConfirmForgottenPasswordCommand(IGetUserByTokenQuery getUserByTokenQuery, IAsyncDocumentSession session, IPasswordHasher passwordHasher)
        {
            _getUserByTokenQuery = getUserByTokenQuery;
            _session = session;
            _passwordHasher = passwordHasher;
        }

        public async Task<DomainResult> Execute(PasswordResetViewModel model)
        {
            var result = new DomainResult();

            var user = await _getUserByTokenQuery.Get(model.Token);
            
            if (user == null)
            {
                result.AddGeneralError("No user is associated with this token");
                return result;
            }

            if (user.PasswordResetTokenExpiry < DateTime.Now)
            {
                result.AddGeneralError("The token has expired");
                return result;
            }

            var passwordhash = _passwordHasher.HashPassword(model.Password);
            user.UpdatePassword(passwordhash);

            await _session.StoreAsync(user);
            await _session.SaveChangesAsync();

            return result;
        }
    }
}