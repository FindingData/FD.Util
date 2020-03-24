using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Web
{
    public class PageRequest
    {
        public int page_num { get; set; }

        public int page_size { get; set; }

        public string order_by { get; set; }
    }
}
