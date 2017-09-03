using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Wildmouse.Unearth.GeoFilterDemoUI.Contracts;

namespace Wildmouse.Unearth.GeoFilterDemoUI.Helpers
{
    public static class SearchHelper
    {
        private static SearchServiceClient _searchClient; 
        private static ISearchIndexClient _searchIndexClient;
        //TODO: Change the search service name and query key if BYO index
        private static string _searchServiceName = "unearthdemo";
        private static string _queryKey = "4C38982C5B39BA43CFA64A63173C0C01";
        private static string _indexName = "oranges-and-lemons-index";

        public static string ConstructorErrorMessage { get; set; }

        static SearchHelper()
        {
            try
            {
                _searchClient = new SearchServiceClient(_searchServiceName, new SearchCredentials(_queryKey));
                _searchIndexClient = _searchClient.Indexes.GetClient(_indexName);
            }
            catch (Exception e)
            {
                ConstructorErrorMessage = e.Message.ToString();
            }
        }

        public static OAndLSearchResult Search(string query, string geoFilter)
        {
            OAndLSearchResult result = new OAndLSearchResult()
            {
                Verses = new List<OandLDocument>()
            };

            try
            {
                // Do the search
                result.Verses = SearchFiltered(query, geoFilter);
            }
            catch (Exception ex)
            {
                var msg = string.Format("Exception in SearchFiltered: {0}", ex.Message.ToString());
                Trace.TraceError(msg);
            }
            return result;
        }

        private static List<OandLDocument> SearchFiltered(string query, string geoFilter)
        {
            var result = new List<OandLDocument>();
            SearchParameters sp = new SearchParameters()
            {
                QueryType = QueryType.Full,
                SearchMode = SearchMode.Any,
                HighlightFields = new List<string> { "text" },
                HighlightPreTag = "<mark><b><i>",
                HighlightPostTag = "</i></b></mark>",
                Filter = geoFilter,
                Top = 1000
            };

            // Do a search
            var dsr = _searchIndexClient.Documents.Search(query, sp);

            // Extract the highlights
            if (dsr != null && dsr.Results != null)
            {
                foreach (var sr in dsr.Results)
                {
                    var doc = new OandLDocument();
                    doc.documentId = (string)sr.Document["documentId"];
                    doc.churchName = (string) sr.Document["churchName"];
                    doc.geoPoint = (GeographyPoint)sr.Document["geoPoint"];
                    doc.text = (string)sr.Document["text"];
                    if (sr.Highlights != null)
                    {
                        doc.highlight = sr.Highlights["text"][0];
                    }
                    result.Add(doc);
                }
            }
            return result;
        }
    }
}
