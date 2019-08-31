using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuilingTest.model
{
   public class allnode
    {
        public root root { get; set; }
        public List<building> buildinglist { get; set; } = new List<building>();
        public List<meter> meterlist { get; set; } = new List<meter>();
    }
}
