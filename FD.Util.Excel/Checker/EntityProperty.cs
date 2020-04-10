using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Excel
{
    public class EntityProperty
    {
        public EntityProperty(string _Name, string _Value, string _DataType)
        {
            Name = _Name;
            Value = _Value;
            DataType = _DataType;
        }
        public string Name { get; set; }
        public string Value { get; set; }
        public string DataType { get; set; }
        public string ToStringValue { get { return "{" + Name + "$" + Value + "$" + DataType + "}"; } }
    }
}
