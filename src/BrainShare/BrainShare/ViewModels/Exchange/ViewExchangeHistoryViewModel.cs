using System.Collections.Generic;
using BrainShare.Domain.Documents;
using System.Linq;
using BrainShare.ViewModels.Exchange;

namespace BrainShare.ViewModels
{
    public class ViewExchangeHistoryViewModel
    {
        public ViewExchangeHistoryViewModel(IEnumerable<ExchangeHistory> items, string myId)
        {
            Items = items.Select(e => new ExchangeHistoryViewModel(e, myId)).ToList();
        }

        public List<ExchangeHistoryViewModel> Items { get; set; } 
    }
}