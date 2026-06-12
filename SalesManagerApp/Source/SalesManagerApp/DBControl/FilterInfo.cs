using DatabaseLib.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagerApp.DBControl
{
    public class FilterInfo
    {
        public List<string> StoreList { get; }
        public List<string> ProductList { get; }
        public List<string> CategoryList { get; }

        public DateTime? StartTime { get; }
        public DateTime? EndTime { get; }

        public FilterInfo(List<string> storelist, List<string> productlist, List<string> categorylist, DateTime? start = null, DateTime? end = null)
        {
            StoreList = new List<string>(storelist);
            ProductList = new List<string>(productlist);
            CategoryList = new List<string>(categorylist);
            StartTime = start;
            EndTime = end;
        }
    }
}
