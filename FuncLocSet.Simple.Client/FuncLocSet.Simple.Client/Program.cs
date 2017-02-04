using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FuncLocSet.Simple.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                // any async anything you need here without worry

                // var client = new ODataClient("http://sapfs1didb.erp.xcelenergy.com:8000/sap/opu/odata/SAP/ZGW_FUNC_LOC_SRV/");
                var client = new ODataClient(new ODataClientSettings("http://sapfs1didb.erp.xcelenergy.com:8000/sap/opu/odata/SAP/ZGW_FUNC_LOC_SRV/")
                {
                    // IgnoreResourceNotFoundException = true,
                    OnTrace = (x, y) => Console.WriteLine(string.Format(x, y)),
                    Credentials = new NetworkCredential(
                        ConfigurationManager.AppSettings["username"], 
                        ConfigurationManager.AppSettings["password"]
                    )
                });

                var flocs = await client.FindEntriesAsync("FuncLocSet?$top=2");
                foreach (var floc in flocs)
                    Console.WriteLine(floc["Floc"]);

            }).GetAwaiter().GetResult();
        }
    }
}
