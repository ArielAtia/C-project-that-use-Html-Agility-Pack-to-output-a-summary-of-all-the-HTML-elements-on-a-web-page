using System;
using System.Collections;

namespace HtmlAgilityPack.Samples
{
    class GetDocLinks
    {
        [STAThread]
        static void Main(string[] args)
        {
            HtmlWeb hw = new HtmlWeb();
            string url = @"https://jobs.thejobnetwork.com/Login";
            HtmlDocument doc = hw.Load(url);
            doc.Save("theJobNetworkHome.htm");

            HtmlDocument htmlDoc = new HtmlDocument();
            Console.Write("<div>= ");
            Console.WriteLine(doc.DocumentNode.SelectNodes("//div").Count);

            Console.Write("<link>= ");
            Console.WriteLine(doc.DocumentNode.SelectNodes("//link").Count);

            Console.Write("<script>= ");
            Console.WriteLine(doc.DocumentNode.SelectNodes("//script").Count);

            Console.Write("<a>= ");
            Console.WriteLine(doc.DocumentNode.SelectNodes("//a").Count);

            Console.Write("<span>= ");
            Console.WriteLine(doc.DocumentNode.SelectNodes("//span").Count);

            DocumentWithLinks nwl = new DocumentWithLinks(doc);
            Console.WriteLine("Anchors urls:");
            for (int i = 0; i < nwl.Links.Count; i++)
            {
                Console.WriteLine(nwl.Links[i]);
            }

            Console.WriteLine("Referenced urls:");
            for (int i = 0; i < nwl.References.Count; i++)
            {
                Console.WriteLine(nwl.References[i]);
            }
            Console.ReadKey();
        }
    }
    /// Represents a document that needs linked files to be rendered.
    public class DocumentWithLinks
    {
        private ArrayList _links;
        private ArrayList _references;
        private HtmlDocument _doc;

        /// Creates an instance of a DocumentWithLinkedFiles.
        /// <param name="doc">The input HTML document.</param>
        public DocumentWithLinks(HtmlDocument doc)
        {
            if (doc == null)
            {
                throw new ArgumentNullException("doc");
            }
            _doc = doc;
            GetLinks();
            GetReferences();
        }

        private void GetLinks()
        {
            _links = new ArrayList();
            HtmlNodeCollection atts = _doc.DocumentNode.SelectNodes("//*[@href]");
            if (atts == null)
                return;

            foreach (HtmlNode n in atts)
            {
                ParseLink(n, "href");
            }
        }

        private void GetReferences()
        {
            _references = new ArrayList();
            HtmlNodeCollection hrefs = _doc.DocumentNode.SelectNodes("//a[@href]");
            if (hrefs == null)
                return;

            foreach (HtmlNode href in hrefs)
            {
                _references.Add(href.Attributes["href"].Value);
            }
        }


        private void ParseLink(HtmlNode node, string name)
        {
            HtmlAttribute att = node.Attributes[name];
            if (att == null)
                return;

            // if name = href, we are only interested by <link> tags
            if ((name == "href") && (node.Name != "link"))
                return;

            _links.Add(att.Value);
        }
        /// Gets a list of links as they are declared in the HTML document.
        public ArrayList Links
        {
            get
            {
                return _links;
            }
        }
        /// Gets a list of reference links to other HTML documents, as they are declared in the HTML document.
        public ArrayList References
        {
            get
            {
                return _references;
            }
        }
    }
}