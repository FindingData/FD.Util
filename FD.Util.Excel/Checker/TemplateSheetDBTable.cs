using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Excel
{
    public class TemplateSheetDBTable
    {
        /// <summary>
        /// 关联到的DBTable Name
        /// </summary>
        public string DBTableName { get; set; }

        /// <summary>
        /// 中文表名
        /// </summary>
        public string DBTableName_CHS { get; set; }

        /// <summary>
        /// 关联的序列
        /// </summary>
        public string DB_SEQ { get; set; }

        /// <summary>
        /// 关联的列集
        /// </summary>
        public IList<TemplateColumnRule> ColumnRuleList = new List<TemplateColumnRule>();

        public TemplateColumnRule ColumnRule(string columnName)
        {
            TemplateColumnRule aColumnRule = null;
            if (ColumnRuleList != null && ColumnRuleList.Count > 0)
            {
                if (ColumnRuleList.Any(c => c.ColumnName.ToLower() == columnName.ToLower()))
                {
                    aColumnRule = ColumnRuleList.Single(c => c.ColumnName.ToLower() == columnName.ToLower());
                }
            }
            return aColumnRule;
        }

        public TemplateColumnRule FsqFieldRule(string fsqField)
        {
            TemplateColumnRule aColumnRule = null;
            if (ColumnRuleList != null && ColumnRuleList.Count > 0)
            {
                if (ColumnRuleList.Any(c => c.FsqField.ToLower() == fsqField.ToLower()))
                {
                    aColumnRule = ColumnRuleList.Single(c => c.FsqField.ToLower() == fsqField.ToLower());
                }
            }
            return aColumnRule;
        }
    }
}
