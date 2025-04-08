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
using SufniBrowser;
using System.Drawing.Text; 


namespace SufniVortex
{
    #region Segédosztályok

    public class ProxyEntry
    {
        public string ip { get; set; }
        public int port { get; set; }
        public List<string> protocols { get; set; }
    }

    public class ProxyResponse
    {
        public List<ProxyEntry> data { get; set; }
    }

    public class ClosableTabButton : ToolStripButton
    {
        private const int CloseButtonWidth = 15;
        public event EventHandler CloseClicked;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int offset = -5; 

            Rectangle rect = new Rectangle(this.Width - CloseButtonWidth - offset,
                                           (this.Height - CloseButtonWidth) / 2,
                                           CloseButtonWidth,
                                           CloseButtonWidth);

            using (Font font = new Font("Segoe UI", 10, FontStyle.Bold))
            using (StringFormat format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            using (SolidBrush brush = new SolidBrush(Color.White)) 
            {
                e.Graphics.DrawString("x", font, brush, rect, format);
            }
        }



        protected override void OnMouseDown(MouseEventArgs e)
        {
           
            if (e.X >= this.Width - CloseButtonWidth)
            {
                CloseClicked?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                base.OnMouseDown(e);
            }
        }
    }

    #endregion

    public class ComplexBrowserForm : Form
    {
        #region HTML & URL

      
        private static readonly string HomePageHtml = @"
<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
  <title>Sufni Vortex - Cyberpunk HITLER Search</title>
  <link href=""https://fonts.googleapis.com/css2?family=Orbitron:wght@400;700&display=swap"" rel=""stylesheet"">
  <script src=""https://cdn.jsdelivr.net/npm/particles.js@2.0.0/particles.min.js""></script>
  <style>
 @keyframes gradient {
 0% { background-position: 0% 50%; }
 50% { background-position: 100% 50%; }
 100% { background-position: 0% 50%; }
 }
 @keyframes glow {
 0%, 100% { text-shadow: 0 0 10px #f00, 0 0 20px #f00; }
 50% { text-shadow: 0 0 20px #f00, 0 0 40px #f00; }
 }
 body {
 margin: 0;
 padding: 0;
 background: linear-gradient(45deg, #800000, #000428, #004e92, #001f3f);
 background-size: 400% 400%;
 animation: gradient 15s ease infinite;
 font-family: 'Orbitron', sans-serif;
 color: #f00;
 overflow: hidden;
 height: 100vh;
 display: flex;
 justify-content: center;
 align-items: center;
 position: relative;
 }
 #particles-js {
 position: absolute;
 width: 100%;
 height: 100%;
 top: 0;
 left: 0;
 z-index: -1;
 }
 .container {
 text-align: center;
 padding: 40px;
 border: 3px solid rgba(255, 0, 0, 0.7);
 border-radius: 15px;
 background: rgba(0, 0, 0, 0.8);
 box-shadow: 0 0 30px rgba(255, 0, 0, 0.7);
 max-width: 600px;
 }
 h1 {
 font-size: 64px;
 margin: 0 0 20px;
 animation: glow 2s ease-in-out infinite;
 }
 p {
 font-size: 22px;
 margin-bottom: 30px;
 }
 form {
 display: flex;
 justify-content: center;
 }
 input[type=""text""] {
 width: 80%;
 padding: 12px 15px;
 font-size: 18px;
 border: none;
 border-radius: 5px 0 0 5px;
 outline: none;
 background: rgba(255, 255, 255, 0.2);
 color: #f00;
 transition: background 0.3s ease;
 }
 input[type=""text""]:focus {
 background: rgba(255, 255, 255, 0.3);
 }
 input[type=""submit""], input[type=""button""] {
 padding: 12px 20px;
 font-size: 18px;
 border: none;
 border-radius: 0 5px 5px 0;
 background: #f00;
 color: #000;
 cursor: pointer;
 transition: background 0.3s ease, transform 0.2s ease;
 margin-left: 5px;
 }
 input[type=""submit""]:hover, input[type=""button""]:hover {
 background: #800000;
 transform: scale(1.05);
 }
 </style>
</head>
<body>
  <div id=""particles-js""></div>
  <div class=""container"">
    <h1>Sufni Vortex</h1>
    <p>Experience a cyberpunk HITLER revolution in browsing</p>
    <form action=""https://www.duckduckgo.com/"" method=""get"" target=""_self"">
      <input type=""text"" name=""q"" placeholder=""Search the web..."">
      <input type=""submit"" value=""Search"">
    </form>
  </div>
  <script>
    particlesJS('particles-js', {
      particles: {
        number: { value: 100, density: { enable: true, value_area: 800 } },
        color: { value: '#0ff' },
        shape: { type: 'circle' },
        opacity: { value: 0.5, random: true, anim: { enable: true, speed: 1, opacity_min: 0.1, sync: false } },
        size: { value: 4, random: true },
        line_linked: { enable: true, distance: 150, color: '#0ff', opacity: 0.4, width: 1 },
        move: { enable: true, speed: 2, direction: 'none', random: false, out_mode: 'out', bounce: false }
      },
      interactivity: { detect_on: 'canvas', events: { onhover: { enable: true, mode: 'repulse' }, onclick: { enable: true, mode: 'push' }, resize: true } },
      retina_detect: true
    });
  </script>
</body>
</html>
";
      
        private static readonly string HomePageUrl = "data:text/html," + Uri.EscapeDataString(HomePageHtml);

        #endregion

        #region UI Elemei

       
        private MenuStrip menuStrip;
        private ToolStripMenuItem fileMenu;
        private ToolStripMenuItem newTabMenuItem;
        private ToolStripMenuItem closeTabMenuItem;
        private ToolStripMenuItem exitMenuItem;
        private ToolStripMenuItem bookmarksMenu;
        private ToolStripMenuItem addBookmarkMenuItem;
        private ToolStripMenuItem showBookmarksMenuItem;
        private ToolStripMenuItem historyMenu;
        private ToolStripMenuItem showHistoryMenuItem;
        private ToolStripMenuItem saveCookiesMenuItem;
        private ToolStripMenuItem passwordManagerMenuItem;
        private ToolStripMenuItem securityMenu;
        private ToolStripMenuItem toggleJavaScriptMenuItem;

     
        private ToolStrip toolStrip;
        private ToolStripButton backButton;
        private ToolStripButton forwardButton;
        private ToolStripButton refreshButton;
        private ToolStripButton homeButton;
        private ToolStripButton stopButton;
        private ToolStripTextBox urlTextBox;
        private ToolStripButton goButton;

     
        private ToolStrip tabToolStrip;

      
        private TabControl tabControl;

     
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;

        #endregion

        #region Egyéb tagváltozók

       
        private List<string> bookmarksList = new List<string>();
        private List<string> historyList = new List<string>();

       
        private bool blockJavaScript = false;

        
        private static CoreWebView2Environment torEnvironment;

    
        private HashSet<TabPage> pinnedTabs = new HashSet<TabPage>();
        private HashSet<TabPage> mutedTabs = new HashSet<TabPage>();

        public static ComplexBrowserForm Instance { get; private set; }


        #endregion

        #region Konstruktor és Inicializáció

        public ComplexBrowserForm()
        {
            this.Text = "Sufni Vortex";
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.Black;
            try { this.Icon = new Icon("sufni.ico"); } catch { }

            bookmarksList = DataManager.LoadBookmarks();
            historyList = DataManager.LoadHistory();
            System.Timers.Timer cookieSaveTimer = new System.Timers.Timer(500);
            cookieSaveTimer.Elapsed += async (s, e) => await AutoSaveCookies();
            cookieSaveTimer.AutoReset = true;
            cookieSaveTimer.Start();


            InitializeComponentsAsync();

            CreateNewTab(HomePageUrl);
        }

        private async Task InitializeComponentsAsync()
        {

            menuStrip = new MenuStrip
            {
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Cyan,
                Dock = DockStyle.Top
            };

          

            Instance = this;

            fileMenu = new ToolStripMenuItem("File");
            passwordManagerMenuItem = new ToolStripMenuItem("Password Manager", null, PasswordManagerMenuItem_Click);
            saveCookiesMenuItem = new ToolStripMenuItem("Save Cookies", null, SaveCookiesMenuItem_Click);
            newTabMenuItem = new ToolStripMenuItem("New Tab", null, NewTabMenuItem_Click);
            closeTabMenuItem = new ToolStripMenuItem("Close Tab", null, CloseTabMenuItem_Click);
            exitMenuItem = new ToolStripMenuItem("Exit", null, ExitMenuItem_Click);
            fileMenu.DropDownItems.Add(passwordManagerMenuItem);
            fileMenu.DropDownItems.Add(saveCookiesMenuItem);
            fileMenu.DropDownItems.Add(newTabMenuItem);
            fileMenu.DropDownItems.Add(closeTabMenuItem);
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            fileMenu.DropDownItems.Add(exitMenuItem);

            bookmarksMenu = new ToolStripMenuItem("Bookmarks");
            addBookmarkMenuItem = new ToolStripMenuItem("Add Bookmark", null, AddBookmarkMenuItem_Click);
            showBookmarksMenuItem = new ToolStripMenuItem("View Bookmarks", null, ShowBookmarksMenuItem_Click);
            bookmarksMenu.DropDownItems.Add(addBookmarkMenuItem);
            bookmarksMenu.DropDownItems.Add(showBookmarksMenuItem);

            historyMenu = new ToolStripMenuItem("History");
            showHistoryMenuItem = new ToolStripMenuItem("View History", null, ShowHistoryMenuItem_Click);
            historyMenu.DropDownItems.Add(showHistoryMenuItem);

            securityMenu = new ToolStripMenuItem("Security");
            toggleJavaScriptMenuItem = new ToolStripMenuItem("Block JavaScript")
            {
                CheckOnClick = true
            };
            toggleJavaScriptMenuItem.Click += ToggleJavaScriptMenuItem_Click;
            securityMenu.DropDownItems.Add(toggleJavaScriptMenuItem);

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(bookmarksMenu);
            menuStrip.Items.Add(historyMenu);
            menuStrip.Items.Add(securityMenu);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);


            toolStrip = new ToolStrip
            {
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Cyan,
                Dock = DockStyle.Top
            };
            backButton = new ToolStripButton("◄", null, BackButton_Click) { ToolTipText = "Back" };
            forwardButton = new ToolStripButton("►", null, ForwardButton_Click) { ToolTipText = "Forward" };
            refreshButton = new ToolStripButton("↻", null, RefreshButton_Click) { ToolTipText = "Refresh" };
            homeButton = new ToolStripButton("🏠", null, HomeButton_Click) { ToolTipText = "Home" };
            stopButton = new ToolStripButton("■", null, StopButton_Click) { ToolTipText = "Stop" };
            urlTextBox = new ToolStripTextBox() { Width = 400, ToolTipText = "URL or Search" };
            goButton = new ToolStripButton("Search", null, GoButton_Click);
            toolStrip.Items.Add(backButton);
            toolStrip.Items.Add(forwardButton);
            toolStrip.Items.Add(refreshButton);
            toolStrip.Items.Add(homeButton);
            toolStrip.Items.Add(stopButton);
            toolStrip.Items.Add(urlTextBox);
            toolStrip.Items.Add(goButton);
            this.Controls.Add(toolStrip);


            tabToolStrip = new ToolStrip
            {
                BackColor = Color.FromArgb(50, 50, 50),
                ForeColor = Color.Cyan,
                Dock = DockStyle.Top
            };
            this.Controls.Add(tabToolStrip);
   
            statusStrip = new StatusStrip
            {
                BackColor = Color.FromArgb(30, 30, 30),
                ForeColor = Color.Cyan,
                Dock = DockStyle.Bottom
            };
            statusLabel = new ToolStripStatusLabel("Ready");
            statusStrip.Items.Add(statusLabel);
            this.Controls.Add(statusStrip);

            tabControl = new TabControl
            {
                BackColor = Color.Black,
                ForeColor = Color.Cyan
            };

            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.ItemSize = new Size(0, 1);
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            
            int topOffset = menuStrip.Height + toolStrip.Height + tabToolStrip.Height;

            int bottomOffset = statusStrip.Height;

            tabControl.Location = new Point(0, topOffset);
            tabControl.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - topOffset - bottomOffset);
          
            tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.Controls.Add(tabControl);

            this.Resize += (s, e) =>
            {
                int newTopOffset = menuStrip.Height + toolStrip.Height + tabToolStrip.Height;
                int newBottomOffset = statusStrip.Height;
                tabControl.Location = new Point(0, newTopOffset);
                tabControl.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - newTopOffset - newBottomOffset);
            };





