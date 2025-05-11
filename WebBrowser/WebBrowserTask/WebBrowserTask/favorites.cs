using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedData;
using System.Data.SQLite;


namespace WebBrowserTask
{
    public partial class favorites : Form
    {
        public favorites()
        {
            InitializeComponent();
        }


        private void LoadFavorites()
        {
            if (!Session.IsLoggedIn)
            {
                MessageBox.Show("You must be logged in to view favorites.");
                this.Close();
                return;
            }

            string username = Session.LoggedInUserName;
            string connectionString = @"Data Source=C:\Users\Sergey\Desktop\WebBrowser\SharedDB\users.db;Version=3;";
            listBox1.Items.Clear();

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT url FROM favorites WHERE username = @username";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listBox1.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string url = listBox1.SelectedItem.ToString();
                ((Template)Owner)?.LoadFavoriteInTab(url); // 🔥 send to main form
                this.Close();
            }
        }

        private void favorites_Load(object sender, EventArgs e)
        {
            LoadFavorites();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Please select a favorite to delete.");
                return;
            }

            string urlToDelete = listBox1.SelectedItem.ToString();
            string username = Session.LoggedInUserName;

            string connectionString = @"Data Source=C:\Users\Sergey\Desktop\WebBrowser\SharedDB\users.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string deleteQuery = "DELETE FROM favorites WHERE username = @username AND url = @url";

                using (SQLiteCommand cmd = new SQLiteCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@url", urlToDelete);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Favorite removed.");
                        listBox1.Items.Remove(urlToDelete); // update UI
                    }
                    else
                    {
                        MessageBox.Show("Could not find favorite to remove.");
                    }
                }
            }
        }
    }
}
