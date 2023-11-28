using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnhQuoc_WPF_C4_B1
{
    class Menu
    {
        #region Fields
        public List<string> Items = new List<string>();
        public int Count { get { return Items.Count; } }
        #endregion

        #region Methods
        public int Select(bool isEsc)
        {
            int startIndex = isEsc ? 0 : 1;
            return Utilities.InputInt(">= && <=", 0, Count);
        }
        public int Select(bool isEsc, int max)
        {
            int startIndex = isEsc ? 0 : 1;
            return Utilities.InputInt(">= && <=", 0, max);
        }
        public void Output(bool isEsc)
        {
            string outputFormat = "{0}. {1}";
            Utilities.Output(outputFormat, Items);

            int tempNumber = 6;
            if (Items.Count >= tempNumber)
                Console.CursorTop = tempNumber;
            if (isEsc)
            {
                string escapeStr = string.Format(outputFormat, 0, "Exit");
                Utilities.WriteLine(escapeStr, ConsoleColor.DarkYellow);
            }
        }
        public void OutputMainMenu(bool isEsc)
        {
            int no = 0;
            string outputFormat = "{0}. {1}";

            int indexMenu = 0;
            List<string> menuItem = new List<string>();

            menuItem.Add(Items[indexMenu++]);
            Utilities.WriteLine("--- Product ---");
            Utilities.Output(outputFormat, menuItem, ref no);

            menuItem.Clear();
            menuItem.Add(Items[indexMenu++]);
            menuItem.Add(Items[indexMenu++]);
            Utilities.WriteLine("\n--- Receipt ---");
            Utilities.Output(outputFormat, menuItem, ref no);

            menuItem.Clear();
            menuItem.Add(Items[indexMenu++]);
            menuItem.Add(Items[indexMenu++]);
            Utilities.WriteLine("\n--- Invoice ---");
            Utilities.Output(outputFormat, menuItem, ref no);

            menuItem.Clear();
            menuItem.Add(Items[indexMenu++]);
            menuItem.Add(Items[indexMenu++]);
            menuItem.Add(Items[indexMenu++]);
            Utilities.WriteLine("\n--- Inventory ---");
            Utilities.Output(outputFormat, menuItem, ref no);

            menuItem.Clear();
            menuItem.Add(Items[indexMenu++]);
            menuItem.Add(Items[indexMenu++]);
            menuItem.Add(Items[indexMenu++]);
            menuItem.Add(Items[indexMenu++]);
            Utilities.WriteLine("\n--- Statistical ---");
            Utilities.Output(outputFormat, menuItem, ref no);

            menuItem.Clear();
            menuItem.Add(Items[indexMenu++]);
            Utilities.WriteLine("\n--- Others ---");
            Utilities.Output(outputFormat, menuItem, ref no);

            if (isEsc)
            {
                string escapeStr = string.Format(outputFormat, 0, "Exit");
                Utilities.WriteLine(escapeStr, ConsoleColor.DarkYellow);
            }
        }
        #endregion
    }
}
