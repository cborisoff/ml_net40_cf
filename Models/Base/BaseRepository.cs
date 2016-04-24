using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Base.Model
{
    public class BaseRepository : IDisposable
    {
        protected BaseUnitOfWork data;
        public BaseRepository()
        {
        }



        //protected DkhEntityContext Context { get; set; }
        public DbContext Context { get; set; }

        public virtual DbSet<T> Query<T>() where T : class
        {
            var DbSet = this.Context.Set<T>();
            return DbSet;
        }
        public virtual IQueryable<T> Query<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var DbSet = this.Context.Set<T>().Where(predicate);
            return DbSet;
        }
        public virtual T Find<T>(object id) where T : class
        {
            var DbSet = this.Context.Set<T>();
            return DbSet.Find(id);
        }

        public virtual void Add<T>(T entity) where T : class
        {
            var DbSet = this.Context.Set<T>();
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Detached)
            {
                entry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
        }

        public virtual void Update<T>(T entity) where T : class
        {
            var DbSet = this.Context.Set<T>();
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public virtual void Delete<T>(T entity) where T : class
        {
            var DbSet = this.Context.Set<T>();
            DbEntityEntry entry = this.Context.Entry(entity);
            if (entry.State != EntityState.Deleted)
            {
                entry.State = EntityState.Deleted;
            }
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        public virtual void Delete<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var entitiesToBeDeleted = Query<T>(predicate);
            foreach (var entity in entitiesToBeDeleted)
            {
                Delete<T>(entity);
            }
        }


        public virtual void Delete<T>(object id) where T : class
        {
            var DbSet = this.Context.Set<T>();
            var entity = this.Find<T>(id);

            if (entity != null)
            {
                this.Delete(entity);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }


        public virtual void ChangeOrderInt<T>(object id, bool moveUp, Func<T, int?> orderProp, Expression<Func<T, int?>> setterProp, Expression<Func<T, bool>> predicate = null)
            where T : class
        {
            var DbSet = this.Context.Set<T>();
            var current = this.Find<T>(id);

            T next;
            IQueryable<T> items = DbSet;
            if (predicate != null)
            {
                items = DbSet.Where(predicate);
            }
            if (moveUp)
            {
                next = items.AsEnumerable().Where(gyz => orderProp(gyz) < orderProp(current)).OrderByDescending(x => orderProp(x)).FirstOrDefault();
            }
            else
            {
                next = items.AsEnumerable().Where(x => orderProp(x) > orderProp(current)).OrderBy(x => orderProp(x)).FirstOrDefault();
            }
            if (next != null)
            {
                int? middleValue = orderProp(current);
                current.SetPropertyValue<T, int?>(setterProp, orderProp(next));
                next.SetPropertyValue<T, int?>(setterProp, middleValue.Value);
                Update<T>(current);
                Update<T>(next);
                SaveChanges();
            }

        }




        private Tres GetValueFromExpression<T, Tres>(Expression<Func<T>> e) where T : class
        {
            MemberExpression member = (MemberExpression)e.Body;
            Expression strExpr = member.Expression;
            return Expression.Lambda<Func<Tres>>(strExpr).Compile()();
        }


        public virtual void Detach<T>(T entity) where T : class
        {
            DbEntityEntry entry = this.Context.Entry(entity);

            entry.State = EntityState.Detached;
        }

        public int SaveChanges()
        {
            return this.Context.SaveChanges();
        }

        public void Dispose()
        {
            this.Context.Dispose();
        }

        public T Other<T>() where T : BaseRepository
        {
            T result = (T)Activator.CreateInstance(typeof(T));
            result.Context = this.Context;

            return result;
        }

        public IList<TSpEntity> ExecProcedure<TSpEntity>(string storedProcedureName, List<SqlParameter> parameters) where TSpEntity : class
        {
            string sql = storedProcedureName + " ";
            for (int i = 0; i < parameters.Count; i++)
            {
                sql += "@" + parameters[i].ParameterName;
                if (i != parameters.Count - 1)
                {
                    sql += ", ";
                }
            }

            return this.Context.Database.SqlQuery<TSpEntity>(sql, parameters.ToArray()).ToList();
        }
        public IList<int?> ExecProcedure_int(string storedProcedureName, List<SqlParameter> parameters)
        {
            string sql = storedProcedureName + " ";
            for (int i = 0; i < parameters.Count; i++)
            {
                sql += "@" + parameters[i].ParameterName;
                if (i != parameters.Count - 1)
                {
                    sql += ", ";
                }
            }

            return this.Context.Database.SqlQuery<int?>(sql, parameters.ToArray()).ToList();
        }
        public void ExecuteSqlCommand(string sql, List<SqlParameter> parameters)
        {
            this.Context.Database.ExecuteSqlCommand(sql, parameters.ToArray());
        }
        public IEnumerable<T> SkipTakePageRecords<T>(IEnumerable<T> source, int page_no, int page_size, out int record_count) where T : class
        {
            record_count = 0;

            if (source != null)
            {
                record_count = source.Count();
                int fromRow = (page_no - 1) * page_size;
                return source.Skip(fromRow).Take(page_size).ToArray();
            }
            else
            {
                return null;
            }
        }


        #region CastToSqlDbValue methods

        public object CastToSqlDbValue(string value)
        {
            return value != null ? value : SqlString.Null;
        }

        public object CastToSqlDbValue(int value)
        {
            return value;
        }

        public object CastToSqlDbValue(int? value)
        {
            return value.HasValue ? value.Value : SqlInt32.Null;
        }

        public object CastToSqlDbValue(bool value)
        {
            return value;
        }

        public object CastToSqlDbValue(bool? value)
        {
            return value.HasValue ? value.Value : SqlBoolean.Null;
        }

        public object CastToSqlDbValue(DateTime value)
        {
            return value;
        }

        public object CastToSqlDbValue(DateTime? value)
        {
            return value.HasValue ? value.Value : SqlDateTime.Null;
        }

        public object CastToSqlDbValue(Guid value)
        {
            return value;
        }

        public object CastToSqlDbValue(Guid? value)
        {
            return value.HasValue ? value.Value : SqlGuid.Null;
        }

        public object CastToSqlDbValue(byte[] value)
        {
            return value != null ? value : SqlBinary.Null;
        }

        #endregion

        /// <summary>
        /// Връща името на текущото repository + "." + името на извикания метод
        /// </summary>
        protected string CurrentLocation
        {
            get
            {
                try
                {
                    StackTrace trace = new StackTrace();
                    int caller = 1;

                    StackFrame frame = trace.GetFrame(caller);

                    MethodBase _mb = frame.GetMethod();

                    string methodName = _mb.Name;
                    string typeName = _mb.ReflectedType.Name;

                    return typeName + "." + methodName;
                }
                catch (Exception e)
                {
                    return string.Empty;
                }

            }
        }

        /// <summary>
        /// Връща името на текущото repository + "." + името на извикания метод
        /// </summary>
        protected static string CurrentLocationStatic
        {
            get
            {
                return System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name + "." + System.Reflection.MethodBase.GetCurrentMethod().Name;
            }
        }
    }

    public static class LambdaExtensions
    {
        public static void SetPropertyValue<T, Tobj>(this T target, Expression<Func<T, Tobj>> memberLamda, Tobj value)
        {
            var memberSelectorExpression = memberLamda.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(target, value, null);
                }
            }
        }

    }
}
