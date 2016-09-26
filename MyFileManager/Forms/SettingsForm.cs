using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MyFileManager
{
    public partial class SettingsForm : Form
    {
        VisualSettings vs;
        MainForm MyParent;
        public SettingsForm(MainForm MyParent)
        {
            InitializeComponent();
            this.vs = MyParent.vs;
            this.MyParent = MyParent;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Show the dialog.
            DialogResult result = fontDialog.ShowDialog();
            // See if OK was pressed.
            if (result == DialogResult.OK)
            {
                // Get Font.
                Font font = fontDialog.Font;
                Font newFont;
                if (font.SizeInPoints > 14f)
                {
                    newFont = new Font(font.FontFamily, 14f, font.Style);
                }
                else
                {
                    newFont = new Font(font, font.Style);
                }
                labelCurFont.Font = newFont;
                vs.Font = newFont;
            }
        }
        private void buttonChangeDirColor_Click(object sender, EventArgs e)
        {
            // Show the dialog.
            DialogResult result = colorDialog.ShowDialog();
            // See if OK was pressed.
            if (result == DialogResult.OK)
            {
                // Get Font.
                Color color = colorDialog.Color;                
                labelCurrentDirColor.BackColor = color;
                vs.DirColor = color;
            }
        }

        private void buttonBackColorChange_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog.ShowDialog();
            // See if OK was pressed.
            if (result == DialogResult.OK)
            {
                // Get Font.
                Color color = colorDialog.Color;
                labelCurrentBackColor.BackColor = color;
                vs.BackColor = color;
            }
        }

        private void Save()
        {
            vs.WriteToFile();
        }
        private void SettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Save();
            MyParent.RedrawGUI();
        }
        private void SettingsForm_Load(object sender, EventArgs e)
        {
            labelCurrentBackColor.BackColor = vs.BackColor;
            labelCurFont.Font = vs.Font;
            labelCurrentDirColor.BackColor = vs.DirColor;
        }
    }
}
