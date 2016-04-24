using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace UDS_Accounts
{
    public static class LinqExtensions
    {


        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return true;

            if (source.Count() == 0)
            {
                return true;
            }

            return false;
        }

        public static IQueryable<T> Include<T>(this IQueryable<T> source, string path)
    where T : class
        {
            //RuntimeFailureMethods.Requires(source != null, null, "source != null");
            DbQuery<T> query = source as DbQuery<T>;

            return query.Include(path); // your case
        }
    }
}