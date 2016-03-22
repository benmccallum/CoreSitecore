using CoreSitecore.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CoreSitecore.Sys.ComponentModel
{
    /// <summary>
    /// A Compare attribute that gets the error message to use from the Sitecore Dictionary (localised to current language).
    /// </summary>
    public class ScCompareAttribute : System.ComponentModel.DataAnnotations.CompareAttribute, IClientValidatable
    {
        private readonly string _dictionaryKey;

        /// <summary>
        /// Initializes a Compare field attribute that gets its error message from the Sitecore Dictionary using <paramref name="dictionaryKey"/>.
        /// </summary>
        /// <param name="otherProperty">The property to compare with the current property.</param>
        /// <param name="dictionaryKey">Key to lookup in Sitecore Dictionary.</param>
        public ScCompareAttribute(string otherProperty, string dictionaryKey)
            : base(otherProperty)
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
            return ValidationHelper.GetClientValidationRules(metadata, "compare", _dictionaryKey);
        }
    }
}
