using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Brainshare.Infrastructure.Services.Validation
{
    public class LanguagesService
    {
        private readonly List<string> _order = new List<string>{"ru", "en", "de"};

        public IEnumerable<LanguageInfo> GetAllLanguages()
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures).Where(x =>
                !Equals(x, CultureInfo.InvariantCulture) && 
                Equals(x.Parent, CultureInfo.InvariantCulture)).OrderBy(GetOrder).Select(x => new LanguageInfo(x));
        }

        private int GetOrder(CultureInfo info)
        {
            var index = _order.IndexOf(info.TwoLetterISOLanguageName);
            if (index == -1)
            {
                return 999;
            }
            return index;
        }
    }

    public class LanguageInfo
    {
        public LanguageInfo(CultureInfo cultureInfo)
        {
            Name = cultureInfo.EnglishName;
            NativeName = cultureInfo.NativeName;
            Symbol = cultureInfo.TwoLetterISOLanguageName;
        }

        public string Name { get; set; }
        public string NativeName { get; set; }
        public string Symbol { get; set; }
    }
}