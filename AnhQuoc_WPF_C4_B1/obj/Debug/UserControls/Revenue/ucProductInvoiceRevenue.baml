   M S B A M L   `   `   `  ����L  HAnhQuoc_WPF_C4_B1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:AnhQuoc_WPF_C4_B1AnhQuoc_WPF_C4_B1  5      /    )AnhQuoc_WPF_C4_B1.ucProductInvoiceRevenue   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:AnhQuoc_WPF_C4_B1     5        � 	Resources  5	      .  (AnhQuoc_WPF_C4_B1.NoteEmptyListConverter   NoteEmptyListConverter)   #AnhQuoc_WPF_C4_B1.CurrencyConverter  CurrencyConverter��%:   &	        &	        5
   
   6'     5   
   6"   5      .��5      �� .+�5   
   �� -   5        $��lblHeaderUI��5        	lblHeader#��[ 5      $��0 0 0 20q�5      ��5      ��  �� RelativeSource �� *FindAncestor+ �� AncestorType$ UserControlL� �� Path$ 	lblHeader��� 5   
   $��1 1 1 0q�6   
���   �$��0 20 0 0q�5      ._�5      � $	��0 0q�6    � ColumnDefinitions 5      �� 5      $	��100 �6'   �� 5      $��1* �6'   �� 5      $
��auto �6'   .+�5      ��   	lblStyle1#��[ 6   $��5q�5      .��6$   Date:6+   �� 5      $��1��6   
�������.+�5!      (  "System.Windows.Controls.DatePicker  -   5        $��	dateOrder��5!   !   $��1 0 0 0q�5"   !     
dateStyle1#��[ 5#   !   $��5q�5$   !      SelectedDate 5%   !   �� *SelectedDate+5&      �� 5'      $��2��6   $��0 0 0 0q�5(       �� Orientation$ 
Horizontal=�5)      $��Center=�5*      .+�5+      �� -   5        $��btnClear��5+        btnClear#��[ 5,      $��Clear��5-      5.      5/      50      �� 53   
   
���   �$��	0 0.5 0 0q�54      ._�55      �� #��� ��6   .+�56      �� -   5        $��lbCategories��56        listBoxHorizontal#��[ 57      $��Stretch=�59       �� SelectedValuePath$ Products��5:       �� DisplayMemberPath$ Category��5;      ��58      �� *
Products+5;      5<      5=      &   System.Windows.Controls.DataGrid  5?   
   -   5        $��
dgProducts��5?      ��5@      �� 	 �� ElementName$	 lbCategories��$ SelectedValue�
 �� SelectedItem
 5A      �� *	Product+   Columns 5B      0  *System.Windows.Controls.DataGridTextColumn  5C      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ 30 6%      Header$ Id��6P   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *
Product.Id+6P     5D      $ * 6%   $
 Name��6P    6/   �� *Product.Name+6P     5E      $	 100 6%   $ Producer��6V    61   �� *Product.Producer+6V     5G      $ 90 6%   $ Quantity��6R    60   �� *TotalQuantity+6R   5J      �� 5M   
   $��0 10 0 0q�6   .+�5N      � $
��10 0q�6    5O      �� 5P      $	��100 �6'   �� 5Q      $��1* �6'   .+�5S      �� $��15(�6   $��5q�5T      .��6$   
Revenue:6.   �� 5U      $��1��6   $��15(�5V      $��5q�5W      ��5X      �� *	Revenue+ �� 	Converter# [ 5Y      � 5Z      $
��10 0q�6    5[      �� 5\      $	��100 �6'   �� 5]      $��1* �6'   .+�5_      �� $��15(�6   $��5q�5`      
��� � �.��5a   +   	Profit:64   �� 5b      $��1��6   $��15(�5c      $��5q�5d      
��� � ���5f      �� *Profit+# [ 5g      5h      5i      5j      