using Sitecore.Globalization;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CoreSitecore.Helpers
{
    /// <summary>
    /// Helper for Sitecore validation.
    /// </summary>
    internal static class ValidationHelper
    {
        /// <summary>
        /// Gets a localised error message from the Sitecore CMS dictionary given a key.
        /// </summary>
        /// <param name="dictionaryKey">Key in the Sitecore dictionary.</param>
        /// <returns>The text in that dictionary entry (localised).</returns>
        public static string GetErrorMessage(string dictionaryKey)
        {
            return Translate.Text(dictionaryKey);
        }

        /// <summary>
        /// Gets the client validation rules, where the validation message is a localised error message from the Sitecore CMS dictionary given a key.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="validationType"></param>
        /// <param name="dictionaryKey">Key in the Sitecore dictionary.</param>
        /// <returns></returns>
        public static List<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, string validationType, string dictionaryKey)
        {
            var modelClientValidationRule = new ModelClientValidationRule
            {
                ValidationType = validationType,
                ErrorMessage = GetErrorMessage(dictionaryKey)
            };

            modelClientValidationRule.ValidationParameters.Add("param", metadata.PropertyName);
            var rules = new List<ModelClientValidationRule> { modelClientValidationRule };
            return rules;
        }
    }
}
