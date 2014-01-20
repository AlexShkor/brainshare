using BrainShare.Utils.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrainShare.Tests
{
    [TestClass]
    public class UrlUtilityTest
    {
        [TestMethod]
        public void ResizeFbAvatar()
        {
            const string input = "https://graph.facebook.com/100000412732971/picture?width=250&height=250";
            var result = UrlUtility.ResizeAvatar(input, 64);
            Assert.AreEqual("https://graph.facebook.com/100000412732971/picture?width=64&height=64", result);
        }

        [TestMethod]
        public void ResizeCloudinaryAvatar()
        {
            const string input = "http://res.cloudinary.com/hh7rcw6t1/image/upload/c_limit,h_500,w_500/c_crop,h_200,w_200,x_15,y_15/logyild4jby7s8bgksjj.jpg";
            var result = UrlUtility.ResizeAvatar(input, 100);
            Assert.AreEqual("http://res.cloudinary.com/hh7rcw6t1/image/upload/c_limit,h_500,w_500/c_crop,h_200,w_200,x_15,y_15/w_0.5/logyild4jby7s8bgksjj.jpg", result);
        }
    }
}