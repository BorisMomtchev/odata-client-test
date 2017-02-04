using NUnit.Framework;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

// https://github.com/object/Simple.OData.Client/wiki/Getting-started-with-Simple.OData.Client
namespace Simple.OData.Tests
{
    public class Products
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    [TestFixture]
    public class SimpleTest
    {
        ODataClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new ODataClient(new ODataClientSettings("http://localhost:50231/")
            {
                // IgnoreResourceNotFoundException = true,
                PreferredUpdateMethod = ODataUpdateMethod.Put,
                OnTrace = (x, y) => Console.WriteLine(string.Format(x, y)),
                // OnApplyClientHandler = h => h.PreAuthenticate = true,
                // Credentials = new NetworkCredential(
                //    ConfigurationManager.AppSettings["username"],
                //    ConfigurationManager.AppSettings["password"]
                // )
            });
        }

        [Test]
        public void test_nunit_setup()
        {
            Assert.AreEqual(2 + 2, 4);
            Console.WriteLine("print...");
        }

        //------------------------------------------------------------------------------


        [Test]
        public void dynamic_fluent_2()
        {
            var x = ODataDynamic.Expression;

            var flocs = _client
                .FindEntriesAsync("Products?$top=2")
                .Result;

            foreach (var f in flocs)
                Console.WriteLine(f["Name"]);
        }

        [Test]
        public void typed_fluent()
        {
            var flocs = _client
                .For<Products>()
                .Filter(x => x.Name == "MS-DOS 2.1 (OEM)")
                .FindEntriesAsync()
                .Result;

            foreach (var f in flocs)
                Console.WriteLine(f.ID + " | " + f.Name);
        }

        [Test]
        public void typed_fluent_2()
        {
            var flocs = _client
                .For<Products>()
                .Top(2)
                .FindEntriesAsync()
                .Result;

            foreach (var f in flocs)
                Console.WriteLine(f.ID + " | " + f.Name);
        }

        [Test]
        public void typed_fluent_3()
        {
            var x = ODataDynamic.Expression;

            var flocs = _client
                .For<Products>()
                .Filter(x.ID == 3)
                .FindEntriesAsync()
                .Result;

            foreach (var f in flocs)
                Console.WriteLine(f.ID + " | " + f.Name);
        }


        [Test]
        public void typed_fluent_create()
        {
            var result = _client
                .For<Products>()
                .Set(new Products()
                {
                    Name = "NEW PRODUCT 1",
                })
                .InsertEntryAsync(true).Result;

            Console.WriteLine(result.ToString());
        }

        [Test]
        public void typed_fluent_update_id()
        {
            var result = _client
                .For<Products>()
                .Filter(x => x.ID == 67)
                .Set(new Products()
                {
                    ID = 67,
                    Name = "UPD PRODUCT 1",
                })
                .UpdateEntryAsync(true).Result;

            Console.WriteLine(result.ToString());
        }

        [Test]
        public void typed_fluent_update_name()
        {
            var result = _client
                .For<Products>()
                .Filter(x => x.Name == "NEW PRODUCT 1")
                .Set(new Products()
                {
                    ID = 67,
                    Name = "UPD PRODUCT 2"
                })
                .UpdateEntryAsync(true).Result;

            Console.WriteLine(result.ToString());
        }
    }
}

