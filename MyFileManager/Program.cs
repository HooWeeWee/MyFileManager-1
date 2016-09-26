using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyFileManager
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Form1 MainForm = new Form1();
            //Application.Run(MainForm);
            LoginForm loginForm = new LoginForm();
            Application.Run(loginForm);
        }
    }
}
