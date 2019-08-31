using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuartzBuildingDemo.model
{
   public class meter
    {
        /// <summary>
        /// 节点属性值
        /// </summary>
        public string attibuteid { get; set; }
        /// <summary>
        /// 节点值
        /// </summary>
        public string innertext { get; set; }
        /// <summary>
        /// 父节点得属性值
        /// </summary>
        public string parentid { get; set; }
    }
}
