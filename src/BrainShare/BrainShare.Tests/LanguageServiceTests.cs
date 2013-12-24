using System;
using System.Linq;
using Brainshare.Infrastructure.Services.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrainShare.Tests
{
    [TestClass]
    public class LanguageServiceTests
    {
        [TestMethod]
        public void GetAllLanguages_NotNull()
        {
            var service = new LanguagesService();
            var all = service.GetAllLanguages().ToList();
            Assert.IsNotNull(all);
        }
    }
}