            Extensions.BrowserExtensionLoader.LoadExtensions(this);
        }

        private async Task AutoSaveCookies()
        {
            if (tabControl.InvokeRequired)
            {
                tabControl.Invoke(new Action(async () => await AutoSaveCookies()));
                return;
            }

            if (tabControl.TabPages.Count == 0) return;

            List<CookieInfo> allCookies = new List<CookieInfo>();

            foreach (TabPage tab in tabControl.TabPages)
            {
                WebView2 webView = GetWebViewFromTab(tab);
                if (webView == null || webView.CoreWebView2 == null)
                {
                    Console.WriteLine("WebView2 példány nincs inicializálva ezen a lapon.");
                    continue;
                }

                try
                {
                    var cookieManager = webView.CoreWebView2.CookieManager;
                    var cookies = await cookieManager.GetCookiesAsync(webView.Source.ToString());

                    foreach (var cookie in cookies)
                    {
                        allCookies.Add(new CookieInfo
                        {
                            Name = cookie.Name,
                            Value = cookie.Value,
                            Domain = cookie.Domain,
                            Path = cookie.Path,
                            Expires = cookie.Expires == DateTime.MinValue ? (DateTime?)null : cookie.Expires
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Hiba a sütik mentésekor: {ex.Message}");
                }
            }

            if (allCookies.Count > 0)
            {
                Console.WriteLine($"Összesen {allCookies.Count} süti mentése...");
                DataManager.SaveCookies(allCookies);
            }
        }





        #endregion

        #region Lapok Kezelése és WebView2


        public async void CreateNewTab(string url)
        {

            

            

            TabPage tabPage = new TabPage("New Tab")
            {
                BackColor = Color.Black
            };
            WebView2 webView = new WebView2
            {
                BackColor = Color.Black,
                Dock = DockStyle.Fill  
            };
            tabPage.Controls.Add(webView);
            tabControl.TabPages.Add(tabPage);
            tabControl.SelectedTab = tabPage;

            

            try
            {

                await webView.EnsureCoreWebView2Async(null);

               
                webView.CoreWebView2.Settings.IsPasswordAutosaveEnabled = true;
                webView.CoreWebView2.Settings.IsGeneralAutofillEnabled = true;

                webView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = true;
                webView.CoreWebView2.Settings.IsSwipeNavigationEnabled = true;

                LoadCookiesIntoBrowser(webView);


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing WebView2: " + ex.Message);
                return;
            }

            webView.CoreWebView2.DocumentTitleChanged += (sender, args) =>
            {
                tabPage.Text = webView.CoreWebView2.DocumentTitle;
                UpdateTabToolStrip();
            };

            webView.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
            webView.CoreWebView2.WebResourceRequested += (sender, args) =>
            {
                if (blockJavaScript && args.ResourceContext == CoreWebView2WebResourceContext.Script)
                {
                    args.Response = webView.CoreWebView2.Environment.CreateWebResourceResponse(
                        null, 403, "Blocked", "Content-Type: text/javascript");
                    return;
                }

                if (AdBlocker.IsAdUrl(args.Request.Uri))
                {
                    args.Response = webView.CoreWebView2.Environment.CreateWebResourceResponse(
                        null, 403, "Blocked", "Content-Type: text/html");
                    return;
                }
            };

            

            webView.CoreWebView2.NavigationCompleted += async (sender, args) =>
            {
                urlTextBox.Text = webView.Source.ToString();
                statusLabel.Text = args.IsSuccess ? "Loaded" : "Error occurred";

                if (!string.IsNullOrEmpty(webView.Source.ToString()))
                {
                    historyList.Add(webView.Source.ToString());
                    DataManager.SaveHistory(historyList);
                }


            };

            try
            {
                webView.CoreWebView2.Navigate(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Navigation error: {ex.Message}");
            }

            UpdateTabToolStrip();
           
        }

        public async void LoadCookiesIntoBrowser(WebView2 webView)
        {
            if (webView == null || webView.CoreWebView2 == null)
                return;

            var cookieManager = webView.CoreWebView2.CookieManager;
            var cookies = DataManager.LoadCookies();
            foreach (var cookie in cookies)
            {
                var newCookie = webView.CoreWebView2.CookieManager.CreateCookie(cookie.Name, cookie.Value, cookie.Domain, cookie.Path);
                if (cookie.Expires.HasValue)
                    newCookie.Expires = cookie.Expires.Value;
                     cookieManager.AddOrUpdateCookie(newCookie);
            }
        }

        public async void ClearCookies()
        {
            var webView = GetCurrentWebView();
            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.CookieManager.DeleteAllCookies();
                DataManager.ClearCookies();
                MessageBox.Show("All cookies have been deleted.");
            }
        }





        public WebView2 GetBrowserControl()
        {
            if (tabControl.SelectedTab != null && tabControl.SelectedTab.Controls.Count > 0)
            {
                return tabControl.SelectedTab.Controls[0] as WebView2;
            }
            return null;
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            int topOffset = menuStrip.Height + toolStrip.Height + tabToolStrip.Height;
            int bottomOffset = statusStrip.Height;
            tabControl.Location = new Point(0, topOffset);
            tabControl.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - topOffset - bottomOffset);
        }



        











        public void ResizeWebView(TabPage tabPage, WebView2 webView)
        {
            if (tabPage == null || webView == null) return;

          
            int menuHeight = menuStrip.Visible ? menuStrip.Height : 0;
            int toolStripHeight = toolStrip.Visible ? toolStrip.Height : 0;
            int tabStripHeight = tabToolStrip.Visible ? tabToolStrip.Height : 0;
            int statusStripHeight = statusStrip.Visible ? statusStrip.Height : 0;

            int reservedHeight = menuHeight + toolStripHeight + tabStripHeight + statusStripHeight;

            webView.SetBounds(0, 0, tabPage.ClientSize.Width, tabPage.ClientSize.Height - reservedHeight);
        }




        
        private void UpdateTabToolStrip()
        {
            tabToolStrip.Items.Clear();
            foreach (TabPage page in tabControl.TabPages)
            {
                ClosableTabButton btn = new ClosableTabButton();
                btn.Text = page.Text;
                btn.Tag = page;
                btn.Click += (s, e) => { tabControl.SelectedTab = page; };
                btn.CloseClicked += (s, e) => { CloseTab(page); };
                btn.MouseUp += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        ShowTabContextMenu(btn, e);
                    }
                };

                if (pinnedTabs.Contains(page))
                {
                    btn.Text = "[PIN] " + btn.Text;
                }
                if (mutedTabs.Contains(page))
                {
                    btn.Text += " (Muted)";
                }
                if (tabControl.SelectedTab == page)
                {
                    btn.Font = new Font(btn.Font, FontStyle.Bold);
                }
                tabToolStrip.Items.Add(btn);
            }
            ToolStripButton newTabButton = new ToolStripButton();
            newTabButton.Text = "+";
            newTabButton.Click += (s, e) => { CreateNewTab(HomePageUrl); };
            tabToolStrip.Items.Add(newTabButton);
        }

       
        public void ShowTabContextMenu(ClosableTabButton btn, MouseEventArgs e)
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem closeOthers = new ToolStripMenuItem("Close Others");
            closeOthers.Click += (s, ev) => { CloseOtherTabs(btn.Tag as TabPage); };

