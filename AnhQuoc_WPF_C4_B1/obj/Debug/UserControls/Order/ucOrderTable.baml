   M S B A M L   `   `   `  ����L  HAnhQuoc_WPF_C4_B1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:AnhQuoc_WPF_C4_B1AnhQuoc_WPF_C4_B1  5      $    AnhQuoc_WPF_C4_B1.ucOrderTable   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:AnhQuoc_WPF_C4_B1     5        � 	Resources  5	      .  (AnhQuoc_WPF_C4_B1.NoteEmptyListConverter   NoteEmptyListConverter��%   &	          5
   
   6'   5      .��5      �� .+�5   
   �� -   5        $��lblHeaderUI��5        	lblHeader#��[ 5      $��0 0 0 20q�5      ��5      ��  �� RelativeSource �� *FindAncestor+ �� AncestorType$ UserControlL� �� Path$ 	lblHeader��� 5   
   $��1 1 1 0q�6   
���   �$��0 20 0 0q�5      ._�5      � $	��0 0q�6    � ColumnDefinitions 5      �� 5      $	��100 �6'   �� 5      $��1* �6'   �� 5      $
��auto �6'   .+�5      ��   	lblStyle1#��[ 6   .��6;   Date:6B   �� 5      $��1��6   
�������.+�5      (  "System.Windows.Controls.DatePicker  -   5        $��	dateOrder��5   !   $��1 0 0 0q�5   !     
dateStyle1#��[ 5    !   $��5q�5!   !      SelectedDate 5"   !   �� *SelectedDate+5#      �� 5$      $��2��6   $��0 0 0 0q�5%       �� Orientation$ 
Horizontal=�5&      $��Center=�5'      .+�5(      �� -   5        $��btnClear��5(        btnClear#��[ 5)      $��Clear��5*      5+      5,      5-      &   System.Windows.Controls.DataGrid  5/   
   -   5        $��dgOrder��5/      $	��600��50         	RowHeight$ 50Q�51         Columns 52      0  *System.Windows.Controls.DataGridTextColumn  53      ,  &System.Windows.Controls.DataGridColumn	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 80 6%   
   Header$
 Id��6H   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *Id+6H     54      $	 * 6%   $
 
UserCreate��6T    6/   �� *UserCreated.Name+6T     55      $	 * 6%   $
 Customer��6Q    6/   �� *Customer.Name+6Q     56      $	 80 6%   $
 Total Quantity��6R    60   �� *TotalQuantity+6R     57      $		 100 6%   $
 Total Price��6m    61   �� *TotalPrice.Out+ �� StringFormat$ 	{0:N0} đ��6m     58      $	 80 6%   $

 Date��6j    60   �� *Date+$ {0:dd/MM/yyyy}��6j   4  .System.Windows.Controls.DataGridTemplateColumn  59      $
	 auto 6)      CellTemplate 5:      �� 5;      .R�5<      �� $��
btnDetails��6%   -   5        $��Details��5=   %     
btnDetails#��[ 5>   %   5?   @   5@      5B      5D      5E      5F      