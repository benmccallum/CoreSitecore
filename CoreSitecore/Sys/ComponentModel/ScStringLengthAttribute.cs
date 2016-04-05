using CoreSitecore.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoreSitecore.Sys.ComponentModel
{
    /// <summary>
    /// A StringLength attribute that gets the error message to use from the Sitecore Dictionary (localised to current language).
    /// </summary>
    public class ScStringLengthAttribute : StringLengthAttribute, IClientValidatable
    {
        private readonly string _dictionaryKey;

        /// <summary>
        /// Initializes a StringLength attribute that gets its error message from the Sitecore Dictionary using <paramref name="dictionaryKey"/>.
        /// </summary>
        /// <param name="maximumLength">The maximum allowable length of string data.</param>
        /// <param name="dictionaryKey">Key to lookup in Sitecore Dictionary.</param>
        public ScStringLengthAttribute(int maximumLength, string dictionaryKey)
            : base(maximumLength)
        {
            _dictionaryKey = dictionaryKey;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the error message for this property in the current language.
            // Do it every IsValid call to avoid ValidationAttribute caching and get fresh from CMS if changed.
            ErrorMessage = ValidationHelper.GetErrorMessage(_dictionaryKey);

            return base.IsValid(value, validationContext);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new StringLengthAttributeAdapter(metadata, context, this).GetClientValidationRules();
        }
    }
}
