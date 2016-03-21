using Sitecore.Data;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;

public static class TemplateItemExtensions
{
    /// <summary>
    /// Gets the template's immediate fields (ie. those defined on this template and not any inherited templates).
    /// </summary>
    /// <param name="templateItem">Template Item definition.</param>
    /// <returns>Array of fields found.</returns>
    public static TemplateFieldItem[] GetImmediateFields(this TemplateItem templateItem)
    {
        return templateItem.GetSections().SelectMany(s => s.GetFields()).ToArray();
    }

    public static IEnumerable<TemplateItem> GetAllBaseTemplates(this TemplateItem templateItem)
    {
        var allBaseTemplates = new List<TemplateItem>();
        ExtractBaseTemplates(allBaseTemplates, templateItem);
        return allBaseTemplates;
    }

    private static void ExtractBaseTemplates(List<TemplateItem> allBaseTemplates, TemplateItem templateItem)
    {
        allBaseTemplates.Add(templateItem);
        foreach (var ti in templateItem.BaseTemplates)
        {
            ExtractBaseTemplates(allBaseTemplates, ti);
        }
    }

    public static bool InheritsTemplate(this TemplateItem templateItem, ID templateId)
    {
        if (templateItem.ID.Equals(templateId))
        {
            return true;
        }

        foreach (var template in templateItem.BaseTemplates)
        {
            if (template.ID.Equals(templateId))
            {
                return true;
            }

            if (template.InheritsTemplate(templateId))
            {
                return true;
            }
        }

        return false;
    }
}
