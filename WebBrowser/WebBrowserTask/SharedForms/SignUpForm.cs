using System;
using System.Data;
using System.Data.SQLite;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SharedForms
{
    public partial class SignUpForm : Form
    {
        private string connectionString = @"Data Source=C:\Users\Sergey\Desktop\WebBrowser\SharedDB\users.db;Version=3;";

        public SignUpForm()
        {
            InitializeComponent();
            CreateUsersTable();
        }


        private void CreateUsersTable()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS users (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        email TEXT NOT NULL UNIQUE,
                        username TEXT NOT NULL UNIQUE,
                        password TEXT NOT NULL
                    );";

                using (var cmd = new SQLiteCommand(createTableQuery, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get input values
            string email = emailtb.Text.ToLower();
            string username = usernametb.Text;
            string password = passwordtb.Text;
            string captcha = captchatb.Text;

            // Validate the captcha
            if (captcha != "57")  // Example captcha check (25 + 32)
            {
                MessageBox.Show("Captcha verification failed!");
                return;
            }

            // Validate email format
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Invalid email format.");
                return;
            }

            // Validate password (1 Upper, 1 digit, Min 6 chars)
            if (!IsValidPassword(password))
            {
                MessageBox.Show("Password must contain at least 1 uppercase letter, 1 digit, and be at least 6 characters long.");
                return;
            }

            // Validate username (First letter uppercase, Min 6 chars)
            if (!IsValidUsername(username))
            {
                MessageBox.Show("Username must start with an uppercase letter and be at least 6 characters long.");
                return;
            }

            // Check if the email or username already exists in the SQLite database
            if (EmailExists(email) || UsernameExists(username))
            {
                MessageBox.Show("Email or Username already exists.");
                return;
            }

            // Insert data into SQLite database
            if (InsertUserData(email, username, password))
            {
                MessageBox.Show("Sign Up Successful!");
                emailtb.Text = "";
                usernametb.Text = "";
                passwordtb.Text = "";
                captchatb.Text = "";

                this.Close();



            }
            else
            {
                MessageBox.Show("Error signing up. Please try again.");
            }
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d).{6,}$");
        }

        private bool IsValidUsername(string username)
        {
            return Regex.IsMatch(username, @"^[A-Z].{5,}$");
        }

        private bool EmailExists(string email)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM users WHERE email = @email", connection))
                {
                    cmd.Parameters.AddWithValue("email", email);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private bool UsernameExists(string username)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM users WHERE username = @username", connection))
                {
                    cmd.Parameters.AddWithValue("username", username);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private bool InsertUserData(string email, string username, string password)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new SQLiteCommand("INSERT INTO users (email, username, password) VALUES (@email, @username, @password)", connection))
                {
                    cmd.Parameters.AddWithValue("email", email);
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("password", password);  // You should hash the password before storing it
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        private void passwordtb_TextChanged(object sender, EventArgs e)
        {
            if (passwordtb.Text != "")
            {
                passwordtb.UseSystemPasswordChar = true;
            }
            else
            {
                passwordtb.UseSystemPasswordChar = false;
            }
        }

        private void SignUp_Load(object sender, EventArgs e)
        {

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

        private void captchatb_KeyDown(object sender, KeyEventArgs e)
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