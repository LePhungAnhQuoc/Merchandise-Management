   M S B A M L   `   `   `  ����L  HAnhQuoc_WPF_C4_B1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:AnhQuoc_WPF_C4_B1AnhQuoc_WPF_C4_B1  5      )    #AnhQuoc_WPF_C4_B1.ucReceiptTableAll   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:AnhQuoc_WPF_C4_B1     5        � 	Resources  5	      .  (AnhQuoc_WPF_C4_B1.NoteEmptyListConverter   NoteEmptyListConverter��%   &	          5
   
   6'   5      .��5      �� .+�5   
   �� $��14(�6   $
��Bold"�5      $��5q�5      $��0 0 0 10q�6   $��Center=�5      .��6-   Receipts Table6=   &   System.Windows.Controls.DataGrid  5   
   -   5        $��	dgReceipt��5      $��Stretch=�5         
IsReadOnly . $	��200��5      ��.    AutoGenerateColumns .     RowStyle 5      �� 5       �� 
TargetType)  #System.Windows.Controls.DataGridRow"  6   .�5      ��  �� Property � �� �� Value$ Center=�6A   5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ 80 6%   	   Header$	 Id��6H   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 61   �� *Id+6H     5      $ * 6%   $	 
UserCreate��6O   
 61   �� *	User.Name+6O     5      $ 80 6%   $	 TotalQuantity��6S   
 61   �� *TotalQuantity+6S     5       $	 100 6%   $	 TotalPriceIn��6l   
 61   �� *TotalPrice.In+ �� StringFormat$ 	{0:N0} đ��6l     5!      $	 100 6%   $	 TotalPriceOut��6m   
 61   �� *TotalPrice.Out+$ 	{0:N0} đ��6m     5"      $ 80 6%   $
	 Date��6k   
 61   �� *Date+$ {0:dd/MM/yyyy}��6k   5$      �� 5&   
   -   5        $��lblNoteEmptyList��5&      
���  ��$��12(�5(      $��Italic$�5)      $��Normal"�5*      $��5q�5+      $��0 5 0 20q�6   $��Center=�5,      $��Center=�5-      ��5.      �� *ReceiptSource.Count+ �� 	Converter# [  �� 51   
   -   5        $��scrollDetails��51      $	��400��52      $��Stretch=�53      $
��Auto=�54      ��. 56      57      58      