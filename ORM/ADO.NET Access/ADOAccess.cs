using Common;
using Common.Util;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class ADOAccess
    {
        public static List<object> ExecuteGetBySql(string sql, CommandType commandType = CommandType.Text, SqlParameter[] commandParamters = null)
        {
            List<object> result = new List<object>();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.connectionStr, commandType, sql, commandParamters))
            {
                while (reader.Read())
                {
                    result.Add(reader.GetValue(0));
                }
            }
            return result;
        }

        public static List<T> ExecuteGetBySql<T>(string sql, CommandType commandType = CommandType.Text, SqlParameter[] commandParamters = null, string fieldPrefix = "")
        {
            List<T> DataList = new List<T>();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.connectionStr, commandType, sql, commandParamters))
            {
                while (reader.Read())
                {
                    T RowInstance = Activator.CreateInstance<T>();//动态创建数据实体对象  
                    //通过反射取得对象所有的Property  
                    foreach (PropertyInfo Property in typeof(T).GetProperties())
                    {
                        try
                        {
                            //var type = Property.PropertyType;
                            //取得当前数据库字段的顺序  
                            int Ordinal = -1;
                            try
                            {
                                Ordinal = reader.GetOrdinal(fieldPrefix + Property.Name);
                            }
                            catch { continue; }
                            if (reader.GetValue(Ordinal) != DBNull.Value)
                            {
                                //将DataReader读取出来的数据填充到对象实体的属性里  
                                Property.SetValue(RowInstance, reader.GetValue(Ordinal), null);
                            }
                        }
                        catch (Exception ex)
                        {
                            TxtUtility.TxtWrite("SQL ExecuteGet Faield! Error Info: Property:" + Property.Name + " Error Message:  " + ex.Message + "------StackTrace:" + ex.StackTrace, TxtUtility.LogType.Error, "ADOAccess");
                        }
                    }
                    DataList.Add(RowInstance);
                }
            }
            return DataList;
        }


        public static List<T> ExecuteGetForModelBySql<T>(string sql, CommandType commandType = CommandType.Text, SqlParameter[] commandParamters = null) where T : BaseModel
        {
            List<T> DataList = new List<T>();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(SqlHelper.connectionStr, commandType, sql, commandParamters))
            {
                while (reader.Read())
                {

                    T RowInstance = null;
                    int index = 0;
                    try
                    {
                        index = reader.GetOrdinal("ID");
                    }
                    catch { continue; }
                    if (reader.GetValue(index) != DBNull.Value)
                    {
                        RowInstance = GetItemByID(DataList, Convert.ToInt32(reader.GetValue(index)));
                        if (RowInstance == null)
                            RowInstance = Activator.CreateInstance<T>();
                    }

                    RowInstance = (T)FillValue(reader, typeof(T), RowInstance);
                    DataList.Add(RowInstance);
                }
            }
            return DataList;
        }

        private static Object FillValue(SqlDataReader reader, Type type, Object RowInstance, bool isLinkedTable = false)
        {
            bool nullKey = true;
            if (RowInstance == null)
                RowInstance = Activator.CreateInstance(type);//动态创建数据实体对象  
            foreach (PropertyInfo Property in type.GetProperties())
            {
                try
                {
                    int Ordinal = -1;
                    var attr = (DataProcessAttribute)Property.GetCustomAttribute(typeof(DataProcessAttribute), false);
                    switch (attr == null ? DataProcessType.Default : attr.ProcessType)
                    {
                        case DataProcessType.Object:
                            Type otype = Property.PropertyType;
                            var obj = FillValue(reader, otype, Activator.CreateInstance(otype));
                            Property.SetValue(RowInstance, obj, null);
                            break;
                        case DataProcessType.Json:
                            try
                            {
                                Ordinal = reader.GetOrdinal(string.Format("{0}_{1}", isLinkedTable ? type.Name : "", Property.Name));
                            }
                            catch { continue; }
                            if (reader.GetValue(Ordinal) != DBNull.Value)
                            {
                                //将DataReader读取出来的数据填充到对象实体的属性里  
                                Type t = Property.PropertyType;
                                var dl = JsonConvert.DeserializeObject(reader.GetValue(Ordinal).ToString(), t) as IList;
                                Property.SetValue(RowInstance, dl, null);
                            }
                            break;
                        case DataProcessType.List:
                            var RowofListInstance = Activator.CreateInstance(Property.PropertyType.GetGenericArguments().FirstOrDefault(), true);
                            if ((IList)Property.GetValue(RowInstance) == null)
                                Property.SetValue(RowInstance, Activator.CreateInstance(Property.PropertyType, true));
                            var v = FillValue(reader, Property.PropertyType.GetGenericArguments().FirstOrDefault(), RowofListInstance, true);
                            if (v != null)
                                ((IList)Property.GetValue(RowInstance)).Add(v);
                            break;
                        default:
                            try
                            {
                                Ordinal = reader.GetOrdinal(string.Format("{0}_{1}", isLinkedTable ? type.Name : "", Property.Name));
                            }
                            catch { continue; }
                            if (reader.GetValue(Ordinal) != DBNull.Value)
                            {
                                //将DataReader读取出来的数据填充到对象实体的属性里  
                                Property.SetValue(RowInstance, reader.GetValue(Ordinal), null);
                                nullKey = false;
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    TxtUtility.TxtWrite("SQL ExecuteGet Faield! Error Info: Property:" + Property.Name + " Error Message:  " + ex.Message + "------StackTrace:" + ex.StackTrace, TxtUtility.LogType.Error, "ADOAccess");
                }
            }
            return nullKey ? null : RowInstance;
        }

        private static T GetItemByID<T>(List<T> dataList, int ID) where T : BaseModel
        {
            foreach (var item in dataList)
            {
                if (item.ID == ID)
                    return item;
            }
            return null;
        }

        public static string Pager(string sql, string orderby, string top, string skip)
        {
            StringBuilder sqlBuilder = new StringBuilder(sql);
            sqlBuilder.Append(string.IsNullOrEmpty(orderby) ? "" : " order by " + orderby);
            if (!string.IsNullOrEmpty(top))
            {
                if (string.IsNullOrEmpty(orderby))
                    sqlBuilder.Append(" order by 1");
                sqlBuilder.Append(string.Format(" OFFSET {0} ROWS", string.IsNullOrEmpty(skip) ? "0" : skip));
                sqlBuilder.Append(string.Format(" FETCH NEXT {0} ROWS ONLY", string.IsNullOrEmpty(top) ? "1" : top));
            }
            return sqlBuilder.ToString();
        }

    }
}
