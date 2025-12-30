using System;
using System.Windows.Forms;
using System.Drawing;

namespace AsBuiltExplorer.Utilities
{
    public enum CompareMethod
    {
        Binary = 0,
        Text = 1
    }

    public static class VBCompat
    {
        public static string Mid(string str, int start, int length)
        {
            if (string.IsNullOrEmpty(str)) return "";
            if (start <= 0) return ""; 
            if (start > str.Length) return "";
            
            int p = start - 1;
            if (p + length > str.Length) length = str.Length - p;
            if (length < 0) return "";
            
            return str.Substring(p, length);
        }

        public static string Mid(string str, int start)
        {
             if (string.IsNullOrEmpty(str)) return "";
             if (start > str.Length) return "";
             int p = start - 1;
             return str.Substring(p);
        }

        public static string Left(string str, int length)
        {
            if (string.IsNullOrEmpty(str)) return "";
            if (length <= 0) return "";
            if (length > str.Length) return str;
            return str.Substring(0, length);
        }

        public static string Right(string str, int length)
        {
            if (string.IsNullOrEmpty(str)) return "";
            if (length <= 0) return "";
            if (length > str.Length) return str;
            return str.Substring(str.Length - length);
        }

        public static int Len(string str)
        {
            return str?.Length ?? 0;
        }
        
        public static int InStr(int start, string str, string search, StringComparison comparisonType = StringComparison.Ordinal)
        {
             if (string.IsNullOrEmpty(str)) return 0;
             if (start < 1) return 0; 
             if (start > str.Length) return 0;
             
             int idx = str.IndexOf(search, start - 1, comparisonType);
             if (idx == -1) return 0;
             return idx + 1; 
        }

        public static int InStr(string str, string search, StringComparison comparisonType = StringComparison.Ordinal)
        {
            return InStr(1, str, search, comparisonType);
        }
        
        public static int InStrRev(string str, string search, int start = -1, StringComparison comparisonType = StringComparison.Ordinal)
        {
             if (string.IsNullOrEmpty(str)) return 0;
             if (start == -1) start = str.Length;
             if (start > str.Length) start = str.Length; 
             
             int p = start - 1;
             if (p < 0) return 0;
             if (p >= str.Length) p = str.Length - 1;

             int idx = str.LastIndexOf(search, p, comparisonType);
             if (idx == -1) return 0;
             return idx + 1;
        }

        public static string Trim(string str)
        {
            return str?.Trim() ?? "";
        }

        public static string RTrim(string str) { return str?.TrimEnd() ?? ""; }
        public static string LTrim(string str) { return str?.TrimStart() ?? ""; }

        public static char Chr(int charCode)
        {
            if (charCode < 0 || charCode > 255) 
            {
               // Fallback
            }
            return (char)charCode;
        }
        
        public static int Asc(char c) { return (int)c; }
        public static int Asc(string s) { return string.IsNullOrEmpty(s) ? 0 : (int)s[0]; }
        
        public static int Val(string s)
        {
             if (string.IsNullOrEmpty(s)) return 0;
             s = s.Trim();
             if (s.StartsWith("&h", StringComparison.OrdinalIgnoreCase) || s.StartsWith("&H"))
             {
                 try { return Convert.ToInt32(s.Substring(2), 16); } catch { return 0; }
             }
             
             string nums = "";
             foreach(char c in s)
             {
                 if (char.IsDigit(c) || c == '.' || c == '-') nums += c;
                 else break;
             }
             if (nums.Length == 0) return 0;
             double.TryParse(nums, out double res);
             return (int)res; 
        }

        public static string Replace(string expression, string find, string replacement)
        {
            if (string.IsNullOrEmpty(expression)) return "";
            return expression.Replace(find, replacement);
        }
        
        public static string[] Split(string expression, string delimiter)
        {
             if (expression == null) return new string[0];
             if (string.IsNullOrEmpty(delimiter)) return new string[] { expression };
             return expression.Split(new string[] { delimiter }, StringSplitOptions.None);
        }

        public static string UCase(string str) { return str?.ToUpper() ?? ""; }
        public static string LCase(string str) { return str?.ToLower() ?? ""; }

