using CoreSitecore.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoreSitecore.Sys.ComponentModel
{
    /// <summary>
    /// A EmailAddress attribute that gets the error message to use from the Sitecore Dictionary (localised to current language).
    /// </summary>
    public class ScEmailAddressAttribute : DataTypeAttribute, IClientValidatable
    {
        private readonly string _dictionaryKey;

        private static EmailAddressAttribute _emailAddressAttribute;
        private static EmailAddressAttribute EmailAddressAttribute
        {
            get { return _emailAddressAttribute ?? (_emailAddressAttribute = new EmailAddressAttribute()); }
        }

        /// <summary>
        /// Initializes a EmailAddress attribute that gets its error message from the Sitecore Dictionary using <paramref name="dictionaryKey"/>.
        /// </summary>
        /// <param name="dictionaryKey">Key to lookup in Sitecore Dictionary.</param>
        public ScEmailAddressAttribute(string dictionaryKey)
            : base(DataType.EmailAddress)
        {
            _dictionaryKey = dictionaryKey;
        }

        public override bool IsValid(object value)
        {
            // Get the error message for this property in the current language.
            // Do it every IsValid call to avoid ValidationAttribute caching and get fresh from CMS if changed.
            ErrorMessage = ValidationHelper.GetErrorMessage(_dictionaryKey);

            return EmailAddressAttribute.IsValid(value);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return ValidationHelper.GetClientValidationRules(metadata, "emailaddress", _dictionaryKey);
        }
    }
}
