using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace MvcMusicStore.MongoDB
{
    public class BaseFilter
    {

        public BaseFilter()
        {
            ExcludeFields = new List<string>();
            Ordering = new List<FilterOrder>();
        }

        public List<FilterOrder> Ordering { get; set; }

        public List<string> ExcludeFields { get; set; }

        public void AddOrder(string key, bool desc = false)
        {
            Ordering.Add(new FilterOrder() { Key = key, Desc = desc });
        }

        public void AddOrder<T, TResult>(Expression<Func<T, TResult>> sortKeySelector, bool desc = false)
        {
            var s = sortKeySelector.Body.ToString();
            var key = s.Substring(sortKeySelector.Parameters[0].Name.Length + 1);
            AddOrder(key, desc);
        }
    }

    public class FilterOrder
    {
        public string Key { get; set; }
        public bool Desc { get; set; }
    }
}