using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace texxt_editor
{
    public partial class Form1 : Form
    {
        private bool editing = true;

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
                if (txtLocation == "")
                {
                    lblFile.Text = "file: no file";
                }
                else
                {
                    lblFile.Text = "file: " + txtLocation;
                }
            }
        }

        public bool Editing
        {
            get
            {
                return editing;
            }
            set
            {
                editing = value;
                if (editing == true)
                {
                    lblFile.Text = saved == true ? "file: no file" : "file: no file *";
                }
                else
                {
                    lblFile.Text = "script mode";
                }
            }
        }

        public Form1()
        {
            InitializeComponent();

            tbEditor.SelectionBackColor = Color.FromArgb(230, 230, 230);

            var args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
            {
                if (File.Exists(args[1]))
                {
                    OpenFile(args[1]);
                }
                else
                {
                    MessageBox.Show("could not find file: " + args[1]);
                    lblFile.Text = "file: no file";
                }
            }
            else
            {
                lblFile.Text = "file: no file";
            }
        }

        void OpenFile(string location)
        {
            // Load from the file
            if (location.EndsWith(".txxt"))
            {
                // Open from script file.
                try
                {
                    using (StreamReader streamReader = new StreamReader(location))
                    {
                        tbEditor.Text = streamReader.ReadToEnd();
                        // Set cursor location to the end
                        tbEditor.SelectionStart = tbEditor.Text.Length != 0 ? tbEditor.Text.Length : 0;
                        TextLocation = location;
                        saved = true;
                        Editing = false;
                        lblFile.Text = txtLocation != "" ? "script mode: " + txtLocation : "script mode";
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show($"there was an error: {e.Message.ToLower()}");
                }
            }
            else
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
                        if (Editing == true)
                        {
                            lblFile.Text = txtLocation != "" ? "file: " + txtLocation : "file: no file";
                        }
                        else
                        {
                            lblFile.Text = txtLocation != "" ? "script mode: " + txtLocation : "script mode";
                        }
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
                (tbEditor.SelectionStart == 0 && e.KeyData == Keys.Back && tbEditor.SelectedText == "") ||
                tbEditor.SelectionStart == tbEditor.TextLength && e.KeyData == Keys.Delete
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
                    openFileDialog.Filter = "Texxt FIles|*.txt;*.txxt|All Files|*.*";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        OpenFile(openFileDialog.FileName);
                    }
                }
            }
            else if (e.KeyCode == Keys.F6)
            {
                if (txtLocation == "")
                {
                    Editing = !Editing;
                }
                else
                {
                    if (txtLocation.EndsWith(".txxt"))
                    {
                        MessageBox.Show("You are currently editing a script. You must close it before entering normal mode.");
                    }
                    else
                    {
                        MessageBox.Show("You are currently editing a file. You must close it before entering script mode.");
                    }
                }
            }
            else if (e.KeyCode == Keys.F5)
            {
                if (Editing == false)
                {
                    tbEditor.SelectionAlignment = HorizontalAlignment.Left;
                    ParseArgs(GlobalFunctions.SplitText(tbEditor.Text));
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                CloseFile();
            }
        }

        private void tbEditor_TextChanged(object sender, EventArgs e)
        {
            if (saved != false)
            {
                saved = false;
                if (Editing == true)
                {
                    lblFile.Text = txtLocation != "" ? "file: " + txtLocation + " *" : "file: no file *";
                }
                else
                {
                    lblFile.Text = txtLocation != "" ? "script mode: " + txtLocation + " *" : "script mode *";
                }
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

        private void tbEditor_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        void CloseFile()
        {
            if (TextLocation != "")
            {
                if (saved == false)
                {
                    SaveFile();
                }
                Editing = true;
                TextLocation = "";
                tbEditor.Text = "";
            }
        }

        void ParseArgs(string[] commands)
        {
            // Basic format of a command: --commandname('args')
            foreach (var command in commands)
            {
                var commandArgsReg = new Regex("'.*?'");
                var commandArgsMatch = commandArgsReg.Matches(command);
                string[] commandArgs = new string[commandArgsMatch.Count];
                for (int i = 0; i < commandArgsMatch.Count; i++)
                {
                    commandArgs[i] = commandArgsMatch[i].ToString().Substring(1, commandArgsMatch[i].ToString().Length - 2);
                }
                var commandNameReg = new Regex(@"-.*?\(");
                var commandNameMatches = commandNameReg.Matches(command);
                string commandName = "";
                foreach (var arg in commandNameMatches)
                {
                    commandName = arg.ToString().Substring(2, arg.ToString().Length - 3);
                }

                if (commandName == "get-cmd-args")
                {
                    if (commandArgs.Length == 1)
                    {
                        GetCommandArgsCommand.Execute(commandArgs[0]);
                    }
                    else if (commandArgs.Length == 2)
                    {
                        GetCommandArgsCommand.Execute(commandArgs[0], commandArgs[1]);
                    }
                }
                else if (commandName == "set-variable")
                {
                    if (commandArgs.Length == 2)
                    {
                        SetVariableCommand.Execute(commandArgs[0], commandArgs[1]);
                    }
                }
                else if (commandName == "print")
                {
                    if (commandArgs.Length == 1)
                    {
                        PrintCommand.Execute(commandArgs[0]);
                    }
                }
                else if (commandName == "exec")
                {
                    if (commandArgs.Length == 1)
                    {
                        ExecuteCommand.Execute(commandArgs[0]);
                    }
                    else if (commandArgs.Length == 2)
                    {
                        ExecuteCommand.Execute(commandArgs[0], commandArgs[1]);
                    }
                }
                else if (commandName == "clean")
                {
                    if (commandArgs.Length == 0)
                    {
                        CleanCommand.Execute();
                    }
                }
                else if (commandName == "delete")
                {
                    if (commandArgs.Length == 1)
                    {
                        DeleteVariableCommand.Execute(commandArgs[0]);
                    }
                }
                else if (commandName == "increment")
                {
                    if (commandArgs.Length == 1)
                    {
                        IncrementCommand.Execute(commandArgs[0]);
                    }
                }
                else if (commandName == "decrement")
                {
                    if (commandArgs.Length == 1)
                    {
                        DecrementCommand.Execute(commandArgs[0]);
                    }
                }
                else if (commandName == "close")
                {
                    if (commandArgs.Length == 0)
                    {
                        CloseCommand.Execute();
                    }
                }
                else
                {
                    MessageBox.Show("Command not recognised: " + commandName);
                }
            }
        }
    }
}
