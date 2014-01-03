using System.Collections.Generic;
using BrainShare.Domain.Documents;
using System.Linq;

namespace BrainShare.ViewModels
{
    public class ViewExchangeHistoryViewModel
    {
        public ViewExchangeHistoryViewModel(IEnumerable<ExchangeHistory> items )
        {
            Items = items.Select(e => new ExchangeHistoryViewModel(e)).ToList();
        }

        public List<ExchangeHistoryViewModel> Items { get; set; } 
    }
}