using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace texxt_editor
{
    public static class GetCommandArgsCommand
    {
        public static void Execute(string name)
        {
            // get the cmd args
            var args = Environment.GetCommandLineArgs();
            string value = string.Join(",", args);

            // Need to check if what was passed in name is a variable
            if (name.StartsWith(":"))
            {
                GlobalFunctions.SaveColonName(name, value);
            }
            else
            {
                GlobalFunctions.SaveNoColons(name, value);
            }
        }
    }

    public static class SetVariableCommand
    {
        public static void Execute(string name, string value)
        {
            if (name.StartsWith(":") && value.StartsWith(":"))
            {
                // Both name and value are variables.
                GlobalFunctions.SaveBothColons(name, value);
            }
            else if (!name.StartsWith(":") && value.StartsWith(":"))
            {
                GlobalFunctions.SaveColonValue(name, value);
            }
            else if (name.StartsWith(":") && !value.StartsWith(":"))
            {
                GlobalFunctions.SaveColonName(name, value);
            }
            else
            {
                GlobalFunctions.SaveNoColons(name, value);
            }
        }
    }

    public static class PrintCommand
    {
        public static void Execute(string value)
        {
            if (value.StartsWith(":"))
            {
                // Its a variable, get its contents
                try
                {
                    MessageBox.Show(GlobalFunctions.GetVariable(value.Substring(1)));
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            else
            {
                MessageBox.Show(value);
            }
        }
    }

    public static class ExecuteCommand
    {
        public static void Execute(string path)
        {
            if (path.StartsWith(":"))
            {
                try
                {
                    string actualPath = GlobalFunctions.GetVariable(path.Substring(1));
                    try
                    {
                        System.Diagnostics.Process.Start(actualPath);
                    }
                    catch
                    {
                        MessageBox.Show("Could not start process: " + actualPath);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            else
            {
                try
                {
                    System.Diagnostics.Process.Start(path);
                }
                catch
                {
                    MessageBox.Show("Could not start process: " + path);
                }
            }
        }

        public static void Execute(string path, string args)
        {
            if (path.StartsWith(":") && !args.StartsWith(":"))
            {
                try
                {
                    string actualPath = GlobalFunctions.GetVariable(path.Substring(1));
                    try
                    {
                        System.Diagnostics.Process.Start(actualPath, args);
                    }
                    catch
                    {
                        MessageBox.Show("Could not start process: " + actualPath);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            else if (!path.StartsWith(":") && args.StartsWith(":"))
            {
                try
                {
                    string actualArgs = GlobalFunctions.GetVariable(args.Substring(1));
                    try
                    {
                        System.Diagnostics.Process.Start(path, actualArgs);
                    }
                    catch
                    {
                        MessageBox.Show("Could not start process: " + path);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            else if (path.StartsWith(":") && args.StartsWith(":"))
            {
                try
                {
                    string actualArgs = GlobalFunctions.GetVariable(args.Substring(1));
                    string actualPath = GlobalFunctions.GetVariable(path.Substring(1));
                    try
                    {
                        System.Diagnostics.Process.Start(actualPath, actualArgs);
                    }
                    catch
                    {
                        MessageBox.Show("Could not start process: " + actualPath);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            else
            {
                try
                {
                    System.Diagnostics.Process.Start(path, args);
                }
                catch
                {
                    MessageBox.Show("Could not start process: " + path);
                }
            }
        }
    }

    public static class CleanCommand
    {
        public static void Execute()
        {
            CommandVariables.Variables.Clear();
        }
    }

    public static class DeleteVariableCommand
    {
        public static void Execute(string name)
        {
            if (name.StartsWith(":"))
            {
                string actualName = GlobalFunctions.GetVariable(name.Substring(1));
                if (CommandVariables.Variables.ContainsKey(actualName))
                {
                    CommandVariables.Variables.Remove(actualName);
                }
                else
                {
                    MessageBox.Show("Variable not found: " + actualName);
                }
            }
            else
            {
                if (CommandVariables.Variables.ContainsKey(name))
                {
                    CommandVariables.Variables.Remove(name);
                }
                else
                {
                    MessageBox.Show("Variable not found: " + name);
                }
            }
        }
    }

    public static class IncrementCommand
    {
        public static void Execute(string name)
        {
            if (name.StartsWith(":"))
            {
                string actualName = GlobalFunctions.GetVariable(name.Substring(1));

                // Get the variable
                if (CommandVariables.Variables.ContainsKey(actualName))
                {
                    try
                    {
                        double value = Convert.ToDouble(GlobalFunctions.GetVariable(actualName));
                        value++;
                        GlobalFunctions.SaveNoColons(actualName, value.ToString());
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Increment error: " + e.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Variable not found: " + actualName);
                }
            }
            else
            {
                // Get the variable
                if (CommandVariables.Variables.ContainsKey(name))
                {
                    try
                    {
                        double value = Convert.ToDouble(GlobalFunctions.GetVariable(name));
                        value++;
                        GlobalFunctions.SaveNoColons(name, value.ToString());
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Increment error: " + e.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Variable not found: " + name);
                }
            }
        }
    }

    public static class DecrementCommand
    {
        public static void Execute(string name)
        {
            if (name.StartsWith(":"))
            {
                string actualName = GlobalFunctions.GetVariable(name.Substring(1));

                // Get the variable
                if (CommandVariables.Variables.ContainsKey(actualName))
                {
                    try
                    {
                        double value = Convert.ToDouble(GlobalFunctions.GetVariable(actualName));
                        value--;
                        GlobalFunctions.SaveNoColons(actualName, value.ToString());
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Increment error: " + e.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Variable not found: " + actualName);
                }
            }
            else
            {
                // Get the variable
                if (CommandVariables.Variables.ContainsKey(name))
                {
                    try
                    {
                        double value = Convert.ToDouble(GlobalFunctions.GetVariable(name));
                        value--;
                        GlobalFunctions.SaveNoColons(name, value.ToString());
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Increment error: " + e.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Variable not found: " + name);
                }
            }
        }
    }
}
