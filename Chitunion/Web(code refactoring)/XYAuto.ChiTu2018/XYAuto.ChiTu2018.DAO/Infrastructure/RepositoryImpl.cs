

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;

namespace XYAuto.ChiTu2018.DAO.Infrastructure
{
    /// <summary>
    /// 注释：RepositoryImpl（仓储模式）
    /// 作者：guansl
    /// 日期：2018/4/19 
    /// </summary>
    public abstract class RepositoryImpl<T> : Disposable, Repository<T> where T : class, new()
    {
        #region 配置
        public Chitunion2017DbContext context;

        public RepositoryImpl(Chitunion2017DbContext _context)
        {
            context = _context;
        }

        public RepositoryImpl()
        {
            context = GetCurrentContext();
            Console.WriteLine("new Chitunion2017DbContext()");
            //context = new Chitunion2017DbContext();
        }

        protected override void DisposeCore()
        {
            Console.WriteLine("DisposeCore");
            if (context != null)
            {
                Console.WriteLine("DisposeCore Dispose");
                context.Dispose();
            }
        }

        public Chitunion2017DbContext GetCurrentContext(string sqlConnString = "")
        {
            //CallContext:保证线程内创建的数据操作上下文是唯一的。
            Chitunion2017DbContext dbContext = (Chitunion2017DbContext)CallContext.GetData(sqlConnString);
            if (dbContext == null)
            {
                dbContext = new Chitunion2017DbContext();
                CallContext.SetData(sqlConnString, dbContext);
            }
            return dbContext;
        }

        #endregion

        #region 实现接口方法
        public virtual T Add(T entity)
        {
            context.Entry(entity).State = EntityState.Added;
            var createRowCount = context.SaveChanges();
            return createRowCount > 0 ? entity : null;
        }

        public virtual int Delete(int id)
        {
            var model = context.Set<T>().Find(id);
            if (model == null)
            {
                throw new ArgumentOutOfRangeException("pk");
            }
            context.Entry(model).State = EntityState.Deleted;
            return context.SaveChanges();
        }

        public virtual List<T> FindAll()
        {
            return context.Set<T>().Where(q => true).ToList();
        }

        public virtual IQueryable<T> Queryable()
        {
            return context.Set<T>();
        }

        public virtual List<T> FindAll(Expression<Func<T, bool>> expression, Expression<Func<T, dynamic>> sortPredicate, SortOrder sortOrder, int skip, int take, out int total)
        {
            total = context.Set<T>().Where(expression).Count();
            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    return context.Set<T>().Where(expression).OrderBy(sortPredicate).Skip(skip).Take(take).ToList();

                case SortOrder.Descending:
                    return context.Set<T>().Where(expression).OrderByDescending(sortPredicate).Skip(skip).Take(take).ToList();

            }
            throw new InvalidOperationException("基于分页功能的查询必须指定排序字段和排序顺序。");

        }

        public virtual T Retrieve(Expression<Func<T, bool>> expression)
        {
            //   return context.Database.ExecuteSqlCommand(sqlCmd, "");
            return context.Set<T>().FirstOrDefault(expression);

        }

        public virtual T Put(T model)
        {
            context.Entry(model).State = EntityState.Modified;
            var updateRowAcount = context.SaveChanges();
            return updateRowAcount > 0 ? model : null;
        }


        //public int UpdateModelFields(T model, List<string> fileds)
        //{
        //    try
        //    {
        //        int updateRowAcount = 0;
        //        if (model != null && fileds != null)
        //        {
        //            context.Set<T>().Attach(model);
        //            var setEntry = ((IObjectContextAdapter)context).ObjectContext.
        //                ObjectStateManager.GetObjectStateEntry(model);
        //            foreach (var t in fileds)
        //            {
        //                setEntry.SetModifiedProperty(t);
        //            }
        //            updateRowAcount = context.SaveChanges();
        //        }
        //        return updateRowAcount;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }

        //}
   

        #endregion

    }
}
