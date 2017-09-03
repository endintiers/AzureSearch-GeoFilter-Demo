using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WildMouse.Unearth.GeoFilterLoadApp
{
    [SerializePropertyNamesAsCamelCase]
    public partial class OandLDocument
    {
        [Key]
        public string documentId { get; set; }

        [IsFilterable]
        public GeographyPoint geoPoint { get; set; }

        public string churchName { get; set; }

        [IsSearchable]
        [Analyzer(AnalyzerName.AsString.EnMicrosoft)]
        public string text { get; set; }
    }

}
