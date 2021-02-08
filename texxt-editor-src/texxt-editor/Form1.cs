using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace texxt_editor
{
    public partial class Form1 : Form
    {
        private bool saved = false;

        private string txtLocation = "";

        public Form1()
        {
            InitializeComponent();

            var args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
            {
                // Load from the file
                OpenFile(args[1]);
            }
        }

        void OpenFile(string location)
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(location))
                {
                    tbEditor.Text = streamReader.ReadToEnd();
                    tbEditor.SelectionStart = tbEditor.Text.Length != 0 ? tbEditor.Text.Length - 1 : 0;
                    txtLocation = location;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"there was an error: {e.Message.ToLower()}");
            }
        }
    }
}
