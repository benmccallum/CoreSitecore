using System.Collections.Generic;
using System.Web.Mvc;

namespace CoreSitecore.Sys.ComponentModel
{
    public class ScStringLengthAdapter : DataAnnotationsModelValidator<ScStringLengthAttribute>
    {
        public ScStringLengthAdapter(ModelMetadata metadata, ControllerContext context, ScStringLengthAttribute attribute)
            : base(metadata, context, attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            return new[] { new ModelClientValidationStringLengthRule(ErrorMessage, Attribute.MinimumLength, Attribute.MaximumLength) };
        }
    }
}
