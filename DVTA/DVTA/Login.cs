using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DBAccess;


namespace DVTA
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            String username = txtLgnUsername.Text.Trim();
            String password = txtLgnPass.Text.Trim();
            if (username == string.Empty || password == string.Empty)
            {
                MessageBox.Show("Please enter all the fields!");
                return;
            }

            DBAccessClass db = new DBAccessClass();

            db.openConnection();

           SqlDataReader data = db.checkLogin(username,password);
           if (data.HasRows)
           {
               String user;
               String pass;
               String email;
               int isadmin=0;

              /* Microsoft.Win32.RegistryKey myRegKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("Details");
               String uname = (String)myRegKey.GetValue("username", null);
               String pass = (String)myRegKey.GetValue("password", null);*/

               while (data.Read())
               {
                   user = data.GetString(1);
                   pass = data.GetString(2);
                   email = data.GetString(3);
                   isadmin = (int) data.GetValue(4);

                   if (user != "admin")
                   {
                       Microsoft.Win32.RegistryKey key;
                       key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey("dvta");
                       key.SetValue("username", user);
                       key.SetValue("password", pass);
                       key.SetValue("email", email);
                       key.SetValue("isLoggedIn", "true");
                       key.Close();
                   }
               }
               txtLgnUsername.Text = "";
               txtLgnPass.Text = "";

               //redirecting to main screen

               if(isadmin != 1)
               {
                   this.Close();
                   Main main = new Main();
                   main.ShowDialog();
                   Application.Exit();
               }
               else
               {                
                   this.Hide();
                   Admin admin = new Admin();
                   admin.ShowDialog();
                   Application.Exit();
               }

               return;
               
           }
           else
           {
             
               MessageBox.Show("Invalid Login");
               txtLgnUsername.Text = "";
               txtLgnPass.Text = "";

           }
            db.closeConnection();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnlgnregister(object sender, EventArgs e)
        {
            this.Hide();
            Register reg = new Register();
            reg.ShowDialog();
            Application.Exit();

        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }

       
    }
}
