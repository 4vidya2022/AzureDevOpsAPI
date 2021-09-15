# AzureDevOpsAPI-POC
## Sample C# console application to call Azure DevOps API
            Replace following variables with your own Azure DevOps environment variables
                        public const string BASE = "https://dev.azure.com";
                        public const string PAT = "";
                        public const string ORG = "";
                        public const string API = "api-version=6.0";
                        public const string PROJECT = "Demo-Project";
                        public const string WikiIdentifier = "Demo-Project.wiki";
                        public const string PageName = "Azure Wiki Page TesT";

## Authenticate Azure DevOps using PAT (Personal Access Token)
following code snippet is used to authenticate console application written in C# to authenticate Azure DevOPS. read write permission should be given to PAT.

        // Create and initialize HttpClient instance.
            HttpClient client = new HttpClient();

            // Set Media Type of Response.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Generate base64 encoded authorization header.
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "", Azure.PAT))));

## Create  Azure DevOps Wiki Page
Following code snippets is used to create Azure DevOps Wiki Page with some content in it.

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
        }
        
 ## Read Azure DevOps Wiki Page Content
 Following method is used to read content from Azure DevOps Wiki Page
 
            public static async Task<string> ReadWikiPageContent(HttpClient client)
        {
            // Build the URI for creating Work Item.
            string uri = String.Join("&", String.Join("/", Azure.BASE, Azure.ORG, Azure.PROJECT, "_apis/wiki/wikis", Azure.WikiIdentifier, "pages?path=/" + Azure.PageName + "&includeContent=True"), Azure.API);
            try
            {
                // Send asynchronous Get request.
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
        
 ## Delete Azure DevOps Wiki Page
 Following method is used to delete specific Azure DevOps Wiki Page
 
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
           
