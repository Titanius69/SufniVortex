using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace SufniVortex
{
    public static class AdBlocker
    {
        private static readonly List<Regex> DefaultRules = new List<Regex>
        {
            new Regex(@"(^|\.)doubleclick\.net", RegexOptions.IgnoreCase | RegexOptions.Compiled),
            new Regex(@"(^|\.)adservice\.google\.com", RegexOptions.IgnoreCase | RegexOptions.Compiled),
            new Regex(@"(^|\.)googlesyndication\.com", RegexOptions.IgnoreCase | RegexOptions.Compiled),
            new Regex(@"(^|\.)youtube\.com/(pagead|ad(?:s)?)\b", RegexOptions.IgnoreCase | RegexOptions.Compiled),
            new Regex(@"(^|\.)googlevideo\.com/videoplayback.*(&|\?)sparams=.*(ad|advertising)", RegexOptions.IgnoreCase | RegexOptions.Compiled),
            new Regex(@"tracking|analytics|advertising|pixel", RegexOptions.IgnoreCase | RegexOptions.Compiled)
        };

        private static List<Regex> Rules = new List<Regex>(DefaultRules);

        public static bool LoggingEnabled { get; set; } = false;

        public static bool HideInsteadOfRemove { get; set; } = false;


        public static void LoadRulesFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            try
            {
                var lines = File.ReadAllLines(filePath);
                var customRules = lines
                    .Where(line => !string.IsNullOrWhiteSpace(line) && !line.TrimStart().StartsWith("#"))
                    .Select(line => new Regex(line.Trim(), RegexOptions.IgnoreCase | RegexOptions.Compiled));
                Rules.AddRange(customRules);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading adblock rules: " + ex.Message);
            }
        }


        public static void ResetRules() => Rules = new List<Regex>(DefaultRules);


        public static bool IsAdUrl(string url)
        {
            return !string.IsNullOrEmpty(url) && Rules.Any(rule => rule.IsMatch(url));
        }


        private static void LogRemoval(string message)
        {
            if (LoggingEnabled)
            {
                try
                {
                    File.AppendAllText("adblocker.log", DateTime.Now + ": " + message + Environment.NewLine);
                }
                catch {  }
            }
        }


        private static bool ShouldRemoveNode(HtmlNode node)
        {
            string[] attributesToCheck = { "src", "href", "class", "id", "style", "data-ad" };
            foreach (var attr in attributesToCheck)
            {
                string value = node.GetAttributeValue(attr, "");
                if (!string.IsNullOrEmpty(value))
                {
                    if (Rules.Any(rule => rule.IsMatch(value)))
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public static string RemoveAdsFromHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return html;

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);


            foreach (var node in doc.DocumentNode.Descendants().ToList())
            {
                if (ShouldRemoveNode(node))
                {
                    string info = $"<{node.Name}> eltávolítva. Attribútumok: {string.Join(", ", node.Attributes.Select(a => a.Name + "=\"" + a.Value + "\""))}";
                    LogRemoval(info);

                    if (HideInsteadOfRemove)
                    {

                        string existingStyle = node.GetAttributeValue("style", "");
                        node.SetAttributeValue("style", existingStyle + " display: none !important;");
                    }
                    else
                    {
                        node.Remove();
                    }
                }
            }

            return doc.DocumentNode.OuterHtml;
        }
        public static string GetYouTubeSkipAdScript()
        {
            return @"
                (function() {
                    function removeAds() {
                        var selectors = [
                            'ytd-ad-slot-renderer', 
                            '.ytp-ad-player-overlay', 
                            '.ytp-ad-module', 
                            '.ytp-ad-overlay-container'
                        ];
                        selectors.forEach(function(selector) {
                            document.querySelectorAll(selector).forEach(function(el) { el.remove(); });
                        });
                        var video = document.querySelector('video');
                        if (video && video.duration < 30) {
                            video.currentTime = video.duration;
                        }
                    }
                    setInterval(removeAds, 500);
                })();
            ";
        }


        public static string ProcessPageHtml(string url, string html)
        {
            string cleanedHtml = RemoveAdsFromHtml(html);
            if (!string.IsNullOrWhiteSpace(url) && url.Contains("youtube.com", StringComparison.OrdinalIgnoreCase))
            {

                cleanedHtml = cleanedHtml.Replace("</body>", "<script>" + GetYouTubeSkipAdScript() + "</script></body>");
            }
            return cleanedHtml;
        }
    }
}
