using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AzureWikiUpdate
{

   

        static class Azure
        {
            public const string BASE = "https://dev.azure.com";
            public const string PAT = "";
            public const string ORG = "";
            public const string API = "api-version=6.0";
            public const string PROJECT = "Demo-Project";
            public const string WikiIdentifier = "Demo-Project.wiki";
             public const string PageName = "Azure Wiki Page TesT";
        }
    public class Root
    {
        public string content { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // Create and initialize HttpClient instance.
            HttpClient client = new HttpClient();

            // Set Media Type of Response.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Generate base64 encoded authorization header.
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Azure.PAT))));




            // Call CreateWikiPage method.
             bool result = CreateWikiPageWithTable(client).Result;

            //Call Read WikiPage method
            //string result = ReadWikiPage(client).Result;

            // string result = ReadWikiPageContent(client).Result;

            // Delete Wiki Page method
            //  bool result = DeleteWikiPage(client).Result;

            Console.WriteLine(result);
            Console.ReadLine();
            //client.Dispose();
        }

        public static async Task<bool> CreateWikiPageWithTable(HttpClient client)
        {
             // Build the URI for creating Work Item.
             string uri = String.Join("&", String.Join("/", Azure.BASE, Azure.ORG, Azure.PROJECT, "_apis/wiki/wikis", Azure.WikiIdentifier, "pages?path="+ Azure.PageName), Azure.API);
            try
            {
                // Create Request body in JSON format.
                Root root = new Root()
                {
                    content = "<table><tr><th>Company</th><th>Contact</th><th>Country</th></tr><tr><td>Alfreds Futterkiste</td><td>Maria Anders</td><td>Germany</td></tr><tr><td>Centro comercial Moctezuma</td><td>Francisco Chang</td><td>Mexico</td></tr></table>"
                };
                string stringjson = JsonConvert.SerializeObject(root);
                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                var content = new StringContent(stringjson, Encoding.UTF8, "application/json");

                // Send asynchronous POST request.
                using (HttpResponseMessage response = await client.PutAsync(uri, content))
                {
                    response.EnsureSuccessStatusCode();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        } // End of CreateWikiPage method

      // read wiki Page with its content method
        public static async Task<string> ReadWikiPage(HttpClient client)
        {
            // Build the URI for creating Work Item.
            string uri = String.Join("&", String.Join("/", Azure.BASE, Azure.ORG, Azure.PROJECT, "_apis/wiki/wikis", Azure.WikiIdentifier, "pages?path=/"+  Azure.PageName +"&includeContent=True"), Azure.API);
            try
            {
                // Send asynchronous POST request.
                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    response.EnsureSuccessStatusCode();
                    var pageResult = response.Content.ReadAsStringAsync().Result;
                    return (await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }

        public static async Task<string> ReadWikiPageContent(HttpClient client)
        {
            // Build the URI for creating Work Item.
            string uri = String.Join("&", String.Join("/", Azure.BASE, Azure.ORG, Azure.PROJECT, "_apis/wiki/wikis", Azure.WikiIdentifier, "pages?path=/" + Azure.PageName + "&includeContent=True"), Azure.API);
            try
            {
                // Send asynchronous POST request.
                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    response.EnsureSuccessStatusCode();
                    var pageResult = response.Content.ReadAsStringAsync().Result;
                    dynamic json = JValue.Parse(pageResult);
                    var content = json.content;
                    return (content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }

        public static async Task<string> ReadWikiPageVersion(HttpClient client)
        {
            // Build the URI for creating Work Item.
            string uri = String.Join("&", String.Join("/", Azure.BASE, Azure.ORG, Azure.PROJECT, "_apis/wiki/wikis", Azure.WikiIdentifier, "pages?path=/" + Azure.PageName + "&includeContent=True"), Azure.API);
            try
            {
                // Send asynchronous POST request.
                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    response.EnsureSuccessStatusCode();
                    var pageResult = response.Content.ReadAsStringAsync().Result;
                    return ( response.Headers.ETag.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }

        // Delete Wiki Page with its content method
        public static async Task<bool> DeleteWikiPage(HttpClient client)
        {
            // Build the URI for creating Work Item.
            string uri = String.Join("&", String.Join("/", Azure.BASE, Azure.ORG, Azure.PROJECT, "_apis/wiki/wikis", Azure.WikiIdentifier, "pages?path=/" + Azure.PageName), Azure.API);
            try
            {
                // Send asynchronous POST request.
                using (HttpResponseMessage response = await client.DeleteAsync(uri))
                {
                    response.EnsureSuccessStatusCode();
                    var pageResult = response.Content.ReadAsStringAsync().Result;
                    return (true);
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //Insert Table to Existing Page
        // Note its not updating azure wiki page have added  version num er in headers as well. I have followed following documentations https://docs.microsoft.com/en-us/rest/api/azure/devops/wiki/pages/update?view=azure-devops-rest-6.0
        public static async Task<string> InsertTableToPage(HttpClient client)
        {
            var PageVersionNumber = ReadWikiPageVersion(client).Result;
              string uri = String.Join("&", String.Join("/", Azure.BASE, Azure.ORG, Azure.PROJECT, "_apis/wiki/wikis", Azure.WikiIdentifier, "pages?path=" + Azure.PageName), Azure.API);
            try
            {
                // Create Request body in JSON format.
                Root root = new Root()
                {
                    //content = "<table>< tr >< th > Company </ th >< th > Contact </ th >< th > Country </ th ></ tr >< tr >< td > Alfreds Futterkiste </ td >< td > Maria Anders </ td >< td > Germany </ td ></ tr ></ table >"
                    content = "Test Content"
                };
                string stringjson = JsonConvert.SerializeObject(root);
                // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
                var content = new StringContent(stringjson, Encoding.UTF8, "application/json");
                content.Headers.TryAddWithoutValidation("If-Match", PageVersionNumber);
                // Send asynchronous POST request.
                using (HttpResponseMessage response = await client.PutAsync(uri, content))
                {
                    response.EnsureSuccessStatusCode();
                    var pageResult = response.Content.ReadAsStringAsync().Result;
                    return (await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return string.Empty;
            }
        }
    }
}
    
