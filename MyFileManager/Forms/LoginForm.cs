using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MyFileManager
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            user = User.LoadFromFile();
        }
        User user;
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (user.isCorrect(textBoxLogin.Text, textBoxPassword.Text))
            {
                MainForm MainForm = new MainForm();
                Model model = new Model(MainForm);
                MainForm.Show();
                this.Visible = false;
                user.WriteToFile();
            }
        }
    }
}
