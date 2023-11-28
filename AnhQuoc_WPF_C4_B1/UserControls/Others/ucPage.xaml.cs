using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AnhQuoc_WPF_C4_B1
{
    /// <summary>
    /// Interaction logic for ucPage.xaml
    /// </summary>
    public partial class ucPage : UserControl
    {
        #region getData
        public Func<MainWindow> getfrmMain;
        public Func<frmAdmin> getfrmAdmin;
        public Func<frmStocker> getfrmStocker;
        public Func<frmCashier> getfrmCashier;
        #endregion

        #region DependencyProperties
        public List<string> Menus
        {
            get { return (List<string>)GetValue(MenusProperty); }
            set { SetValue(MenusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Menus.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenusProperty =
            DependencyProperty.Register("Menus", typeof(List<string>), typeof(ucPage), new UIPropertyMetadata(new List<string>()));
        #endregion

        public ucPage()
        {
            InitializeComponent();
            lbMenus.SelectionChanged += LbBooks_SelectionChanged;
            Loaded += UcPage_Loaded;
        }

        private void UcPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (getfrmAdmin != null)
                getfrmAdmin().getUcPage = () => this;
            if (getfrmStocker != null)
                getfrmStocker().getUcPage = () => this;
            if (getfrmCashier != null)
                getfrmCashier().getUcPage = () => this;
        }

        private void LbBooks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string menu = lbMenus.SelectedItem as string;

            if (getfrmAdmin != null)
            {
                if (menu == "Update Price")
                {
                    getfrmAdmin().CheckMenu(menu);
                    return;
                }
            }

            if (getfrmStocker != null)
                getfrmStocker().CheckMenu(menu);
            if (getfrmCashier != null)
                getfrmCashier().CheckMenu(menu);
        }
    }
}
