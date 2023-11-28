using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Runtime.CompilerServices;
using System.IO;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace AnhQuoc_WPF_C4_B1
{
    public class Utilities
    {
        public static void ClearfReceipt()
        {
            DataProvider.Instance.Open(Constants.fReceipts);
            RemoveAllChilds(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();

            DataProvider.Instance.Open(Constants.fReceiptDetails);
            RemoveAllChilds(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();
        }

        public static void ClearfInvoice()
        {
            DataProvider.Instance.Open(Constants.fInvoices);
            RemoveAllChilds(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();

            DataProvider.Instance.Open(Constants.fInvoiceDetails);
            RemoveAllChilds(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();
        }
        public static void ClearfProductInvoice()
        {
            DataProvider.Instance.Open(Constants.fProductInvoices);
            RemoveAllChilds(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();
        }
        public static void ClearfProductInvoiceOrder()
        {
            DataProvider.Instance.Open(Constants.fProductInvoicesOrder);
            RemoveAllChilds(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();
        }

        public static void ClearfInventory()
        {
            DataProvider.Instance.Open(Constants.fInventory);
            RemoveAllChilds(DataProvider.Instance.nodeRoot.FirstChild);
            DataProvider.Instance.Close();
        }
        public static void ClearfProductInventory()
        {
            DataProvider.Instance.Open(Constants.fProductInventorys);
            RemoveAllChilds(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();
        }
        public static void ClearfProductInventoryStatus()
        {
            DataProvider.Instance.Open(Constants.fProductInventorysStatus);
            RemoveAllChilds(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();
        }

        public static void ClearfProductImportInventorysStatus()
        {
            DataProvider.Instance.Open(Constants.fProductImportInventorysStatus);
            RemoveAllChilds(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();
        }

        public static void ClearfProductImportInventory()
        {
            DataProvider.Instance.Open(Constants.fImportInventory);
            RemoveAllChilds(DataProvider.Instance.nodeRoot.FirstChild);
            DataProvider.Instance.Close();
        }

        public static void ClearfOrder()
        {
            DataProvider.Instance.Open(Constants.fOrders);
            RemoveAllChilds(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();

            DataProvider.Instance.Open(Constants.fOrderDetails);
            RemoveAllChilds(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();
        }

        public static void ClearfCustomer()
        {
            DataProvider.Instance.Open(Constants.fCustomers);
            RemoveAllChilds(DataProvider.Instance.nodeRoot);
            DataProvider.Instance.Close();
        }

        public static void ClearAllFile()
        {
            ClearfReceipt();
            ClearfInvoice();
            ClearfOrder();

            ClearfInventory();
            ClearfProductImportInventory();

            ClearfProductImportInventorysStatus();
            ClearfProductInventory();
            ClearfProductInventoryStatus();
           
            ClearfProductInvoice();
            ClearfProductInvoiceOrder();
           
            ClearfCustomer();
        }

        private static int getIndexDirectory(string filePath)
        {
            if (!filePath.Contains(".xml"))
            {
                return -1;
            }
            int index = filePath.Length - 1;
            foreach (char c in filePath.Reverse())
            {
                if (c.ToString() == "\\" || c.ToString() == "/")
                {
                    break;
                }
                index--;
            }
            return index;
        }

        public static void CreateXML(string filePath, string rootName)
        {
            string temp_filePath = filePath;
            int indexOf = getIndexDirectory(filePath);
            if (indexOf == -1)
            {
                return;
            }
            string directory = filePath.Remove(indexOf);
            CreateDirectory(directory);

            filePath = temp_filePath;
            if (!File.Exists(filePath))
            {
                XmlDocument doc = new XmlDocument();
                XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);

                XmlNode root = doc.DocumentElement;
                doc.InsertBefore(xmlDeclaration, root);

                XmlElement element1 = doc.CreateElement(string.Empty, rootName, string.Empty);
                doc.AppendChild(element1);
                try { doc.Save(filePath); }
                catch { }
            }
        }


        public static void CreateDirectory(string folderPath)
        {
            // If directory does not exist, create it
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        public static void RenameDirectory(string oldName, string newName)
        {
            // If directory does not exist, create it
            if (Directory.Exists(oldName))
            {
                Directory.Move(oldName, newName);
            }
        }

        public static void DeleteDirectory(string folderPath)
        {
            // If directory does not exist, create it
            if (Directory.Exists(folderPath))
            {
                Directory.Delete(folderPath, true);
            }
        }


        public static void RenameFile(string oldName, string newName)
        {
            // If directory does not exist, create it
            if (File.Exists(oldName))
            {
                File.Move(oldName, newName);
            }
        }

        public static void DeleteFile(string folderPath)
        {
            // If directory does not exist, create it
            if (File.Exists(folderPath))
            {
                File.Delete(folderPath);
            }
        }


        public static void removeGridCol(Grid grid1, int index)
        {
            if (grid1.ColumnDefinitions.Count <= 0)
            {
                return;
            }
            else
            {
                try { grid1.ColumnDefinitions.RemoveAt(index); }
                catch { }
            }
        }

        public static void addGridCol(Grid grid1)
        {
            var colDef1 = new ColumnDefinition();
            grid1.ColumnDefinitions.Add(colDef1);
        }

        public static void removeGridRow(Grid grid1, int index)
        {
            if (grid1.RowDefinitions.Count <= 0)
            {
                return;
            }
            else
            {
                grid1.RowDefinitions.RemoveAt(index);
            }
        }

        public static void addRow(Grid grid1)
        {
            var rowDef1 = new RowDefinition();
            grid1.RowDefinitions.Add(rowDef1);
        }

        public static int ValidateNumber(char character, int min = 0, int max = 9)
        {
            if (!char.IsDigit(character))
                return -1;
            int characterNumber = -1;
            characterNumber = (int)character - 48;
            if (characterNumber < min || characterNumber > max)
            {
                return -2;
            }
            return 1;
        }

        public static bool ValidateNumber(string strInput, int min = 0, int max = 9)
        {
            string regexString = string.Format(@"^[{0}-{1}]+$", min, max);

            Regex regex = new Regex(regexString);
            return regex.IsMatch(strInput);
        }

        public static bool ValidateText(string strInput)
        {
            return Regex.IsMatch(strInput, @"^[a-zA-Z ]+$");
        }

        public static bool ValidateTextNumber(string strInput)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9]+$");
            return regex.IsMatch(strInput);
        }

        public static void CatchError()
        {
            // Phat trien them
            MessageBox.Show("This feature is currently error! Please restart the program");
            Environment.Exit(0);
        }
        public static Brush GetColorCode(string colorCode)
        {
            return new BrushConverter().ConvertFrom(colorCode) as Brush;
        }

        public static string Join(object[] source, string seperate = " ")
        {
            StringBuilder result = new StringBuilder();
            foreach (object item in source)
            {
                result.Append(item);
                result.Append(seperate);
            }
            result.Remove(result.Length - 1, 1);
            return result.ToString();
        }
        public static void Allocate<T>(out List<List<T>> source, int row, int col) where T : class, new()
        {
            source = new List<List<T>>(row);
            for (int idx = 0; idx < source.Capacity; idx += 1)
            {
                source.Add(new List<T>(col));
                for (int idx2 = 0; idx2 < source[idx].Capacity; idx2 += 1)
                {
                    source[idx].Add(new T());
                }
            }
        }
        public static void Stop(ConsoleKey keyContinue)
        {
            while (Console.ReadKey(true).Key != keyContinue) ;
        }
        public static void RemoveAllChilds(XmlNode parentNode)
        {
            XmlNodeList lstChild = parentNode.ChildNodes;
            while (lstChild.Count > 0)
            {
                parentNode.RemoveChild(lstChild[0]);
            }
        }
        public static int ConvertToInt(string number)
        {
            number = number.Replace(".", "");
            return int.Parse(number);
        }
        public static string GetIsExistMessage(bool isExist, string item)
        {
            string result = string.Empty;
            if (isExist)
                result = string.Format("This {0} already exists in the list", item);
            else
                result = string.Format("This {0} is currently not in the list", item);
            return result;
        }
        public static void NotifyAvailable(string item, string lstName, bool isClearScreen, ConsoleColor color = ConsoleColor.DarkYellow)
        {
            WriteLine($"There are no {item} in the {lstName}.", color);
            if (isClearScreen)
            {
                Console.ReadKey(true);
                Console.Clear();
            }
        }
        public static string GetListEmptyMessage(string item, string lstName = "List")
        {
            return $"There are no {item} in the {lstName}.";
        }
        public static string GetFormEmptyMessage()
        {
            return "Please enter all the information text form";
        }
        public static string GetCatchErorExceptionMessage()
        {
            return "Incorrect format input";
        }
        public static string GetSaveMessage(string item)
        {
            return $"Do you want to save {item}?";
        }
        public static string GetCancelMessage(string item)
        {
            return $"Do you want to cancel this {item}?";
        }
        public static string ErrorStr(string errorDescription)
        {
            return "Error {0} Can not continue this operation";
        }
        private static string GetLowerString(string value)
        {
            return char.ToLower(value[0]).ToString() + value.Substring(1);
        }
        public static bool ConfirmationView(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.Write("\n" + message);
            return GetYNOptionKey();
        }
        public static void GoTo(List<int> lengths, int index)
        {
            for (int i = 0; i < index; i++)
            {
                try {
                    Console.CursorLeft += lengths[i] + 3;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.ReadKey(true);
                    return;
                }
            }
        }
        public static List<int> GetFixLengths(List<int> lengths, int cdX)
        {
            List<int> result = new List<int>();
            int newValue = cdX;
            newValue += 2;
            foreach (int len in lengths)
            {
                result.Add(newValue);
                newValue += len + 3;
            }
            result.Add(newValue);
            return result;
        }
        public static void InsertRange(List<string> source, int index, List<string> collection)
        {
            foreach (var item in collection)
            {
                source.Insert(index, item);
                index++;
            }
        }
        public static void InsertRange<T>(List<T> source, int index, List<T> collection)
        {
            foreach (var item in collection)
            {
                source.Insert(index, item);
                index++;
            }
        }
        public static void AppendRange<T>(List<T> source, List<T> collection)
        {
            foreach (var item in collection)
            {
                source.Add(item);
            }
        }
        public static void InsertRange(List<int> source, int index, List<int> collection)
        {
            foreach (var item in collection)
            {
                source.Insert(index, item);
                index++;
            }
        }
        public static void InsertRange(List<object> source, int index, List<object> collection)
        {
            foreach (var item in collection)
            {
                source.Insert(index, item);
                index++;
            }
        }
        public static Coord GetCurrentCursorPosition()
        {
            return new Coord(Console.CursorLeft, Console.CursorTop);
        }

        public static void ClearToRight(int length)
        {
            DrawToRight(length, ' ');
        }
        public static void ClearToRight()
        {
            ClearToRight(Console.BufferWidth - Console.CursorLeft);
        }

        public static void ClearToLeft(int length)
        {
            DrawToLeft(length, ' ');
        }
        public static void ClearToLeft()
        {
            ClearToLeft(Console.CursorLeft);
        }

        public static void ClearLine()
        {
            ClearToRight();
            ClearToLeft();
        }

        public static void DrawToRight(int length, char c_draw)
        {
            Coord cd = GetCurrentCursorPosition();
            Console.Write(string.Empty.PadLeft(length, c_draw));
            Console.SetCursorPosition(cd.X, cd.Y);
        }
        public static void DrawToLeft(int length, char c_draw)
        {
            Coord cd = GetCurrentCursorPosition();

            Console.CursorLeft = 0;

            Console.Write(string.Empty.PadLeft(length, c_draw));
            Console.SetCursorPosition(cd.X, cd.Y);
        }
        public static void DrawToTop(int length, char c_draw)
        {
            Coord cd = GetCurrentCursorPosition();
            int tempCdY = cd.Y;
            Write(c_draw.ToString());
            for (int idx = 1; idx < length; idx++)
            {
                try
                {
                    Console.CursorLeft = cd.X;
                    Console.CursorTop = --tempCdY;
                }
                catch { return; }
                Write(c_draw.ToString());
            }
            Console.SetCursorPosition(cd.X, cd.Y);
        }
        public static void DrawToBottom(int length, char c_draw)
        {
            Coord cd = GetCurrentCursorPosition();
            int tempCdY = cd.Y;
            Write(c_draw.ToString());
            for (int idx = 1; idx < length; idx++)
            {
                try
                {
                    Console.CursorLeft = cd.X;
                    Console.CursorTop = ++tempCdY;
                }
                catch { return; }
                Write(c_draw.ToString());
            }
            Console.SetCursorPosition(cd.X, cd.Y);
        }

        public static void DrawRectangle(int width, int height, char rowChar, char colChar)
        {
            Coord currentPos = GetCurrentCursorPosition();
            DrawToRight(width, rowChar);
            try { Console.CursorTop += (height - 1); }
            catch { return; }
            DrawToRight(width, rowChar);

            DrawToTop(height, colChar);
            try { Console.CursorLeft += (width - 1); }
            catch { return; }
            DrawToTop(height, colChar);
            Console.SetCursorPosition(currentPos.X, currentPos.Y);
        }

        public static void ClearLines(int numberOfLines)
        {
            for (int index = 0; index < numberOfLines; index++)
            {
                ClearLine();
                try { Console.CursorTop--; }
                catch { }
            }
        }

        public static void ClearInvalidInput(Coord cd)
        {
            ClearInvalidInput(string.Empty, cd);
        }
        public static void ClearInvalidInput(string message, Coord cd)
        {
            if (message != string.Empty)
            {
                Write(message, ConsoleColor.DarkYellow);
                Console.ReadKey(true);
                Console.Write('\r');
                ClearToRight();
            }
            Console.SetCursorPosition(cd.X, cd.Y);
            ClearToRight();
        }

        private static bool IsCheckCompare(int value, string compareType, int min, int max)
        {
            switch (compareType)
            {
                case ">":
                    return (value > min);
                case "<":
                    return (value < max);
                case "> && <":
                    return (value > min) && (value < max);

                case ">=":
                    return (value >= min);
                case "<=":
                    return (value <= max);

                case ">= && <=":
                    return (value >= min) && (value <= max);
                case "> && <=":
                    return (value > min) && (value <= max);
                case ">= && <":
                    return (value >= min) && (value < max);

                case "":
                    return true;
                default:
                    Write("Error formatting!", ConsoleColor.DarkRed);
                    return false;
            }
        }
        private static bool IsCheckCompare(double value, string compareType, double min, double max)
        {
            switch (compareType)
            {
                case ">":
                    return (value > min);
                case "<":
                    return (value < max);
                case "> && <":
                    return (value > min) && (value < max);

                case ">=":
                    return (value >= min);
                case "<=":
                    return (value <= max);

                case ">= && <=":
                    return (value >= min) && (value <= max);
                case "> && <=":
                    return (value > min) && (value <= max);
                case ">= && <":
                    return (value >= min) && (value < max);

                case "":
                    return true;
                default:
                    Write("Error formatting!", ConsoleColor.DarkRed);
                    return false;
            }
        }
        public static void ReadLine(out int value, string compareType, int min, int max)
        {
            bool isValidInput = false;
            bool checkCompare = false;
            Coord cd = GetCurrentCursorPosition();
            value = 0;
            do
            {
                string str = Console.ReadLine();
                isValidInput = int.TryParse(str, out value);
                if (isValidInput)
                {
                    checkCompare = IsCheckCompare(value, compareType, min, max);
                    if (!checkCompare)
                        ClearInvalidInput(cd);
                }
                else
                {
                    ClearInvalidInput(cd);
                }
            }
            while ((!isValidInput) || (!checkCompare));
        }
        public static void ReadLine(out double value, string compareType, double min = -1, double max = -1)
        {
            bool isValidInput = false;
            bool checkCompare = false;
            Coord cd = GetCurrentCursorPosition();
            value = 0;
            do
            {
                isValidInput = double.TryParse(Console.ReadLine(), out value);
                if (isValidInput)
                {
                    checkCompare = IsCheckCompare(value, compareType, min, max);
                    if (!checkCompare)
                        ClearInvalidInput(cd);
                }
            }
            while ((!isValidInput) || (!checkCompare));
        }
        public static void ReadLine(out string value)
        {
            Coord cd = GetCurrentCursorPosition();
            ReadLine:
            {
                value = Console.ReadLine();
            }
            if (IsEmpty(value))
            {
                Console.SetCursorPosition(cd.X, cd.Y);
                goto ReadLine;
            }
        }
        public static void ReadLine(out DateTime value)
        {
            int[] data = new int[3] { 0, 0, 0 };
            string[] texts = new string[3] { "Day", "Month", "Year" };

            ReadLine:
            {
                for (int index = 0; index < 3; index++)
                {
                    Console.Write(texts[index] + ": ");
                    ReadLine(out data[index], string.Empty, -1, -1);
                }
            }
            try
            {
                value = new DateTime(data[2], data[1], data[0]);
            }
            catch (Exception ex)
            {
                ClearInvalidInput(ex.Message, GetCurrentCursorPosition());
                ClearLines(4);
                Console.WriteLine();
                goto ReadLine;
            }
        }
        public static int InputInt(string compareType, int min = -1, int max = -1)
        {
            int value;
            ReadLine(out value, compareType, min, max);
            return value;
        }
        public static double InputDouble(string compareType, double min = -1, double max = -1)
        {
            double value;
            ReadLine(out value, compareType, min, max);
            return value;
        }
        public static string InputStr()
        {
            string value;
            ReadLine(out value);
            return value;
        }
        public static DateTime InputDate()
        {
            DateTime dateInput;
            ReadLine(out dateInput);
            return dateInput;
        }
        public static DateTime InputDate(List<DateTime> lst, string message)
        {
            DateTime dateInput;
            ReadLine:
            {
                ReadLine(out dateInput);
            }
            if (!IsExist(lst, dateInput))
            {
                ClearInvalidInput(message, GetCurrentCursorPosition());
                ClearLines(4);
                goto ReadLine;
            }
            return dateInput;
        }
        public static bool IsExist(List<DateTime> source, DateTime value)
        {
            foreach (var item in source)
            {
                if (item.Date == value.Date)
                {
                    return true;
                }
            }
            return false;
        }
        public static bool IsExist(List<string> source, string value)
        {
            foreach (var item in source)
            {
                if (item == value)
                    return true;
            }
            return false;
        }
        public static bool IsEmpty(string str)
        {
            return string.IsNullOrEmpty(str)
                || string.IsNullOrWhiteSpace(str);
        }
        public static bool GetYNOptionKey()
        {
            Console.Write(" (Y/N): ");
            char c;
            do
            {
                c = Console.ReadKey(true).KeyChar;
                c = char.ToUpper(c); // Cho phép nhập kí tự in hoa, in thường.
            }
            while ((c != 'Y') && (c != 'N'));

            Console.WriteLine(c);
            return (c == 'Y');
        }

        public static void Write(string str)
        {
            Console.Write(str);
        }
        public static void Write(string str, ConsoleColor textColor)
        {
            ConsoleColor temp = Console.ForegroundColor;

            Console.ForegroundColor = textColor;
            Console.Write(str);

            Console.ForegroundColor = temp;
        }
        public static void WriteAt(string str, int cdX)
        {
            try
            {
                Console.CursorLeft = cdX;
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message, ConsoleColor.DarkYellow);
            }
            Write(str);
        }
        public static void WriteAt(string str, ConsoleColor textColor, int cdX)
        {
            try
            {
                Console.CursorLeft = cdX;
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message, ConsoleColor.DarkYellow);
            }
            Write(str, textColor);
        }

        public static void Write(object str)
        {
            Write(str.ToString());
        }
        public static void Write(object str, ConsoleColor textColor)
        {
            Write(str.ToString(), textColor);
        }
        public static void WriteAt(object str, int cdX)
        {
            WriteAt(str.ToString(), cdX);
        }
        public static void WriteAt(object str, ConsoleColor textColor, int cdX)
        {
            WriteAt(str.ToString(), textColor, cdX);
        }

        public static void WriteLine(string str)
        {
            Console.WriteLine(str);
        }
        public static void WriteLine(string str, ConsoleColor textColor)
        {
            Write(str + "\n", textColor);
        }
        public static void WriteLineAt(string str, int cdX)
        {
            try {
                Console.CursorLeft = cdX;
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message, ConsoleColor.DarkYellow);
            }
            WriteLine(str);
        }
        public static void WriteLineAt(string str, ConsoleColor textColor, int cdX)
        {
            try
            {
                Console.CursorLeft = cdX;
            }
            catch (Exception ex)
            {
                WriteLine(ex.Message, ConsoleColor.DarkYellow);
            }
            WriteLine(str, textColor);
        }

        public static void WriteLine(object str)
        {
            WriteLine(str.ToString());
        }
        public static void WriteLine(object str, ConsoleColor textColor)
        {
            WriteLine(str.ToString(), textColor);
        }
        public static void WriteLineAt(object str, int cdX)
        {
            WriteLineAt(str.ToString(), cdX);
        }
        public static void WriteLineAt(object str, ConsoleColor textColor, int cdX)
        {
            WriteLineAt(str.ToString(), textColor, cdX);
        }
        public static void WriteLine()
        {
            Console.WriteLine();
        }

        public static void Output<T>(string format, IEnumerable<T> source)
        {
            int index = 0;
            int Y = Console.CursorTop;
            int X = Console.CursorLeft;
            foreach (var item in source)
            {
                int tempNumber = 6;
                if (index == tempNumber)
                {
                    Console.CursorTop = Y;
                    X = 60;
                }
                if (format.Contains("{1}"))
                    WriteLineAt(string.Format(format, index + 1, item), X);
                else
                    WriteLineAt(string.Format(format, item), X);
                ++index;
            }
        }
        public static void Output<T>(string format, IEnumerable<T> source, ref int index)
        {
            int Y = Console.CursorTop;
            int X = Console.CursorLeft;
            foreach (var item in source)
            {
                if (format.Contains("{1}"))
                    WriteLineAt(string.Format(format, index + 1, item), X);
                else
                    WriteLineAt(string.Format(format, item), X);
                ++index;
            }
        }

        public static void Output(object[] arg, List<int> lengths)
        {
            // Update code
            Console.Write("| ");

            int number = Console.CursorLeft;
            int index = 0;

            foreach (var item in arg)
            {
                WriteAt(item, number);

                // Update code
                try {
                    number += lengths[index];
                    Console.CursorLeft = number;
                    index++;
                }
                catch { }
                Console.Write(" | ");
                number += 3;
            }
            Console.WriteLine();
        }
        public static void OutputLine(object[] arg, List<string> fields, int distance)
        {
            int index = 0;
            int cdX = Console.CursorLeft;
            foreach (object item in arg)
            {
                WriteAt(fields[index++], cdX);
                WriteLineAt(item.ToString(), cdX + distance);
            }
        }

        public static bool IsCheckLetterString(string str)
        {
            bool isLetterString = str.All(char.IsLetter);
            return isLetterString;
        }
        public static char EnterKey()
        {
            return Convert.ToChar(13);
        }
    }
}