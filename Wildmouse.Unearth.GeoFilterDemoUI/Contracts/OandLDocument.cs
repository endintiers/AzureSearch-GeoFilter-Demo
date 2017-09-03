using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Wildmouse.Unearth.GeoFilterDemoUI.Contracts
{
    public class OandLDocument
    {
        public string documentId { get; set; }

        public GeographyPoint geoPoint { get; set; }

        public string churchName { get; set; }

        public string text { get; set; }

        public string highlight { get; set; }
    }

}
