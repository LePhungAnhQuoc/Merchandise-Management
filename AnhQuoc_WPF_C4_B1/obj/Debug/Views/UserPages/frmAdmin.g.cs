﻿#pragma checksum "..\..\..\..\Views\UserPages\frmAdmin.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "C6327DF3C9E6D631D1A9D4B50CD6F21F7AC956F9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AnhQuoc_WPF_C4_B1;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace AnhQuoc_WPF_C4_B1 {
    
    
    /// <summary>
    /// frmAdmin
    /// </summary>
    public partial class frmAdmin : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 48 "..\..\..\..\Views\UserPages\frmAdmin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeView tvDashBoard;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\..\..\Views\UserPages\frmAdmin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeViewItem tvHomePage;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\..\Views\UserPages\frmAdmin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeViewItem tvProduct;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\..\Views\UserPages\frmAdmin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeViewItem tvInventory;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\..\..\Views\UserPages\frmAdmin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeViewItem tvInventoryStatus;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\..\Views\UserPages\frmAdmin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeViewItem tvRevenueProfit;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\..\Views\UserPages\frmAdmin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TreeViewItem tvLogOut;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\..\Views\UserPages\frmAdmin.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gdView;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AnhQuoc_WPF_C4_B1;component/views/userpages/frmadmin.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Views\UserPages\frmAdmin.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.tvDashBoard = ((System.Windows.Controls.TreeView)(target));
            return;
            case 2:
            this.tvHomePage = ((System.Windows.Controls.TreeViewItem)(target));
            
            #line 53 "..\..\..\..\Views\UserPages\frmAdmin.xaml"
            this.tvHomePage.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.tvHomePage_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.tvProduct = ((System.Windows.Controls.TreeViewItem)(target));
            
            #line 57 "..\..\..\..\Views\UserPages\frmAdmin.xaml"
            this.tvProduct.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.tvProduct_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 4:
            this.tvInventory = ((System.Windows.Controls.TreeViewItem)(target));
            
            #line 61 "..\..\..\..\Views\UserPages\frmAdmin.xaml"
            this.tvInventory.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.tvInventory_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 5:
            this.tvInventoryStatus = ((System.Windows.Controls.TreeViewItem)(target));
            
            #line 65 "..\..\..\..\Views\UserPages\frmAdmin.xaml"
            this.tvInventoryStatus.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.tvInventoryStatus_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 6:
            this.tvRevenueProfit = ((System.Windows.Controls.TreeViewItem)(target));
            
            #line 66 "..\..\..\..\Views\UserPages\frmAdmin.xaml"
            this.tvRevenueProfit.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.tvRevenueProfit_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 7:
            this.tvLogOut = ((System.Windows.Controls.TreeViewItem)(target));
            
            #line 70 "..\..\..\..\Views\UserPages\frmAdmin.xaml"
            this.tvLogOut.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.tvLogOut_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 8:
            this.gdView = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

