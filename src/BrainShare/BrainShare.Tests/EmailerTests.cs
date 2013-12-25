using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrainShare.EmailMessaging;
using BrainShare.EmailMessaging.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrainShare.Tests
{
    [TestClass]
    public class EmailerTests
    {
        private string _anyText = "any_text";

        private string _welcomViewName = "Welcome";
        private string _userMessageViewName = "UserMessage";
        private string _giftExchangeViewName = "GiftExchange";
        private string _requestViewName = "Request";
        private string _exchangeConfirmViewName = "ExchangeConfirm";
        private string _userHaveSearchedBookViewName = "UserHaveSearechedBook";

        private Emailer _emailer = new Emailer("","");

        [TestMethod]
        public void SendWelcomeMessage_HtmlTemplateExist()
        {
            var result = _emailer.GetStringFromRazor(_welcomViewName, GetWelcomModel());
            Assert.IsNotNull(result);        
        }

        //public void SendGiftExchangeMessage_HtmlTemplateExist(GiftExchange giftExchangeModel, string email, string displayName)
        //{
        //    var body = GetStringFromRazor("GiftExchange", giftExchangeModel);
        //    Send(email, displayName, "BrainShare : Вам подарили книгу!!", body);
        //}

        //public void EmailUserMessage_HtmlTemplateExist(UserMessage userMessageModel, string email, string displayName)
        //{
        //    var body = GetStringFromRazor("UserMessage", userMessageModel);
        //    Send(email, displayName, "BrainShare : Новое сообщение", body);
        //}

        //public void EmailUserHaveSearechedBook_HtmlTemplateExist(UserHaveSearechedBook userHaveSearechedBookModel, string email, string displayName)
        //{
        //    var body = GetStringFromRazor("UserHaveSearechedBook", userHaveSearechedBookModel);
        //    Send(email, displayName, "BrainShare : Интересующая", body);
        //}

        //public void SendRequestMessage_HtmlTemplateExist(Request requestModel, string email, string displayName)
        //{
        //    var body = GetStringFromRazor("Request", requestModel);
        //    Send(email, displayName, "BrainShare : Уведомление об отправке запроса", body);
        //}

        //public void SendExchangeConfirmMessage_HtmlTemplateExist(ExchangeConfirm exchangeConfirmModel, string email, string displayName)
        //{
        //    var body = GetStringFromRazor("ExchangeConfirm", exchangeConfirmModel);
        //    Send(email, displayName, "BrainShare : Уведомление об обмене", body);
        //}

        private Welcome GetWelcomModel()
        {
            return new Welcome { ReceiverName = _anyText };
        }
    }
}
