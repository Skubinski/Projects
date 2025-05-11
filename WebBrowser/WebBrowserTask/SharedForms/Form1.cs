using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using SharedData;

namespace SharedForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void button2_Click(object sender, EventArgs e)
        {

            SignUpForm signUpForm = new SignUpForm();
            signUpForm.ShowDialog();
            this.Close();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string email = emailtb.Text.Trim().ToLower();
            string password = passwordtb.Text.Trim();
            string retrievedUsername = "";  // This will hold the username if found

            string connectionString = @"Data Source=C:\Users\Sergey\Desktop\WebBrowser\SharedDB\users.db;Version=3;";

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT username FROM users WHERE email = @email AND password = @password";

                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@password", password);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        retrievedUsername = result.ToString();  // Got the username

                        MessageBox.Show("Login successful!");

                        // Example: set session values
                        Session.IsLoggedIn = true;
                        Session.LoggedInUserName = retrievedUsername;



                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Wrong email or password. Try again.");
                    }
                }
            }
        }

        private void passwordtb_TextChanged(object sender, EventArgs e)
        {
            if (passwordtb.Text.Length > 0)
            {
                passwordtb.UseSystemPasswordChar = true;
            }
            else
            {
                passwordtb.UseSystemPasswordChar = false;
            }
        }

        private void passwordtb_TextChanged_1(object sender, EventArgs e)
        {
            if (passwordtb.Text.Length > 0)
            {
                passwordtb.UseSystemPasswordChar = true;
            }
            else
            {
                passwordtb.UseSystemPasswordChar = false;
            }
        }

        private void passwordtb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text == "show")
            {
                passwordtb.UseSystemPasswordChar = false;
                button3.Text = "hide";
            }
            else
            {
                passwordtb.UseSystemPasswordChar = true;
                button3.Text = "show";
            }
        }
    }
}
