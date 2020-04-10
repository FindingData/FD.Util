using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Excel
{
    public class ExcelCheckError
    {

        public int FileOrderNo { get; set; }

        public string FileName { get; set; }

        public string FileFullName { get; set; }

        private string _templateName;
        public string TemplateName
        {
            get
            {
                if (!string.IsNullOrEmpty(_templateName))
                {
                    return "《" + _templateName + "》";
                }
                return _templateName;
            }
            set { _templateName = value; }
        }

        public string ErrorType { get; set; }

      
        /// <summary>
        /// 错误信息--建在对
        /// key:错误类型
        /// </summary>
        public Dictionary<string, List<string>> Errorlist = new Dictionary<string, List<string>>();

        public string ErrorBrief
        {
            get
            {
                string result = null;
                var i = 1;
                foreach (var aError in Errorlist)
                {
                    var errorCount = aError.Value[0].Contains("*----")
                                    ? aError.Value.Count - 1
                                    : aError.Value.Count;
                    if (i == 1)
                    {
                        result = string.Format("{0}:{1}（{2}处）；", i, aError.Key, errorCount);
                    }
                    //1:列错误（5处）；
                    else
                        result = string.Format("{0}{1}{2}:{3}（{4}处）；", result, Environment.NewLine, i,
                            aError.Key, errorCount);
                    i++;
                }
                return result;
            }
        }

        //<br>\n private string _NewLinestr = System.Environment.NewLine;
        public string ErrorDetailed
        {
            get
            {
                string result = null;
                int i = 1;
                foreach (KeyValuePair<string, List<string>> aError in Errorlist)
                {
                    if (i == 1)
                        result = result + i + ":" + aError.Key + ":";
                    else result = result + Environment.NewLine + i + ":" + aError.Key + ":";
                    foreach (string aErrorDetail in aError.Value)
                    {
                        result = result + Environment.NewLine + "  ----" + aErrorDetail;
                    }
                    i++;
                }
                return result;
            }
        }

        public string ErrorDetailedHtml
        {
            get
            {
                string result = null;
                int i = 1;
                foreach (KeyValuePair<string, List<string>> aError in Errorlist)
                {
                    if (i == 1)
                        result = result + i + ":" + aError.Key + ":";
                    else result = result + "<br/>" + i + ":" + aError.Key + ":";
                    foreach (string aErrorDetail in aError.Value)
                    {
                        result = result + "<br/>" + "  ----" + aErrorDetail+"<div class=marBottom></div>";
                    }
                    i++;
                }
                return result;
            }
        }

        public bool IsContainError
        {
            get
            {
                return Errorlist.Keys.Any();
            }
        }
    }
}
