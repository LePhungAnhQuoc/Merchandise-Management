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
    /// Interaction logic for ucHomePoster.xaml
    /// </summary>
    public partial class ucHomePoster : UserControl
    {
        #region DependencyProperties


        public string lblHeader
        {
            get { return (string)GetValue(lblHeaderProperty); }
            set { SetValue(lblHeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for lblHeader.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty lblHeaderProperty =
            DependencyProperty.Register("lblHeader", typeof(string), typeof(ucHomePoster), new UIPropertyMetadata("Action"));



        public string tblData
        {
            get { return (string)GetValue(tblDataProperty); }
            set { SetValue(tblDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for tblData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty tblDataProperty =
            DependencyProperty.Register("tblData", typeof(string), typeof(ucHomePoster), new UIPropertyMetadata("Action"));


        #endregion

        public ucHomePoster()
        {
            InitializeComponent();
        }
    }
}
