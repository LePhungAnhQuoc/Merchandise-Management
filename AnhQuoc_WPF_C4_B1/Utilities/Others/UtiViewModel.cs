using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AnhQuoc_WPF_C4_B1
{
    class UtiViewModel
    {
        public static void ReadLine(out DateTime value, bool isEsc)
        {
            int[] data = new int[3] { 0, 0, 0 };
            string[] texts = new string[3] { "Day", "Month", "Year" };

            ReadLine:
            {
                for (int index = 0; index < 3; index++)
                {
                    Console.Write(texts[index] + ": ");
                    Utilities.ReadLine(out data[index], string.Empty, -1, -1);
                    if (data[index] == 0)
                    {
                        data[0] = 01;
                        data[1] = 01;
                        data[2] = 0001;
                        break;
                    }
                }
            }
            try
            {
                value = new DateTime(data[2], data[1], data[0]);
            }
            catch (Exception ex)
            {
                Utilities.ClearInvalidInput(ex.Message, Utilities.GetCurrentCursorPosition());
                Utilities.ClearLines(4);
                Console.WriteLine();
                goto ReadLine;
            }
        }

        #region MessageBoxShow
        public static void NotifyChooseProduct() => MessageBox.Show("Please choose your Product", "", MessageBoxButton.OK, MessageBoxImage.Information);
        #endregion
    }
}
