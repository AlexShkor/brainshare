using System.Collections.Generic;
using Antlr.Runtime.Misc;
using BrainShare.Domain.Documents;
using BrainShare.Domain.Enums;
using BrainShare.ViewModels.Exchange;

namespace BrainShare.ViewModels
{
    public class ExchangeHistoryViewModel
    {
        public ExchangeHistoryViewModel(ExchangeHistory exchangeHistory)
        {
            Date = exchangeHistory.Date.ToShortDateString();
            Entries = new List<ExchangeEntryViewModel>();
            Arrows = new ListStack<ExchangeArrowViewModel>();

            for (int i = 0; i < exchangeHistory.Entries.Count; i++)
            {
                var entry = exchangeHistory.Entries[i];
                Entries.Add(new ExchangeEntryViewModel(entry));

                if (entry.ExchangeEntryType != ExchangeEntryType.GiftReceiver)
                {
                    Arrows.Add(new ExchangeArrowViewModel { ArrowUrl = Utils.Utilities.UrlUtility.GetExchangeArrowUrl(i, (int)entry.ExchangeEntryType) });
                }
            }
        }

        public string Date { get; set; }
        public List<ExchangeArrowViewModel> Arrows { get; set; }
        public List<ExchangeEntryViewModel> Entries { get; set; }
    }
}