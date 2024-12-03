using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
namespace Algorithm
{
    public class ResBatch
    {
        public List<SchProductRouteRes> ListSchProductRouteRes = new List<SchProductRouteRes>(10);   //资源任务列表
        public ResBatch()
        {
        }
        public ResBatch(string cWcNo)
        {
            if (cWcNo == "") return;
        }
    }
}