using System.Collections.Generic;
using System.Linq;
using Brainshare.Infrastructure.Documents;
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