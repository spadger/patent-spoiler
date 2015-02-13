using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace PatentSpoiler.App.Commands
{
    public class DomainResult
    {
        private readonly Dictionary<string, IEnumerable<string>> errors = new Dictionary<string, IEnumerable<string>>();

        public void AddGeneralError(string message)
        {
            AddError("General", message);
        }

        public void AddError(string key, string message)
        {
            IEnumerable<string> errorsForKey;

            if (!errors.TryGetValue(key, out errorsForKey))
            {
                errors[key] = errorsForKey = new List<string>();
            }
                
            ((List<string>)errorsForKey).Add(message);
        }

        public void AddError(IEnumerable<ValidationResult> validationResults)
        {
            foreach (var validation in validationResults)
                foreach (var memberName in validation.MemberNames)
                {
                    AddError(memberName, validation.ErrorMessage);
                }
        }

        public void AddErrors(ModelStateDictionary model)
        {
            foreach (var kvp in model)
            foreach (var message in kvp.Value.Errors)
            {
                AddError(kvp.Key, message.ErrorMessage);
            }
        }

        public IReadOnlyDictionary<string, IEnumerable<string>> Errors
        {
            get
            {
                return new ReadOnlyDictionary<string, IEnumerable<string>>(errors);
            } 
        }

        public bool HasErrors
        {
            get { return errors.Any(); }
        }

        public bool Success
        {
            get { return !errors.Any(); }
        }

        public static DomainResult Valid()
        {
            return new DomainResult();
        }

        public static DomainResult From(ModelStateDictionary modelState)
        {
            var result = new DomainResult();

            result.AddErrors(modelState);

            return result;
        }
    }
}
