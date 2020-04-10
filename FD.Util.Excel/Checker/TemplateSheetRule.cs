using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Excel
{
    public class TemplateSheetRule
    {
        /// <summary>
        /// Excel工作表名称
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// 表中的列集
        /// </summary>
        public IList<ExcelColumn> ExcelColumns { get; set; }       

        /// <summary>
        /// sheet页所关联的表集
        /// </summary>
        public TemplateSheetDBTable TemplateSheetDbTable { get; set; }

        public ExcelColumn ExcelColumnKey(string columnName)
        {
            ExcelColumn aExcelColumn = null;
            if (ExcelColumns != null && ExcelColumns.Any(c => c.ColumnName == columnName))
            {
                aExcelColumn = ExcelColumns.SingleOrDefault(c => c.ColumnName == columnName);
            }
            return aExcelColumn;
        }

        //以下为方法
        public TemplateColumnRule ColumnRule(string columnName)
        {
            TemplateColumnRule aColumnRule = TemplateSheetDbTable.ColumnRule(columnName);
            if (aColumnRule == null
                || string.IsNullOrEmpty(aColumnRule.FsqField))
            {
                aColumnRule = null;
            }
            return aColumnRule;
        }

        public bool IsEveryColumnCorred()
        {
            bool result = ExcelColumns.All(aExcelColumn => ColumnRule(aExcelColumn.ColumnName) != null);
            return result;
        }
        public List<string> ColumnNameList()
        {
            List<string> result = new List<string>();
            if (ExcelColumns != null && ExcelColumns.Count > 0)
            {
                result.AddRange(ExcelColumns.Select(c => c.ColumnName));
            }
            return result;
        }

        public List<string> PKColumnNameList()
        {
            List<string> result = new List<string>();
            foreach (ExcelColumn aExcelColumns in ExcelColumns.Where(aExcelColumns => aExcelColumns.IsPK == 1 && !result.Contains(aExcelColumns.ColumnName)))
            {
                result.Add(aExcelColumns.ColumnName);
            }
            return result;
        }

        /// <summary>
        /// 返回建值对《key：外键ColumnName,Value：父Sheet中对应列名》
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> FKColumnNameList()
        {
            Dictionary<string, string> result = null;
            foreach (ExcelColumn aExcelColumns in ExcelColumns.Where(aExcelColumns => aExcelColumns.IsFK == 1))
            {
                if (result == null) result = new Dictionary<string, string>();
                if (!result.Keys.Contains(aExcelColumns.ColumnName))
                {
                    result.Add(aExcelColumns.ColumnName, aExcelColumns.FKColumnName);
                }
            }
            return result;
        }

        public int ColumnNum()
        {
            int reslut = 0;
            if (ExcelColumns != null && ExcelColumns.Count > 0)
            {
                reslut = ExcelColumns.Count;
            }
            return reslut;
        }

        public TemplateSheetDBTable SheetDBTable(string dbTableName)
        {
            TemplateSheetDBTable result = null;
            if (TemplateSheetDbTable.DBTableName.ToLower() == dbTableName)
            {
                result = TemplateSheetDbTable;
            }
            return result;
        }

    }
}
