using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wildmouse.Unearth.GeoFilterDemoUI.Contracts
{
    public class GeoPoint
    {
        public string id { get; set; }
        public string pointName { get; set; }
        public double latitude { get; set; }
        public double Longitude { get; set; }
    }
}
