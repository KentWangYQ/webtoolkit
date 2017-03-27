using System;
using System.Collections.Generic;

namespace Common
{
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    #region Interface IRepository<T>
    public interface IRepository<T>
    {
        #region Find
        int Count();

        int Count(System.Linq.Expressions.Expression<Func<T, bool>> predicate);

        IEnumerable<T> FindAll();

        IEnumerable<T> FindAll(int skip, int top);

        T Find(params object[] keyValues);

        IEnumerable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate);

        IEnumerable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate, System.Linq.Expressions.Expression<Func<T, int>> orderby);

        IEnumerable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate, System.Linq.Expressions.Expression<Func<T, int>> orderby, int skip, int top);

        Task<T> FindAsync(params object[] keyValues);

        T FindSingleOrDefault(Expression<Func<T, bool>> predicate);

        Task<T> FindSingleOrDefaultAsync(Expression<Func<T, bool>> predicate);

        T FindFirstOrDefault(Expression<Func<T, bool>> predicate);

        Task<T> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        #endregion

        #region Add

        int Add(T model);

        IEnumerable<T> Add(IEnumerable<T> models);

        Task<int> AddAsync(T model);

        #endregion

        #region Update

        int Update(T model);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="expression">批量更新的where表达式</param>
        /// <param name="updateExpression">更新表达式</param>
        /// <returns></returns>
        //int Update(Expression<Func<T, bool>> expression, Expression<Func<T, T>> updateExpression);

        #endregion

        #region Delete

        int Delete(T model);

        int Delete(IEnumerable<T> models);

        Task<int> DeleteAsync(T model);

        //int Delete(Expression<Func<T, bool>> predicate);

        #endregion

        int SaveChanges();

        Task<int> SaveChangesAsync();

        #region Others

        TResult Excute<TResult>(Expression<Func<IEnumerable<T>, TResult>> expression);

        T Reload(T model);

        void Attach(T model);

        void Detach(T model);

        #endregion

        #region OnAdd, OnUpdate, OnDelete
        void OnAdding(T model);

        void OnAdding(IEnumerable<T> models);

        void OnAdded(T model);

        void OnAdded(IEnumerable<T> models);

        void OnUpdating(T model);

        void OnUpdating(IEnumerable<T> models = null);

        void OnUpdated(T model);

        void OnUpdated(IEnumerable<T> models = null);

        void OnDeleting(T model);

        void OnDeleting(IEnumerable<T> models);

        void OnDeleted(T model);

        void OnDeleted(IEnumerable<T> models);
        #endregion
    }
    #endregion

    #region Interface IRepository<TEntity, in TID>
    public interface IRepository<TEntity, in TID> : IRepository<TEntity>
        where TEntity : IEntity<TID>
        where TID : IEquatable<TID>
    {
        TEntity FindByID(TID id);
        int Delete(TID id);
    }
    #endregion

    #region ICachedRepository<T>
    public interface ICachedRepository<T> : IRepository<T>
    {
        IEnumerable<T> FindAll(bool inCached = false);
    }
    #endregion
}
