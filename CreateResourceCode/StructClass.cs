using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateResourceCode
{
    public class OrganizationItem
    {
        /// <summary>
        /// 数据表 SRV的索引
        /// </summary>
        public List<srvitem> datarow;
    
    }

    public class srvitem
    {
        /// <summary>
        /// 数据表 SRV的索引
        /// </summary>
        public string SRV_ID { get; set; }

        public string tmpSRV_LOGICAL_CODE_GB { get; set; }
    }
}
