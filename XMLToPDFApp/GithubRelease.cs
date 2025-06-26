namespace XMLToPDFApp
{
    internal class GithubRelease
    {
        public string Id { get; set; }
        public string HtmlUrl { get; set; }
        public string TagName { get; set; }
        public string Name { get; set; }
        public bool Draft { get; set; }
        public bool Prerelease { get; set; }
        public string CreatedAt { get; set; }
        public string PublishedAt { get; set; }
        public string Body { get; set; }
    }
}