            ToolStripMenuItem closeAll = new ToolStripMenuItem("Close All");
            closeAll.Click += (s, ev) => { CloseAllTabs(); };

            ToolStripMenuItem close = new ToolStripMenuItem("Close");
            close.Click += (s, ev) => { CloseTab(btn.Tag as TabPage); };

            ToolStripMenuItem newTab = new ToolStripMenuItem("New Tab");
            newTab.Click += (s, ev) => { CreateNewTab(HomePageUrl); };

            ToolStripMenuItem refreshAll = new ToolStripMenuItem("Refresh All Tabs");
            refreshAll.Click += (s, ev) => { RefreshAllTabs(); };

            ToolStripMenuItem refresh = new ToolStripMenuItem("Refresh");
            refresh.Click += (s, ev) => { RefreshTab(btn.Tag as TabPage); };

            ToolStripMenuItem duplicate = new ToolStripMenuItem("Duplicate Tab");
            duplicate.Click += (s, ev) => { DuplicateTab(btn.Tag as TabPage); };

            ToolStripMenuItem pin = new ToolStripMenuItem("Pin Tab");
            pin.Click += (s, ev) => { PinTab(btn.Tag as TabPage); };

            ToolStripMenuItem mute = new ToolStripMenuItem("Mute Tab");
            mute.Click += (s, ev) => { MuteTab(btn.Tag as TabPage); };

