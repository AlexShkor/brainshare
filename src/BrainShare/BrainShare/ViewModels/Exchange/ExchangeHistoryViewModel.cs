using System.Collections.Generic;
using BrainShare.Domain.Documents;

namespace BrainShare.ViewModels.Exchange
{
    public class ExchangeHistoryViewModel
    {
        private readonly string _myId;

        public ExchangeHistoryViewModel(ExchangeHistory exchangeHistory, string myId)
        {
            _myId = myId;
            Date = exchangeHistory.Date.ToShortDateString();
            Entries = new List<ExchangeEntryViewModel>();

            foreach (var entry in exchangeHistory.Entries)
            {
                Entries.Add(new ExchangeEntryViewModel(entry));
            }
        }

        public string Date { get; set; }
        public List<ExchangeEntryViewModel> Entries { get; set; }

        public ExchangeEntryViewModel Me { get { return Entries.Find(x => x.UserId == _myId); } }

        public ExchangeEntryViewModel NotMe { get { return Entries.Find(x => x.UserId != _myId); } }
    }
}