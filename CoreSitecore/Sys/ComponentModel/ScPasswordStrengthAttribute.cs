using CoreSitecore.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CoreSitecore.Sys.ComponentModel
{
    /// <summary>
    /// A validation attribute that checks the value of the attributed property meets the membershipProvider password strength requirements.
    /// The error message used is from the Sitecore Dictionary (localised to current language).
    /// </summary>
    public class ScMembershipPasswordStrengthAttribute : CoreWeb.Sys.ComponentModel.MembershipPasswordStrengthAttribute
    {
        private readonly string _dictionaryKey;

        /// <summary>
        /// Initializes a instance that gets its error message from the Sitecore Dictionary using <paramref name="dictionaryKey"/>.
        /// </summary>
        /// <param name="otherProperty">The property to compare with the current property.</param>
        /// <param name="dictionaryKey">Key to lookup in Sitecore Dictionary.</param>
        public ScMembershipPasswordStrengthAttribute(string dictionaryKey)
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

        // TODO: Create a version of this in CoreMvc that implement IClientValidatable properly. Then update this guy to inherit from that.
        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //{
        //    return ValidationHelper.GetClientValidationRules(metadata, "compare", _dictionaryKey);
        //}
    }
}
