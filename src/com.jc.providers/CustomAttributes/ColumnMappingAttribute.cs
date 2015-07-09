using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.jc.providers.CustomAttributes
{
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class ColumnMappingAttribute : Attribute
    {
        readonly string _columnName;

        // Positional argument
        public ColumnMappingAttribute(string columnName)
        {
            this._columnName = columnName;
        }

        public string ColumnName
        {
            get { return _columnName; }
        }
    }
}
