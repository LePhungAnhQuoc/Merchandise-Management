   M S B A M L   `   `   `  ����L  HAnhQuoc_WPF_C4_B1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:AnhQuoc_WPF_C4_B1AnhQuoc_WPF_C4_B1  5      3    -AnhQuoc_WPF_C4_B1.ucListProductInvoiceGeneral   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:AnhQuoc_WPF_C4_B1     5      -   5        $��UserControl��5        � 	Resources  5
      .  (AnhQuoc_WPF_C4_B1.NoteEmptyListConverter   NoteEmptyListConverter��%   &	          5   
   6'   5      .��5      �� .+�5   
   �� -   5        $��lblHeaderUI��5        	lblHeader#��[ 5      $��0 0 0 20q�5      ��5      ��  �� RelativeSource �� *FindAncestor+ �� AncestorType$ UserControlL� �� Path$ 	lblHeader��� 5   
   
���   �$��	0 0.5 0 0q�5      ._�5      �� #��� ��6   .+�5      �� -   5        $��lbCategories��5        listBoxHorizontal#��[ 5      $��Stretch=�5       �� SelectedValuePath$ Products��5       �� DisplayMemberPath$ Category��5      ��5      �� *
Products+5      5      5      &   System.Windows.Controls.DataGrid  5   
   -   5        $��
dgProducts��5      ��5       ��  �� ElementName$ UserControl��$ DataGridHeight���5!      �� $ lbCategories��$ SelectedValue�   Columns 5"      0  *System.Windows.Controls.DataGridTextColumn  5#      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ 30 6%   	   Header$	 Id��6P   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 61   �� *
Product.Id+6P     5$      $ * 6%   $
	 Name��6P   
 6/   �� *Product.Name+6P     5%      $ * 6%   $	 Price��6n   
 6/   �� *Product.Price.Out+ �� StringFormat$ 	{0:N0} đ��6n     5&      $	 100 6%   $	 Producer��6V   
 61   �� *Product.Producer+6V     5'      $ 90 6%   $	 Quantity��6R   
 60   �� *TotalQuantity+6R   5)      5*      5+      