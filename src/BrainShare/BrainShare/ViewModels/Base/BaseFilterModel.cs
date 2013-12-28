using System.Collections.Generic;
using BrainShare.Infrastructure.Mongo;
using Brainshare.Infrastructure.Extensions;

namespace BrainShare.ViewModels.Base
{
    public class BaseFilterModel<T> : BaseViewModel where T : BaseFilter, new()
    {
        public string OrderByKey { get; set; }
        public bool Desc { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ItemsPerPage { get; set; }

        public virtual IEnumerable<string> DoNotReturnPropertiesNames
        {
            get { return new[] { "DoNotReturnPropertiesNames", "TotalPages", "SubmitBtnText", "ReferrerUrl" }; }
        }

        public BaseFilterModel()
        {
            CurrentPage = 1;
            TotalPages = 1;
            ItemsPerPage = 10;
        }

        public virtual T ToFilter()
        {
            var filter = new T
            {
                PagingInfo = new PagingInfo { CurrentPage = CurrentPage, ItemsPerPage = ItemsPerPage }
            };
            if (OrderByKey.HasValue())
            {
                filter.AddOrder(OrderByKey, Desc);
            }
            return filter;
        }

        public void UpdatePagingInfo(PagingInfo pagingInfo)
        {
            CurrentPage = pagingInfo.CurrentPage;
            TotalPages = pagingInfo.TotalPagesCount;
        }
    }
}