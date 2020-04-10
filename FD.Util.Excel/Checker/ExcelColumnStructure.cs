using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Excel
{
    //excel工作表中列的结构
    public class ExcelColumnStructure
    {
        /// <summary>
        /// 工作表中列的index
        /// </summary>
        public int ColumnIndex { get; set; }

        /// <summary>
        /// excel中的列名
        /// </summary>
        public string ExcelColumn { get; set; }

    }
}
