using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace HostelworldScrape
{
    class appVars
    {
        public static string googleUrl;
        public static string outputLoc = @"NO OUTPUT LOCATION SELECTED";
        public static int sitesScanned = 0;
        public static int emailsFound = 0;

        public static BatchCountries staticBatch = new BatchCountries();

        //hostel, site, email
        public static List<Tuple<string, string, List<string>>> dataList = new List<Tuple<string, string, List<string>>>();

        public static string[] ignoreSites = new string[] { @"booking.com", @"hostelworld", @"hostelbookers", @"expedia", @"venere"};
    }

    class MyWebClient : WebClient
    {
        Uri _responseUri;

        public Uri ResponseUri
        {
            get { return _responseUri; }
        }

        protected override WebResponse GetWebResponse(WebRequest request)
        {
            WebResponse response = base.GetWebResponse(request);
            _responseUri = response.ResponseUri;
            return response;
        }
    }
}
