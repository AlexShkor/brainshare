using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NUnit.Framework;

namespace BrainShare.Services.Validation
{
    public class LanguagesService
    {
        public IEnumerable<LanguageInfo> GetAllLanguages()
        {
            return CultureInfo.GetCultures(CultureTypes.AllCultures).Where(x =>
                !Equals(x, CultureInfo.InvariantCulture) && 
                Equals(x.Parent, CultureInfo.InvariantCulture)).Select(x => new LanguageInfo(x));
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

    public class LanguageServiceTests
    {
        [Test]
        public void test()
        {
            var service = new LanguagesService();
            var all = service.GetAllLanguages().ToList();
            Assert.NotNull(all);
        }
    }
}