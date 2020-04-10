using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Excel
{
    public class ExcelSheetStructure
    {
        /// <summary>
        /// 工作薄中工作表的index
        /// </summary>
        public int SheetIndex { get; set; }

        /// <summary>
        /// Excel工作表名称
        /// </summary>
        public string ExcelSheetName { get; set; }

        public bool ColumnContains(string columnName)
        {
            bool result = false;
            if (_ColumnStructures != null && _ColumnStructures.Any())
            {
                if (_ColumnStructures.Any(s => s.ExcelColumn == columnName))
                {
                    result = true;
                }
            }
            return result;
        }

        public int ColumnNum
        {
            get
            {
                int num = 0;
                if (_ColumnStructures != null)
                {
                    num = _ColumnStructures.Count();
                }
                return num;
            }
        }

        public ExcelColumnStructure ColumnStructure(int index)
        {
            ExcelColumnStructure aColumnRule = new ExcelColumnStructure();
            if (_ColumnStructures != null && _ColumnStructures.Count() > index)
            {
                aColumnRule = _ColumnStructures[index];
            }
            return aColumnRule;
        }

        public ExcelColumnStructure ColumnStructure(string columnName)
        {
            ExcelColumnStructure aColumnStructure = new ExcelColumnStructure();
            if (_ColumnStructures != null && _ColumnStructures.Any())
            {
                if (_ColumnStructures.Any(s => s.ExcelColumn == columnName))
                {
                    aColumnStructure = _ColumnStructures.Single(s => s.ExcelColumn == columnName);
                }
            }
            return aColumnStructure;
        }

        public List<string> ColumnNameList()
        {
            List<string> result = new List<string>();
            if (_ColumnStructures != null && _ColumnStructures.Count > 0)
            {
                result.AddRange(_ColumnStructures.Select(c => c.ExcelColumn));
            }
            return result;
        }

        public List<ExcelColumnStructure> _ColumnStructures = new List<ExcelColumnStructure>();
    }
}
