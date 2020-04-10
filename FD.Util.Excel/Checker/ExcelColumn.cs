using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Excel
{
    public class ExcelColumn
    {
        /// <summary>
        /// 关联的个数
        /// </summary>
        public decimal RelatedCount { get; set; }

        /// <summary>
        /// 是否为主键列:如果是，要求非空
        /// </summary>
        public decimal IsPK { get; set; }

        /// <summary>
        /// 是否为外键列:如果是，要求非空
        /// </summary>
        public decimal IsFK { get; set; }

        /// <summary>
        /// 是否为外键列:如果是，要求非空
        /// </summary>
        public string FKColumnName { get; set; }

        /// <summary>
        /// excel中的列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 例如中的值
        /// </summary>
        public string ExampleData { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// excel中数据使用的控件类型
        /// </summary>
        public string ControlType { get; set; }

        /// <summary>
        /// 下拉数据的列表
        /// </summary>
        public List<string> DownListData { get; set; }
        
        /// <summary>
        /// 对应字段类型
        /// </summary>
        public decimal? DicTypeId { get; set; }

        /// <summary>
        /// 是否忽略
        /// </summary>
        public bool? Ignore { get; set; }

        /// <summary>
        /// 字段长度
        /// </summary>
        public decimal? ColumnLength { get; set; }
        /// <summary>
        /// 小数位数
        /// </summary>
        public decimal? DecimalLength { get; set; }

        /// <summary>
        /// 是否筛选字段
        /// </summary>
        public decimal? IsFilter { get; set; }
        /// <summary>
        /// 字段更新类型: 0:不更新,1更新字段名,2新增,3类型更改,4text长度修改,5数字精度修改
        /// </summary>
        public decimal? UpdateType { get; set; }
        /// <summary>
        /// 数据库字段名
        /// </summary>
        public string fieldName { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        public decimal? IsDisable { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public decimal? Sort { get; set; }
    }
}
