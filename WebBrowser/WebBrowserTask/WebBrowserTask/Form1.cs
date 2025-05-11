using System;
using System.Linq;
using System.Windows.Forms;
using SharedData;
using SharedForms;
using System.Data.SQLite;

namespace WebBrowserTask
{
    public partial class Template : Form
    {
        public Template()
        {
            InitializeComponent();
        }

        private void Template_Load(object sender, EventArgs e)
        {
            UpdateMainUI();
        }

        public void LoadFavoriteInTab(string url)
        {
            if (tabControl1.SelectedTab != null)
            {
                foreach (Control ctrl in tabControl1.SelectedTab.Controls)
                {
                    if (ctrl is WebBrowser browser)
                    {
                        browser.Navigate(url);
                        break;
                    }
                }
            }
        }


        // BUTTON: "+" Create new tab
        private void button3_Click(object sender, EventArgs e)
        {
            AddNewTab();
        }

        // BUTTON: "-" Remove current tab
        private void button4_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabPages.Count > 1)
            {
                tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            }
        }

        private void AddNewTab()
        {
            TabPage newTab = new TabPage("New Tab");
            newTab.BackColor = System.Drawing.Color.WhiteSmoke;

            WebBrowser browser = new WebBrowser
            {
                Size = new System.Drawing.Size(1167, 567),
                Location = new System.Drawing.Point(0, 42),
                ScriptErrorsSuppressed = true
            };
            browser.Navigate("https://www.bing.com");

            browser.Tag = "browser"; // 🔥 THIS is the important fix

            TextBox addressBar = new TextBox
            {
                Size = new System.Drawing.Size(666, 20),
                Location = new System.Drawing.Point(114, 13)
            };
            addressBar.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    if (!string.IsNullOrWhiteSpace(addressBar.Text))
                        browser.Navigate(addressBar.Text);
                    else
                        MessageBox.Show("Enter a URL or search term.");
                }
            };

            Button backBtn = CreateButton("<--", 8, (s, e) =>
            {
                if (browser.CanGoBack)
                    browser.GoBack();
                else
                    MessageBox.Show("No previous page.");
            });

            Button forwardBtn = CreateButton("-->", 44, (s, e) =>
            {
                if (browser.CanGoForward)
                    browser.GoForward();
                else
                    MessageBox.Show("No next page.");
            });

            Button refreshBtn = CreateButton("↻", 78, (s, e) => browser.Refresh());

            Button searchBtn = new Button
            {
                Size = new System.Drawing.Size(50, 23),
                Location = new System.Drawing.Point(794, 11),
                Text = "Search"
            };
            searchBtn.Click += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(addressBar.Text))
                    browser.Navigate(addressBar.Text);
                else
                    MessageBox.Show("Enter a URL or search term.");
            };

            Button addTabBtn = CreateButton("+", 963, (s, e) => AddNewTab());
            Button closeTabBtn = CreateButton("-", 1001, (s, e) => button4_Click(s, e));

            Button signInBtn = new Button
            {
                Size = new System.Drawing.Size(54, 23),
                Location = new System.Drawing.Point(1046, 11),
                Text = "Sign In"
            };

            Button logOutBtn = new Button
            {
                Size = new System.Drawing.Size(56, 23),
                Location = new System.Drawing.Point(1079, 11),
                Text = "Log Out",
                Visible = false
            };

            Button starBtn = new Button
            {
                Size = new System.Drawing.Size(30, 23),
                Location = new System.Drawing.Point(854, 11),
                Text = "☆",
                Visible = false
            };
            starBtn.Click += (s, e) =>
            {
                string connectionString = @"Data Source=C:\Users\Sergey\Desktop\WebBrowser\SharedDB\users.db;Version=3;";
                string username = Session.LoggedInUserName;
                string url = browser.Url?.ToString() ?? "";


                if (string.IsNullOrWhiteSpace(url))
                {
                    MessageBox.Show("No URL to add as favorite.");
                    return;
                }

                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();

                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS favorites (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        username TEXT NOT NULL,
                        url TEXT NOT NULL
                         );";

                    using (SQLiteCommand cmd = new SQLiteCommand(createTableQuery, conn))
                        cmd.ExecuteNonQuery();

                    string checkQuery = "SELECT COUNT(*) FROM favorites WHERE username = @username AND url = @url";
                    using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@username", username);
                        checkCmd.Parameters.AddWithValue("@url", url);

                        long exists = (long)checkCmd.ExecuteScalar();
                        if (exists > 0)
                        {
                            MessageBox.Show("This page is already in your favorites.");
                            return;
                        }
                    }

                    string insertQuery = "INSERT INTO favorites (username, url) VALUES (@username, @url)";
                    using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@username", username);
                        insertCmd.Parameters.AddWithValue("@url", url);
                        insertCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Favorite added.");
            };

            Button viewFavorites = new Button
            {
                Size = new System.Drawing.Size(63, 23),
                Location = new System.Drawing.Point(890, 11),
                Text = "Favorites",
                Visible = false
            };
            viewFavorites.Click += (s, e) =>
            {
                favorites favForm = new favorites();
                favForm.Owner = this;
                favForm.ShowDialog();
            };


            // Sign In Click
            signInBtn.Click += (s, e) =>
            {
                SharedForms.Form1 signin = new SharedForms.Form1();
                signin.ShowDialog();
                if (Session.IsLoggedIn)
                {
                    UpdateAllTabsUI();
                }
            };

            // Log Out Click
            logOutBtn.Click += (s, e) =>
            {
                Session.IsLoggedIn = false;
                Session.LoggedInUserName = "";
                UpdateAllTabsUI();
            };

            // Update address bar when page loads
            browser.DocumentCompleted += (s, e) =>
            {
                if (browser.Url != null)
                    addressBar.Text = browser.Url.ToString();
            };

            // Add controls to the tab
            newTab.Controls.Add(browser);
            newTab.Controls.Add(addressBar);
            newTab.Controls.Add(backBtn);
            newTab.Controls.Add(forwardBtn);
            newTab.Controls.Add(refreshBtn);
            newTab.Controls.Add(searchBtn);
            newTab.Controls.Add(addTabBtn);
            newTab.Controls.Add(closeTabBtn);
            newTab.Controls.Add(signInBtn);
            newTab.Controls.Add(logOutBtn);
            newTab.Controls.Add(starBtn);
            newTab.Controls.Add(viewFavorites);

            tabControl1.TabPages.Add(newTab);
            tabControl1.SelectedTab = newTab;

            UpdateTabUI(newTab);
        }

        private Button CreateButton(string text, int x, EventHandler action)
        {
            Button btn = new Button
            {
                Size = new System.Drawing.Size(30, 23),
                Location = new System.Drawing.Point(x, 11),
                Text = text
            };
            btn.Click += action;
            return btn;
        }

        private void UpdateAllTabsUI()
        {
            foreach (TabPage tab in tabControl1.TabPages)
            {
                UpdateTabUI(tab);
            }
            UpdateMainUI();
        }

        private void UpdateTabUI(TabPage tab)
        {
            foreach (Control ctrl in tab.Controls)
            {
                if (ctrl is Button btn)
                {
                    if (btn.Text == "Sign In")
                        btn.Visible = !Session.IsLoggedIn;
                    else if (btn.Text == "Log Out" || btn.Text == "☆" || btn.Text == "Favorites")
                        btn.Visible = Session.IsLoggedIn;
                }
            }
        }

        private void UpdateMainUI()
        {
            if (Session.IsLoggedIn)
            {
                button10.Visible = true;
                greeting.Visible = true;
                greeting.Text = "Welcome, " + Session.LoggedInUserName + "!";
                button7.Visible = false;
                button9.Visible = true;
                button11.Visible = true;
            }
            else
            {
                button10.Visible = false;
                greeting.Visible = false;
                button7.Visible = true;
                button9.Visible = false;
                button11.Visible = false;

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label1.Hide();
            textBox2.Hide();
            button8.Hide();
            greeting.Hide();
            pictureBox1.Hide();
            webBrowser2.Navigate(textBox1.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            webBrowser2.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (webBrowser2.CanGoForward)
                webBrowser2.GoForward();
            else
                MessageBox.Show("No next page.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (webBrowser2.CanGoBack)
            {
                webBrowser2.GoBack();
                textBox1.Text = "";
            }
            else
            {
                MessageBox.Show("No previous page.");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SharedForms.Form1 signin = new SharedForms.Form1();
            signin.ShowDialog();
            if (Session.IsLoggedIn)
            {
                UpdateMainUI();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Session.IsLoggedIn = false;
            Session.LoggedInUserName = "";
            UpdateMainUI();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBox2.Text))
            {
                webBrowser2.Navigate(textBox2.Text);
                label1.Hide();
                textBox2.Hide();
                button8.Hide();
                greeting.Hide();
                pictureBox1.Hide();
            }
            else
            {
                MessageBox.Show("Please enter a search term.");
            }
        }

        // 🔥 Get URL from the active tab's browser (only the one tagged as "browser")
        private string GetCurrentTabUrl()
        {
            if (tabControl1.SelectedTab != null)
            {
                foreach (Control ctrl in tabControl1.SelectedTab.Controls)
                {
                    if (ctrl is WebBrowser browser && (string)browser.Tag == "browser")
                    {
                        return browser.Url?.ToString() ?? "";
                    }
                }
            }
            return "";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=C:\Users\Sergey\Desktop\WebBrowser\SharedDB\users.db;Version=3;";
            string username = Session.LoggedInUserName;
            string favoriteUrl = "";

            if (tabControl1.SelectedTab != null)
            {
                foreach (Control ctrl in tabControl1.SelectedTab.Controls)
                {
                    if (ctrl is WebBrowser browser && (string)browser.Tag == "browser")
                    {
                        favoriteUrl = browser.Url?.ToString() ?? "";
                        break;
                    }
                }
            }

            // If nothing found in dynamic tabs, fallback to main browser
            if (string.IsNullOrWhiteSpace(favoriteUrl) && webBrowser2.Url != null)
            {
                favoriteUrl = webBrowser2.Url.ToString();
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("You must be logged in to add favorites.");
                return;
            }

            if (string.IsNullOrWhiteSpace(favoriteUrl))
            {
                MessageBox.Show("No URL to add as favorite.");
                return;
            }

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS favorites (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                username TEXT NOT NULL,
                url TEXT NOT NULL
                );";

                using (SQLiteCommand createCmd = new SQLiteCommand(createTableQuery, conn))
                {
                    createCmd.ExecuteNonQuery();
                }

                string checkQuery = "SELECT COUNT(*) FROM favorites WHERE username = @username AND url = @url";

                using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@username", username);
                    checkCmd.Parameters.AddWithValue("@url", favoriteUrl);

                    long exists = (long)checkCmd.ExecuteScalar();
                    if (exists > 0)
                    {
                        MessageBox.Show("This page is already in your favorites.");
                        return;
                    }
                }

                string insertQuery = "INSERT INTO favorites (username, url) VALUES (@username, @url)";

                using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@username", username);
                    insertCmd.Parameters.AddWithValue("@url", favoriteUrl);
                    insertCmd.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Favorite added.");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            favorites favForm = new favorites();
            favForm.Owner = this;
            favForm.ShowDialog();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (!string.IsNullOrWhiteSpace(textBox1.Text))
                { 
                    label1.Hide();
                    textBox2.Hide();
                    button8.Hide();
                    greeting.Hide();
                    pictureBox1.Hide();
                    webBrowser2.Navigate(textBox1.Text);
                }
            else
                MessageBox.Show("Enter a URL or search term.");
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (!string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    label1.Hide();
                    textBox2.Hide();
                    button8.Hide();
                    greeting.Hide();
                    pictureBox1.Hide();
                    webBrowser2.Navigate(textBox2.Text);
                }
                else
                    MessageBox.Show("Enter a search term.");
            }
        }

        private void webBrowser2_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser2.Url != null)
            {
                textBox1.Text = webBrowser2.Url.ToString();
            }

        }
    }
}
