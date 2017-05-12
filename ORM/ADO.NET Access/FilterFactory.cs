using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class FilterFactory
    {
        public static SqlParameter[] SqlParameterCreator(IFilterCondition Filter)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();

            foreach (var property in Filter.GetType().GetProperties())
            {
                var value = property.GetValue(Filter);
                if (value != null)
                    paramList.Add(new SqlParameter()
                    {
                        ParameterName = string.Format("@{0}", property.Name),
                        Value = value
                    });
            }
            return paramList.ToArray<SqlParameter>();
        }

        public static SqlParameter[] SqlParameterCreator(dynamic Filter)
        {
            List<SqlParameter> paramList = new List<SqlParameter>();

            foreach (var property in Filter.GetType().GetProperties())
            {
                var value = property.GetValue(Filter);
                if (value != null)
                    paramList.Add(new SqlParameter()
                    {
                        ParameterName = string.Format("@{0}", property.Name),
                        Value = value
                    });
            }
            return paramList.ToArray<SqlParameter>();
        }

        public static string CacheKeyCreator(string Category, IFilterCondition Filter)
        {
            string key = Category;

            foreach (var property in Filter.GetType().GetProperties())
            {
                var value = property.GetValue(Filter);
                if (value != null)
                    key += string.Format("_{0}-{1}", property.Name, value);
            }
            return key;
        }
    }
}
