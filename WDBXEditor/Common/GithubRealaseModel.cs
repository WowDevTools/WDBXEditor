using System;
using System.Collections.Generic;

namespace WDBXEditor.Common
{
    public class GithubRealaseModel
    {
        public string url { get; set; }
        public string assets_url { get; set; }
        public string upload_url { get; set; }
        public string html_url { get; set; }
        public int id { get; set; }
        public string tag_name { get; set; }
        public string target_commitish { get; set; }
        public bool draft { get; set; }
        public object author { get; set; } // placeholder
        public bool prerelease { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public IList<GithubRealaseAssetsModel> assets { get; set; }
        public string tarball_url { get; set; }
        public string zipball_url { get; set; }
        public string body { get; set; }
    }

    public class GithubRealaseAssetsModel
    {
        public string url { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public object uploader { get; set; } // placeholder
        public string content_type { get; set; }
        public string state { get; set; }
        public int size { get; set; }
        public int download_count { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string browser_download_url { get; set; }
    }
}
