using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Brainshare.Infrastructure.Platform.Extensions;

namespace Brainshare.Infrastructure.Platform.Utilities
{
    public static class EnumUtil
    {
        public static List<SelectListItem> ToSelectList<T>(T? selectedValue = null) where T : struct, IComparable, IConvertible, IFormattable
        {
            return Enum.GetValues(typeof(T)).Cast<T>().Select(x => MapToSelectListItem(x, selectedValue)).ToList();
        }

        public static SelectListItem MapToSelectListItem<T>(T enumValue, T? selectedValue = null) where T : struct, IComparable, IConvertible, IFormattable
        {
            return new SelectListItem
            {
                Value = enumValue.ToString(CultureInfo.InvariantCulture),
                Text = enumValue.GetDescription(),
                Selected = selectedValue.HasValue && enumValue.Equals(selectedValue.Value),
            };
        }
    }
}