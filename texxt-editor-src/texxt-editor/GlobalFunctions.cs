using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace texxt_editor
{
    public static class GlobalFunctions
    {
        #region Variable Saving

        public static void SaveColonName(string name, string value)
        {
            // Its a variable, retrieve the value from the dict
            string variableName = name.Substring(1);
            // The string that is stored in :name
            string valueOfVariableName = "";

            // Checks if variablename is within the dict
            if (CommandVariables.Variables.TryGetValue(variableName, out valueOfVariableName))
            {
                SaveNoColons(valueOfVariableName, value);
            }
            else
            {
                MessageBox.Show("No variable found" + variableName);
            }
        }

        public static void SaveColonValue(string name, string value)
        {
            // Its a variable, retrieve the value from the dict
            string variableValue = value.Substring(1);
            // The string that is stored in :name
            string valueOfVariableValue = "";

            // Checks if variablename is within the dict
            if (CommandVariables.Variables.TryGetValue(variableValue, out valueOfVariableValue))
            {
                SaveNoColons(name, valueOfVariableValue);
            }
            else
            {
                MessageBox.Show("No variable found: " + variableValue);
            }
        }

        public static void SaveBothColons(string name, string value)
        {
            string variableName = name.Substring(1);
            string variableValue = value.Substring(1);

            string valueOfVariableName = "";
            string valueOfVariableValue = "";

            // Check if variablename is within the dict
            if (CommandVariables.Variables.TryGetValue(variableName, out valueOfVariableName))
            {
                if (CommandVariables.Variables.TryGetValue(variableValue, out valueOfVariableValue))
                {
                    SaveNoColons(valueOfVariableName, valueOfVariableValue);
                }
                else
                {
                    MessageBox.Show("Variable not found: " + variableValue);
                }
            }
            else
            {
                MessageBox.Show("Variable not found: " + variableName);
            }
        }

        public static void SaveNoColons(string name, string value)
        {
            // Check if the variable already exists
            if (CommandVariables.Variables.ContainsKey(name))
            {
                // Set a new value at that key
                CommandVariables.Variables[name] = value;
            }
            else
            {
                CommandVariables.Variables.Add(name, value);
            }
        }

        #endregion

        public static string GetVariable(string name)
        {
            string result = "";
            if (CommandVariables.Variables.ContainsKey(name))
            {
                CommandVariables.Variables.TryGetValue(name, out result);
                return result;
            }
            else
            {
                throw new Exception("Variable not found: " + name);
            }
        }

        public static string[] SplitText(string text)
        {
            return text.Split('\n').Where(s => !string.IsNullOrEmpty(s)).Where(s => !s.StartsWith("//")).ToArray();
        }
    }
}
