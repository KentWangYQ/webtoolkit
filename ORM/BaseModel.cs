using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ORM
{
    public class BaseModel
    {
        public int ID { get; set; }
        public T ConvertTo<T>()
        {
            if (!this.GetType().IsSubclassOf(typeof(T)))
                throw new Exception(string.Format("Can not Convert type {0} to type {1}", this.GetType().FullName, typeof(T).FullName));
            var instance = Activator.CreateInstance<T>();
            foreach (System.Reflection.PropertyInfo property in typeof(T).GetProperties())
            {
                property.SetValue(instance, this.GetType().GetProperty(property.Name).GetValue(this, null), null);
            }
            return instance;
        }
    }
}
