using Sitecore.Data;
using Sitecore.Data.Validators;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace CoreSitecore.Data.Validators.FieldValidators
{
    /// <summary>
    /// Validates that a link field value meets template requirements
    /// specified using the following parameters:
    /// - ExcludeTemplatesForSelection: If present, the item being
    ///   based on an excluded template will cause validation to fail.
    /// - IncludeTemplatesForSelection: If present, the item not being
    ///   based on an included template will cause validation to fail
    /// 
    /// ExcludeTemplatesForSelection trumps IncludeTemplatesForSelection
    /// if the same value appears in both lists. 
    /// 
    /// Lists are comma seperated.
    /// 
    /// http://geekswithblogs.net/KyleBurns/archive/2012/08/27/validating-a-linked-itemrsquos-data-template-in-sitecore.aspx
    /// </summary>
    [Serializable]
    public class LinkItemTemplateValidator : StandardValidator
    {
        public LinkItemTemplateValidator()
        {
        }

        /// <summary>
        /// Serialization constructor is required by the runtime
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public LinkItemTemplateValidator(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Returns whether the linked item meets the template
        /// constraints specified in the parameters
        /// </summary>
        /// <returns>
        /// The result of the evaluation.
        /// </returns>
        protected override ValidatorResult Evaluate()
        {
            if (string.IsNullOrWhiteSpace(ControlValidationValue))
            {
                return ValidatorResult.Valid; // let "required" validation handle
            }

            var excludeString = Parameters["ExcludeTemplatesForSelection"];
            var includeString = Parameters["IncludeTemplatesForSelection"];
            if (string.IsNullOrWhiteSpace(excludeString) && string.IsNullOrWhiteSpace(includeString))
            {
                return ValidatorResult.Valid; // "allow anything" if no params
            }

            Guid linkedItemGuid;
            if (!Guid.TryParse(ControlValidationValue, out linkedItemGuid))
            {
                return ValidatorResult.Valid; // probably put validator on wrong field
            }

            var item = GetItem();
            var linkedItem = item.Database.GetItem(new ID(linkedItemGuid));

            if (linkedItem == null)
            {
                return ValidatorResult.Valid; // this validator isn't for broken links
            }

            var exclusionList = (excludeString ?? string.Empty).Split(',');
            var inclusionList = (includeString ?? string.Empty).Split(',');
            var linkedItemTemplateID = linkedItem.TemplateID.ToString();
            if (
                // There is no inclusion list - accept everything, or there is so the linked item's template must be in it by name or id.
                (inclusionList.Length == 0 || inclusionList.Contains(linkedItem.TemplateName) || inclusionList.Contains(linkedItemTemplateID))
                &&
                // The exclusion list doesn't contain the linked item's template by name or id
                (!exclusionList.Contains(linkedItem.TemplateName) && !exclusionList.Contains(linkedItemTemplateID))
               )
            {
                return ValidatorResult.Valid;
            }

            Text = GetText("The field \"{0}\" specifies an item which is based on template \"{1}\". This template is not valid for selection", GetFieldDisplayName(), linkedItem.TemplateName);

            return GetFailedResult(ValidatorResult.FatalError);
        }

        protected override ValidatorResult GetMaxValidatorResult()
        {
            return ValidatorResult.FatalError;
        }

        public override string Name
        {
            get { return @"LinkItemTemplateValidator"; }
        }
    }
}