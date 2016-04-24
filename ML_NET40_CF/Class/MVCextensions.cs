using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace ML_NET40_CF.Class
{
    public static class LinqExtensions
    {
        public static SelectList ToSelectList<TSource, TValue, TText>(
          this IEnumerable<TSource> source,
          Expression<Func<TSource, TValue>> dataValueField,
          Expression<Func<TSource, TText>> dataTextField)
        {
            if (source == null)
            {
                return new SelectList(new List<SelectListItem>());
            }
            string dataName = ExpressionHelper.GetExpressionText(dataValueField);
            string textName = ExpressionHelper.GetExpressionText(dataTextField);
            return new SelectList(source, dataName, textName);
        }
    }
}