        public static string Hex(int number) { return number.ToString("X"); }
        public static string Hex(long number) { return number.ToString("X"); }
        public static string Hex(object number) 
        { 
             if (number == null) return "0";
             try { return string.Format("{0:X}", number); }
             catch { return "0"; }
        }

        public static string Format(object expression, string style)
        {
             if (expression == null) return "";
             if (expression is IFormattable f) return f.ToString(style, null);
             return expression.ToString();
        }
        public static string InputBox(string prompt, string title = "", string defaultResponse = "")
        {
            Form inputForm = new Form();
            inputForm.Width = 400;
            inputForm.Height = 200;
            inputForm.Text = title;
            inputForm.StartPosition = FormStartPosition.CenterScreen;
            inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputForm.MaximizeBox = false;
            inputForm.MinimizeBox = false;

            Label lbl = new Label();
            lbl.Left = 20;
            lbl.Top = 20;
            lbl.Width = 340;
            lbl.Text = prompt;
            inputForm.Controls.Add(lbl);

            TextBox txt = new TextBox();
            txt.Left = 20;
            txt.Top = 50;
            txt.Width = 340;
            txt.Text = defaultResponse;
            inputForm.Controls.Add(txt);

            Button btnOk = new Button();
            btnOk.Text = "OK";
            btnOk.Left = 210;
            btnOk.Top = 100;
            btnOk.DialogResult = DialogResult.OK;
            inputForm.Controls.Add(btnOk);

            Button btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Left = 290;
            btnCancel.Top = 100;
            btnCancel.DialogResult = DialogResult.Cancel;
            inputForm.Controls.Add(btnCancel);

            inputForm.AcceptButton = btnOk;
            inputForm.CancelButton = btnCancel;

            if (inputForm.ShowDialog() == DialogResult.OK)
            {
                return txt.Text;
            }
            return "";
        }
        public static bool IsNothing(object obj)
        {
            return obj == null;
        }
        public static int CompareString(string left, string right, bool textCompare)
        {
            if (left == right) return 0;
            if (left == null) left = "";
            if (right == null) right = "";
            StringComparison comparison = textCompare ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            return string.Compare(left, right, comparison);
        }

         public static int StrComp(string left, string right, CompareMethod compare)
        {
            StringComparison comparison = (compare == CompareMethod.Text) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
            return string.Compare(left, right, comparison);
        }

        // Overloads for CompareMethod
        public static int InStr(int start, string str, string search, CompareMethod compare)
        {
             return InStr(start, str, search, (compare == CompareMethod.Text) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }
        public static int InStr(string str, string search, CompareMethod compare)
        {
            return InStr(1, str, search, (compare == CompareMethod.Text) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }
        public static int InStrRev(string str, string search, int start, CompareMethod compare)
        {
            return InStrRev(str, search, start, (compare == CompareMethod.Text) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }
        public static int InStrRev(string str, string search, CompareMethod compare)
        {
            return InStrRev(str, search, -1, (compare == CompareMethod.Text) ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }
    }

    public static class Conversions
    {
        public static string ToString(object obj) => obj?.ToString() ?? "";
        public static string ToString(bool b) => b.ToString();
        public static string ToString(int i) => i.ToString();
        public static string ToString(long l) => l.ToString();
        public static string ToString(double d) => d.ToString();
        
        public static int ToInteger(object obj)
        {
             if (obj == null) return 0;
             return Convert.ToInt32(obj);
        }
        
        public static string Hex(int num) => VBCompat.Hex(num);
        public static string Hex(long num) => VBCompat.Hex(num);
        public static string Hex(object num) => VBCompat.Hex(num);
        public static int Val(string s) => VBCompat.Val(s);
    }
    
    public static class Utils
    {
         public static Array CopyArray(Array source, Array dest)
         {
             if (source == null) return dest;
             if (dest == null) return null;
             
             int len = Math.Min(source.Length, dest.Length);
             Array.Copy(source, dest, len);
             return dest;
         }
    }

    public static class Information
    {
        public static bool IsNumeric(object expression)
        {
            if (expression == null) return false;
            return double.TryParse(expression.ToString(), out _);
        }
        public static bool IsDate(object expression)
        {
            if (expression == null) return false;
            return DateTime.TryParse(expression.ToString(), out _);
        }
        public static bool IsNothing(object obj) => obj == null;
    }
}
