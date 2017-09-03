using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WildMouse.Unearth.GeoFilterLoadApp
{
    class Program
    {
        private static SearchServiceClient _searchClient;
        //TODO: put your search service name here
        private static string _searchServiceName = "REDACTED";
        //TODO put your search service admin key here
        private static string _adminKey = "REDACTED";
        private static string _indexName = "oranges-and-lemons-index";

        private static List<OandLDocument> _verses
            = new List<OandLDocument>() {
                new OandLDocument()
                    { documentId = "1",
                    geoPoint = GeographyPoint.Create(51.51145, -0.08683),
                    churchName = "St. Clement's, Eastcheap",
                    text = @"Oranges and lemons, Say the bells of St Clement's."},
                new OandLDocument()
                    { documentId = "2",
                    churchName = "St. Margaret's, London",
                    geoPoint = GeographyPoint.Create(51.5, -0.126944),
                    text =@"Bull's eyes and targets, Say the bells of St Margaret's."},
                new OandLDocument()
                    { documentId = "3",
                    churchName = "St Giles, Cripplegate",
                    geoPoint = GeographyPoint.Create(51.5187, -0.0938),
                    text =@"Brickbats and tiles, Say the bells of St Giles'."},
                new OandLDocument()
                    { documentId = "4",
                    churchName = "St. Martin Ongar",
                    geoPoint = GeographyPoint.Create(51.510757,-0.087569),
                    text =@"Halfpence and farthings, Say the bells of St Martin's."},
                new OandLDocument()
                    { documentId = "5",
                    churchName = "St. Peter upon Cornhill",
                    geoPoint = GeographyPoint.Create(51.5133,-0.0846),
                    text =@"Pancakes and fritters, Say the bells of St Peter's."},
                new OandLDocument()
                    { documentId = "6",
                    churchName = "Whitechapel Bell Foundry",
                    geoPoint = GeographyPoint.Create(51.517400,-0.066890),
                    text =@"Two sticks and an apple, Say the bells at Whitechapel."},
                new OandLDocument()
                    { documentId = "7",
                    churchName = "St. Katherine Cree",
                    geoPoint = GeographyPoint.Create(51.5135,-0.0790),
                    text =@"Maids in white aprons, Say the bells at St Katherine's."},
                new OandLDocument()
                    { documentId = "8",
                    churchName = "The Chapel of St John",
                    geoPoint = GeographyPoint.Create(51.5081,-0.0759),
                    text =@"Pokers and tongs, Say the bells of St John's."},
                new OandLDocument()
                    { documentId = "9",
                    churchName = "St. Annes, Soho",
                    geoPoint = GeographyPoint.Create(51.5124, -0.1319),
                    text =@"Kettles and pans, Say the bells of St Ann's."},
                new OandLDocument()
                    { documentId = "10",
                    churchName = "St. Botolph's, Aldgate",
                    geoPoint = GeographyPoint.Create(51.51697,-0.09725),
                    text =@"Old Father Baldpate, Say the slow bells at Aldgate."},
                new OandLDocument()
                    { documentId = "11",
                    churchName = "St. Helen's, Bishopsgate",
                    geoPoint = GeographyPoint.Create(51.514,-0.081823),
                    text =@"You owe me ten shillings, Say the bells at St Helen's."},
                new OandLDocument()
                    { documentId = "12",
                    churchName = "St. Sepulchre, Newgate",
                    geoPoint = GeographyPoint.Create(51.516635, -0.102707),
                    text =@"When will you pay me, Say the bells of Old Bailey."},
                new OandLDocument()
                    { documentId = "13",
                    churchName = "St. Leonard's, Shoreditch",
                    geoPoint = GeographyPoint.Create(51.530206, -0.084445),
                    text =@"When I grow rich, Say the bells of Shoreditch."},
                new OandLDocument()
                    { documentId = "14",
                    churchName = "St. Dunstan's, Stepney",
                    geoPoint = GeographyPoint.Create(51.516817,-0.041802),
                    text =@"Pray when will that be, Say the bells of Stepney."},
                new OandLDocument()
                    { documentId = "15",
                    churchName = "St. Mary-le-Bow, Cheapside",
                    geoPoint = GeographyPoint.Create(51.513699,-0.093539),
                    text = @"I am sure I don't know, Says the great bell of Bow."}
            };

        static void Main(string[] args)
        {
            _searchClient = new SearchServiceClient(_searchServiceName, new SearchCredentials(_adminKey));

            Console.WriteLine("Creating Index");
            DropandRecreateSearchIndex();
            AddVersesToIndex(_verses);
            Console.WriteLine("Mischief Managed!");
            Console.ReadLine();
        }


        public static void DropandRecreateSearchIndex()
        {
            // Drop existing index
            if (_searchClient.Indexes.Exists(_indexName))
            {
                _searchClient.Indexes.Delete(_indexName);
            }

            // Create a new Document index
            var docIndex = new Index()
            {
                Name = _indexName,
                Fields = FieldBuilder.BuildForType<OandLDocument>()
            };
            _searchClient.Indexes.Create(docIndex);
        }

        static void AddVersesToIndex(List<OandLDocument> verses)
        {
            var indexClient = _searchClient.Indexes.GetClient(_indexName);
            var docActions = new List<IndexAction<OandLDocument>>();
            foreach(var verse in verses)
            {
                docActions.Add(IndexAction.MergeOrUpload(verse));
            }
            var docBatch = IndexBatch.New(docActions);
            try
            {
                var result = indexClient.Documents.Index(docBatch);
            }
            catch (IndexBatchException e)
            {
                // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                // the batch. Depending on your application, you can take compensating actions like delaying and
                // retrying. For this simple demo, we just log the failed document keys and continue.
                Console.WriteLine("Failed to index some of the documents: " + string.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
            }
        }
    }

}
