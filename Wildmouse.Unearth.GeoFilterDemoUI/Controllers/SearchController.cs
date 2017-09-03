using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wildmouse.Unearth.GeoFilterDemoUI.Helpers;
using Wildmouse.Unearth.GeoFilterDemoUI.Contracts;

namespace Wildmouse.Unearth.GeoFilterDemoUI.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    { 
        // How you know you are alive...
        [HttpGet]
        public string Get()
        {
            return "hello world";
        }

        // Perform a filtered search
        [HttpPost]
        public OAndLSearchResult Post([FromBody]SearchQuery sq)
        {
            var SearchResult = SearchHelper.Search(sq.QueryText, sq.FilterText);
            return SearchResult;
        }

    }
}
