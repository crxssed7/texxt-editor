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
        private bool saved = true;

        private string txtLocation = "";

        public string TextLocation
        {
            get
            {
                return txtLocation;
            }
            set
            {
                txtLocation = value;
                lblFile.Text = "file: " + txtLocation;
            }
        }

        public Form1()
        {
            InitializeComponent();

            tbEditor.SelectionBackColor = Color.FromArgb(230, 230, 230);

            var args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
            {
                // Load from the file
                OpenFile(args[1]);
            }
            else
            {
                lblFile.Text = "file: no file";
            }
        }

        void OpenFile(string location)
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(location))
                {
                    tbEditor.Text = streamReader.ReadToEnd();
                    // Set cursor location to the end
                    tbEditor.SelectionStart = tbEditor.Text.Length != 0 ? tbEditor.Text.Length : 0;
                    TextLocation = location;
                    saved = true;
                    lblFile.Text = txtLocation != "" ? "file: " + txtLocation : "file: no file";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"there was an error: {e.Message.ToLower()}");
            }
        }

        void SaveFile()
        {
            try
            {
                if (TextLocation != "")
                {
                    // Save it to text location
                    using (StreamWriter streamWriter = new StreamWriter(TextLocation))
                    {
                        streamWriter.Write(tbEditor.Text.Trim());
                        saved = true;
                        lblFile.Text = txtLocation != "" ? "file: " + txtLocation : "file: no file";
                    }
                }
                else
                {
                    SaveAs();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"there was an error: {e.Message.ToLower()}");
            }
        }

        void SaveAs()
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.DefaultExt = "txt";
                    saveFileDialog.Filter = "Text FIles|*.txt|All Files|*.*";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string fileLocation = saveFileDialog.FileName;

                        using (StreamWriter streamWriter = new StreamWriter(fileLocation))
                        {
                            streamWriter.Write(tbEditor.Text.Trim());
                            TextLocation = fileLocation;
                            saved = true;
                            lblFile.Text = txtLocation != "" ? "file: " + txtLocation : "file: no file";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"there was an error: {e.Message.ToLower()}");
            }
        }

        private void tbEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (
                tbEditor.GetLineFromCharIndex(tbEditor.SelectionStart) == 0 && e.KeyData == Keys.Up ||
                tbEditor.GetLineFromCharIndex(tbEditor.SelectionStart) == tbEditor.GetLineFromCharIndex(tbEditor.TextLength) && e.KeyData == Keys.Down ||
                tbEditor.SelectionStart == tbEditor.TextLength && e.KeyData == Keys.Right ||
                tbEditor.SelectionStart == 0 && e.KeyData == Keys.Left ||
                (tbEditor.SelectionStart == 0 && e.KeyData == Keys.Back && tbEditor.SelectedText == "")
            ) 
            { 
                e.Handled = true;
            }

            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveFile();
            }
            else if (e.Control && e.KeyCode == Keys.O)
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Text FIles|*.txt|All Files|*.*";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        OpenFile(openFileDialog.FileName);
                    }
                }
            }
        }

        private void tbEditor_TextChanged(object sender, EventArgs e)
        {
            if (saved != false)
            {
                saved = false;
                lblFile.Text = txtLocation != "" ? "file: " + txtLocation + " *" : "file: no file *";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (saved == false && e.CloseReason == CloseReason.UserClosing)
            {
                // Ask to save the file
                if (MessageBox.Show("The file has not been saved. Do you want to save?", "Not saved", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    SaveFile();
                    e.Cancel = true;
                }
            }
        }
    }
}
