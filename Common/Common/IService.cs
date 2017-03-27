using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IService<T>
    {
        IEnumerable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
        IEnumerable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate, System.Linq.Expressions.Expression<Func<T, int>> orderby);
        IEnumerable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate, System.Linq.Expressions.Expression<Func<T, int>> orderby, int skip, int top);
        IEnumerable<T> FindAll();
        IEnumerable<T> FindAll(int pageIndex, int pageSize);
        T FindByID(int id);
        int Count();
        int Count(System.Linq.Expressions.Expression<Func<T, bool>> predicate);
        int Add(T entity);
        int Update(T entity);
        int Delete(T entity);
        int Delete(int id);
    }
}
