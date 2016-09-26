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
    [Serializable]
    public class VisualSettings
    {
        public VisualSettings(Font font, Color dirColor, Color backColor)
        {
            this.font = font;
            this.dirColor = dirColor;
            this.backColor = backColor;
        }
        public VisualSettings()
        {
            //set default settings
            backColor = SettingsForm.DefaultBackColor;
            dirColor = Color.Coral;
            font = SettingsForm.DefaultFont;
        }
        public static VisualSettings LoadFromFile()
        {
            var fileName = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), "MyVisualSettings.dat");
            BinaryFormatter formatter = new BinaryFormatter();
            Entry entry = Factory.GetEntry(fileName);
            if (entry.Exists)
            {
                using (FileStream fs = entry.FileOpen(FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return (VisualSettings)formatter.Deserialize(fs);
                }
            }
            else
            {
                return new VisualSettings();
            }
        }
        public void WriteToFile()
        {
            var fileName = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), "MyVisualSettings.dat");
            BinaryFormatter formatter = new BinaryFormatter();
            Entry entry = Factory.GetEntry(fileName);
            using (FileStream fs = entry.FileOpen(FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(fs, this);
            }
        }
        Font font;
        public Font Font
        {
            get
            {
                return font;
            }
            set
            {
                font = value;
            }
        }

        Color dirColor;
        public Color DirColor
        {
            get
            {
                return dirColor;
            }
            set
            {
                dirColor = value;
            }
        }

        Color backColor;
        public Color BackColor
        {
            get
            {
                return backColor;
            }
            set
            {
                backColor = value;
            }
        }
    }
}