            ToolStripMenuItem muteOthers = new ToolStripMenuItem("Mute Other Tabs");
            muteOthers.Click += (s, ev) => { MuteOtherTabs(btn.Tag as TabPage); };

            ToolStripMenuItem savePage = new ToolStripMenuItem("Save Page");
            savePage.Click += (s, ev) => { SavePage(btn.Tag as TabPage); };

            menu.Items.AddRange(new ToolStripItem[] { closeOthers, closeAll, close, newTab, refreshAll, refresh, duplicate, pin, mute, muteOthers, savePage });
        }


        private WebView2 GetWebViewFromTab(TabPage tab)
        {
            if (tab != null && tab.Controls.Count > 0)
            {
                return tab.Controls[0] as WebView2;
            }
            return null;
        }
        


        #endregion

        #region Navigációs Gombok Eseménykezelői

        private void GoButton_Click(object sender, EventArgs e)
        {
            var webView = GetCurrentWebView();
            if (webView != null && webView.CoreWebView2 != null)
            {
                string input = urlTextBox.Text.Trim();
                if (!input.StartsWith("http://") && !input.StartsWith("https://"))
                {
                    if (input.Contains(" "))
                    {
                        input = "https://www.duckduckgo.com/" + Uri.EscapeDataString(input);
                    }
                    else if (input.Contains("."))
                    {
                        input = "http://" + input;
                    }
                    else
                    {
                        input = "https://www.duckduckgo.com/" + Uri.EscapeDataString(input);
                    }
                }
                try
                {
                    webView.CoreWebView2.Navigate(input);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Navigation error: {ex.Message}");
                }
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            var webView = GetCurrentWebView();
            if (webView != null && webView.CoreWebView2 != null && webView.CoreWebView2.CanGoBack)
            {
                webView.CoreWebView2.GoBack();
            }
        }

        private void ForwardButton_Click(object sender, EventArgs e)
        {
            var webView = GetCurrentWebView();
            if (webView != null && webView.CoreWebView2 != null && webView.CoreWebView2.CanGoForward)
            {
                webView.CoreWebView2.GoForward();
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            GetCurrentWebView()?.CoreWebView2.Reload();
        }

        private void HomeButton_Click(object sender, EventArgs e)
        {
            var webView = GetCurrentWebView();
            if (webView != null && webView.CoreWebView2 != null)
            {
                webView.CoreWebView2.Navigate(HomePageUrl);
            }
        }

       

        private void StopButton_Click(object sender, EventArgs e)
        {
            GetCurrentWebView()?.CoreWebView2.Stop();
        }

        
        public WebView2 GetCurrentWebView()
        {
            if (tabControl.SelectedTab == null)
            {
                MessageBox.Show("SelectedTab null.");
                return null;
            }
            if (tabControl.SelectedTab.Controls.Count == 0)
            {
                MessageBox.Show("A kiválasztott lap nem tartalmaz semmilyen kontrollt.");
                return null;
            }
            var webView = tabControl.SelectedTab.Controls[0] as WebView2;
            if (webView == null)
            {
                MessageBox.Show("A kiválasztott lapon található vezérlő nem WebView2 típusú.");
            }
            return webView;
        }




        #endregion

        #region Menü Események

        private void NewTabMenuItem_Click(object sender, EventArgs e)
        {
            CreateNewTab(HomePageUrl);
        }

        private void CloseTabMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl.TabPages.Count > 0)
            {
                tabControl.TabPages.Remove(tabControl.SelectedTab);
                UpdateTabToolStrip();
            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddBookmarkMenuItem_Click(object sender, EventArgs e)
        {
            var webView = GetCurrentWebView();
            if (webView != null && webView.CoreWebView2 != null)
            {
                string currentUrl = webView.Source.ToString();
                if (!string.IsNullOrEmpty(currentUrl) && !bookmarksList.Contains(currentUrl))
                {
                    bookmarksList.Add(currentUrl);
                    DataManager.SaveBookmarks(bookmarksList);
                    MessageBox.Show("Bookmark added: " + currentUrl);
                }
            }
        }

        private void ShowBookmarksMenuItem_Click(object sender, EventArgs e)
        {
            using (var bf = new BookmarksForm(bookmarksList))
            {
                if (bf.ShowDialog() == DialogResult.OK)
                {
                    string selectedUrl = bf.SelectedUrl;
                    if (!string.IsNullOrEmpty(selectedUrl))
                    {
                        var webView = GetCurrentWebView();
                        if (webView != null && webView.CoreWebView2 != null)
                        {
                            webView.CoreWebView2.Navigate(selectedUrl);
                        }
                    }
                }
            }
        }

        private void ShowHistoryMenuItem_Click(object sender, EventArgs e)
        {
            using (var hf = new HistoryForm(historyList))
            {
                if (hf.ShowDialog() == DialogResult.OK)
                {
                    string selectedUrl = hf.SelectedUrl;
                    if (!string.IsNullOrEmpty(selectedUrl))
                    {
                        var webView = GetCurrentWebView();
                        if (webView != null && webView.CoreWebView2 != null)
                        {
                            webView.CoreWebView2.Navigate(selectedUrl);
                        }
                    }
                }
            }
        }

        private void ToggleJavaScriptMenuItem_Click(object sender, EventArgs e)
        {
            blockJavaScript = toggleJavaScriptMenuItem.Checked;
        }

        #endregion

        #region Extra Funkciók

        private async void SaveCookiesMenuItem_Click(object sender, EventArgs e)
        {
            var webView = GetCurrentWebView();
            if (webView != null && webView.CoreWebView2 != null)
            {
                var cookieManager = webView.CoreWebView2.CookieManager;
                var cookies = await cookieManager.GetCookiesAsync(webView.Source.ToString());
                List<CookieInfo> cookieInfos = new List<CookieInfo>();
                foreach (var cookie in cookies)
                {
                    cookieInfos.Add(new CookieInfo
                    {
                        Name = cookie.Name,
                        Value = cookie.Value,
                        Domain = cookie.Domain,
                        Path = cookie.Path,
                        Expires = cookie.Expires == DateTime.MinValue ? (DateTime?)null : cookie.Expires
                    });
                }
                DataManager.SaveCookies(cookieInfos);
                MessageBox.Show("Cookies saved.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void PasswordManagerMenuItem_Click(object sender, EventArgs e)
        {
            PasswordManagerForm pmf = new PasswordManagerForm();
            pmf.ShowDialog();
        }

        #endregion
        // TODO: something with tor
        #region Tor Kapcsolat


       




        #endregion

        #region Új Funkciók: Kontextusmenü Akciók

        public void CloseTab(TabPage tab)
        {
            if (tab != null)
            {
                tabControl.TabPages.Remove(tab);
                UpdateTabToolStrip();
            }
        }

        private void CloseOtherTabs(TabPage tab)
        {
            List<TabPage> tabsToClose = new List<TabPage>();
            foreach (TabPage t in tabControl.TabPages)
            {
                if (t != tab)
                    tabsToClose.Add(t);
            }
            foreach (TabPage t in tabsToClose)
                tabControl.TabPages.Remove(t);
            UpdateTabToolStrip();
        }

        private void CloseAllTabs()
        {
            tabControl.TabPages.Clear();
            UpdateTabToolStrip();
        }

        private void RefreshTab(TabPage tab)
        {
            var webView = GetWebViewFromTab(tab);
            webView?.CoreWebView2.Reload();
        }

        private void RefreshAllTabs()
        {
            foreach (TabPage tab in tabControl.TabPages)
            {
                var webView = GetWebViewFromTab(tab);
                webView?.CoreWebView2.Reload();
            }
        }

        private void DuplicateTab(TabPage tab)
        {
            var webView = GetWebViewFromTab(tab);
            if (webView != null)
            {
                string url = webView.Source.ToString();
                CreateNewTab(url);
            }
        }

        private void PinTab(TabPage tab)
        {
            if (tab != null)
            {
                if (pinnedTabs.Contains(tab))
                    pinnedTabs.Remove(tab);
                else
                    pinnedTabs.Add(tab);
                UpdateTabToolStrip();
            }
        }

        private void MuteTab(TabPage tab)
        {
            var webView = GetWebViewFromTab(tab);
            if (webView != null)
            {
                if (mutedTabs.Contains(tab))
                {
                    mutedTabs.Remove(tab);
                    webView.CoreWebView2.ExecuteScriptAsync("document.querySelectorAll('video, audio').forEach(e => e.muted = false);");
                }
                else
                {
                    mutedTabs.Add(tab);
                    webView.CoreWebView2.ExecuteScriptAsync("document.querySelectorAll('video, audio').forEach(e => e.muted = true);");
                }
                UpdateTabToolStrip();
            }
        }

        private void MuteOtherTabs(TabPage tab)
        {
            foreach (TabPage t in tabControl.TabPages)
            {
                if (t != tab)
                {
                    var webView = GetWebViewFromTab(t);
                    if (webView != null)
                    {
                        mutedTabs.Add(t);
                        webView.CoreWebView2.ExecuteScriptAsync("document.querySelectorAll('video, audio').forEach(e => e.muted = true);");
                    }
                }
            }
            UpdateTabToolStrip();
        }

        public void ExecuteScriptInCurrentTab(string script)
        {
            if (tabControl.SelectedTab != null && tabControl.SelectedTab.Controls.Count > 0)
            {
                WebView2 webView = tabControl.SelectedTab.Controls[0] as WebView2;
                if (webView != null && webView.CoreWebView2 != null)
                {
                    webView.CoreWebView2.ExecuteScriptAsync(script);
                }
            }
        }


        private async void SavePage(TabPage tab)
        {
            var webView = GetWebViewFromTab(tab);
            if (webView != null && webView.CoreWebView2 != null)
            {
                string html = await webView.CoreWebView2.ExecuteScriptAsync("document.documentElement.outerHTML");
                html = System.Text.Json.JsonSerializer.Deserialize<string>(html);
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "HTML Files|*.html|All Files|*.*";
                sfd.FileName = "page.html";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(sfd.FileName, html);
                }
            }
        }

        #endregion

        #region TabControl Eseménykezelés

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var webView = GetCurrentWebView();
            if (webView != null && webView.Source != null)
            {
                urlTextBox.Text = webView.Source.ToString();
            }
            UpdateTabToolStrip();
        }

        #endregion
    }
}
