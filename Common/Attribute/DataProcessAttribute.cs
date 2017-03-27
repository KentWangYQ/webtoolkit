using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public enum DataProcessType
    {
        Object,
        Json,
        List,
        Default
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class DataProcessAttribute : Attribute
    {
        public DataProcessAttribute(DataProcessType processType)
        {
            this.ProcessType = processType;
        }
        public DataProcessType ProcessType { get; set; }
    }
}
