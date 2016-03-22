using Sitecore.Globalization;
using System.ComponentModel;

namespace CoreSitecore.Sys.ComponentModel
{
    /// <summary>
    /// A DisplayName attribute that gets the text to use from the Sitecore Dictionary (localised to current language).
    ///
    /// A better solution though is to use
    /// </summary>
    public class ScDisplayNameAttribute : DisplayNameAttribute
    {
        private readonly string _dictionaryKey;

        /// <summary>
        /// Initializes a DisplayName field attribute that gets its text from the Sitecore Dictionary using <paramref name="dictionaryKey"/>.
        /// </summary>
        /// <param name="dictionaryKey">Key to lookup in Sitecore Dictionary.</param>
        public ScDisplayNameAttribute(string dictionaryKey)
        {
            _dictionaryKey = dictionaryKey;
        }

        public override string DisplayName
        {
            get
            {
                return Translate.Text(_dictionaryKey);
            }
        }
    }
}
