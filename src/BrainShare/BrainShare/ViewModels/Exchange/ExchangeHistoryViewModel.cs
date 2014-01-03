using System.Collections.Generic;
using BrainShare.Domain.Documents;

namespace BrainShare.ViewModels
{
    public class ExchangeHistoryViewModel
    {
        public ExchangeHistoryViewModel(ExchangeHistory exchangeHistory)
        {
            Date = exchangeHistory.Date.ToShortDateString();
            Entries = new List<ExchangeEntryViewModel>();

            foreach (var entry in exchangeHistory.Entries)
            {
                Entries.Add(new ExchangeEntryViewModel(entry));
            }
        }

        public string Date { get; set; }
        public List<ExchangeEntryViewModel> Entries { get; set; }
    }
}