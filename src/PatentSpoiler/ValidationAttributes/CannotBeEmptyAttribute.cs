using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace PatentSpoiler.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class CannotBeEmptyAttribute : ValidationAttribute
    {
        private const string defaultError = "'{0}' must have at least one element.";
        
        public CannotBeEmptyAttribute(): base(defaultError){}

        public override bool IsValid(object value)
        {
            var items = value as IEnumerable;
            return (items != null && items.GetEnumerator().MoveNext());
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(ErrorMessageString, name);
        }
    }
}