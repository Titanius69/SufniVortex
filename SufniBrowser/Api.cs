using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using System.Timers;
using SufniVortex;
using Xamarin.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using SufniBrowser;

namespace Sufni_Api
{
    public static class Api


    {
        public static void refreshCurrentTab()
        {
            ComplexBrowserForm.Instance?.GetCurrentWebView()?.CoreWebView2.Reload();
        }
        public static void goBack()
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView?.CoreWebView2.CanGoBack == true)
            {
                webView.CoreWebView2.GoBack();
            }
        }

        public static void goForward()
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView?.CoreWebView2.CanGoForward == true)
            {
                webView.CoreWebView2.GoForward();
            }
        }

        public static void stopLoading()
        {
            ComplexBrowserForm.Instance?.GetCurrentWebView()?.CoreWebView2.Stop();
        }

        public static void navigateTo(string url)
        {
            ComplexBrowserForm.Instance?.GetCurrentWebView()?.CoreWebView2.Navigate(url);
        }

        public static async Task<string> getPageSource()
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView != null)
            {
                string html = await webView.CoreWebView2.ExecuteScriptAsync("document.documentElement.outerHTML");
                return System.Text.Json.JsonSerializer.Deserialize<string>(html);
            }
            return string.Empty;
        }

        public static async Task<string> getCurrentUrl()
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            return webView?.Source.ToString() ?? string.Empty;
        }

        public static async Task<string> getPageTitle()
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView != null)
            {
                string title = await webView.CoreWebView2.ExecuteScriptAsync("document.title");
                return System.Text.Json.JsonSerializer.Deserialize<string>(title);
            }
            return string.Empty;
        }

        public static async Task<List<string>> getAllLinks()
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView != null)
            {
                string linksJson = await webView.CoreWebView2.ExecuteScriptAsync(
                    "JSON.stringify([...document.querySelectorAll('a')].map(a => a.href))");
                return System.Text.Json.JsonSerializer.Deserialize<List<string>>(linksJson);
            }
            return new List<string>();
        }

        public static async Task<List<string>> getAllImages()
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView != null)
            {
                string imagesJson = await webView.CoreWebView2.ExecuteScriptAsync(
                    "JSON.stringify([...document.querySelectorAll('img')].map(img => img.src))");
                return System.Text.Json.JsonSerializer.Deserialize<List<string>>(imagesJson);
            }
            return new List<string>();
        }

        public static async Task<string> getMetaDescription()
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView != null)
            {
                string description = await webView.CoreWebView2.ExecuteScriptAsync(
                    "document.querySelector('meta[name=\"description\"]')?.content || ''");
                return System.Text.Json.JsonSerializer.Deserialize<string>(description);
            }
            return string.Empty;
        }

        public static async Task<string> getMetaKeywords()
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView != null)
            {
                string keywords = await webView.CoreWebView2.ExecuteScriptAsync(
                    "document.querySelector('meta[name=\"keywords\"]')?.content || ''");
                return System.Text.Json.JsonSerializer.Deserialize<string>(keywords);
            }
            return string.Empty;
        }

        public static async Task setDarkMode()
        {
            await executeScriptInCurrentTab(
                "document.body.style.backgroundColor = '#121212';" +
                "document.body.style.color = '#ffffff';" +
                "document.querySelectorAll('*').forEach(el => {" +
                "if (getComputedStyle(el).backgroundColor === 'rgb(255, 255, 255)') {" +
                "el.style.backgroundColor = '#333';}});"
            );
        }

        public static async Task setLightMode()
        {
            await executeScriptInCurrentTab(
                "document.body.style.backgroundColor = '#ffffff';" +
                "document.body.style.color = '#000000';" +
                "document.querySelectorAll('*').forEach(el => {" +
                "if (getComputedStyle(el).backgroundColor === 'rgb(18, 18, 18)') {" +
                "el.style.backgroundColor = '#ffffff';}});"
            );
        }

        public static async Task executeScriptInCurrentTab(string script)
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView != null)
            {
                await webView.CoreWebView2.ExecuteScriptAsync(script);
            }
        }

        public static async Task clearLocalStorage()
        {
            await executeScriptInCurrentTab("localStorage.clear();");
        }

        public static async Task clearSessionStorage()
        {
            await executeScriptInCurrentTab("sessionStorage.clear();");
        }

        public static async Task clearCache()
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView != null)
            {
                await webView.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.clearBrowserCache", "{}");
            }
        }
    

        public static void addTab(string url)
        {
            ComplexBrowserForm.Instance?.CreateNewTab(url);
        }


        public static void GetCurrentWebView()
        {
            ComplexBrowserForm.Instance?.GetCurrentWebView();
        }

        public static void clearCookies()
        {
            ComplexBrowserForm.Instance?.ClearCookies();
        }

        public static void loadCookies(WebView2 webView)
        {
            ComplexBrowserForm.Instance?.LoadCookiesIntoBrowser(webView);
        }

        public static void getBrowserControl ()
        {
            ComplexBrowserForm.Instance?.GetBrowserControl();
        }

        public static void encrypt(string to_encrypt_text)
        {
            CustomCrypto.Encrypt(to_encrypt_text);
        }

        public static void decrypt(string to_decrypt_text)
        {
            CustomCrypto.Decrypt(to_decrypt_text);
        }
    



        public static void duplicateCurrentTab()
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView != null)
            {
                string url = webView.Source.ToString();
                ComplexBrowserForm.Instance?.CreateNewTab(url);
            }
        }







        public static async Task<Dictionary<string, string>> getAllFormInputs()
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView != null)
            {
                string json = await webView.CoreWebView2.ExecuteScriptAsync(
                    "JSON.stringify([...document.querySelectorAll('input, textarea')].reduce((acc, el) => { acc[el.name || el.id || el.type] = el.value; return acc; }, {}));"
                );
                return System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            }
            return new Dictionary<string, string>();
        }

        public static async Task autofillForm(Dictionary<string, string> formData)
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView != null)
            {
                string script = "(() => {";
                foreach (var pair in formData)
                {
                    script += $"document.querySelector('[name=\"{pair.Key}\"], [id=\"{pair.Key}\"]').value = \"{pair.Value}\";";
                }
                script += "})();";
                await webView.CoreWebView2.ExecuteScriptAsync(script);
            }
        }

        public static async Task toggleImages(bool showImages)
        {
            string script = showImages
                ? "document.querySelectorAll('img').forEach(img => img.style.display = 'block');"
                : "document.querySelectorAll('img').forEach(img => img.style.display = 'none');";

            await executeScriptInCurrentTab(script);
        }

        public static async Task toggleJavaScript(bool enable)
        {
            await executeScriptInCurrentTab(enable
                ? "document.querySelectorAll('script').forEach(script => script.removeAttribute('disabled'));"
                : "document.querySelectorAll('script').forEach(script => script.setAttribute('disabled', 'true'));");
        }

        public static async Task downloadImage(string imageUrl, string savePath)
        {
            using (HttpClient client = new HttpClient())
            {
                byte[] imageBytes = await client.GetByteArrayAsync(imageUrl);
                await System.IO.File.WriteAllBytesAsync(savePath, imageBytes);
            }
        }

        public static async Task<List<string>> getYouTubeVideos()
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView != null)
            {
                string json = await webView.CoreWebView2.ExecuteScriptAsync(
                    "JSON.stringify([...document.querySelectorAll('a[href*=\"youtube.com/watch\"]')].map(a => a.href));"
                );
                return System.Text.Json.JsonSerializer.Deserialize<List<string>>(json);
            }
            return new List<string>();
        }

        public static async Task copyCurrentUrlToClipboard()
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView != null)
            {
                string url = webView.Source.ToString();
                Clipboard.SetText(url);
            }
        }

        public static async Task savePageAsPdf(string filePath)
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView != null)
            {
                await webView.CoreWebView2.CallDevToolsProtocolMethodAsync(
                    "Page.printToPDF",
                    $"{{\"path\": \"{filePath}\", \"landscape\": false, \"printBackground\": true}}"
                );
            }
        }

        public static async Task<int> searchForWord(string word)
        {
            var webView = ComplexBrowserForm.Instance?.GetCurrentWebView();
            if (webView != null)
            {
                string count = await webView.CoreWebView2.ExecuteScriptAsync(
                    $"JSON.stringify([...document.body.innerText.matchAll(/\\b{Regex.Escape(word)}\\b/gi)].length);"
                );
                return System.Text.Json.JsonSerializer.Deserialize<int>(count);
            }
            return 0;
        }

        public static async Task highlightWord(string word, string color = "yellow")
        {
            await executeScriptInCurrentTab(
                $"document.body.innerHTML = document.body.innerHTML.replace(/({Regex.Escape(word)})/gi, '<span style=\"background-color: {color}\">$1</span>');"
            );
        }

        public static async Task clearHighlights()
        {
            await executeScriptInCurrentTab(
                "document.body.innerHTML = document.body.innerHTML.replace(/<span style=\"background-color: yellow\">(.*?)<\\/span>/gi, '$1');"
            );
        }

        public static async Task showAlert(string message)
        {
            await executeScriptInCurrentTab($"alert('{message}');");
        }

        public static async Task scrollToTop()
        {
            await executeScriptInCurrentTab("window.scrollTo(0, 0);");
        }

        public static async Task scrollToBottom()
        {
            await executeScriptInCurrentTab("window.scrollTo(0, document.body.scrollHeight);");
        }


    }

}