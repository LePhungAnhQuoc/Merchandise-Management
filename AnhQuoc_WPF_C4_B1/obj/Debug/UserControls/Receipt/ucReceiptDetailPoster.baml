   M S B A M L   `   `   `  ����L  HAnhQuoc_WPF_C4_B1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:AnhQuoc_WPF_C4_B1AnhQuoc_WPF_C4_B1  5      -    'AnhQuoc_WPF_C4_B1.ucReceiptDetailPoster   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:AnhQuoc_WPF_C4_B1     5      -   5        $��userControl��5      $
��auto��5	      $	��250��6   .��5
      �� 
���   �$��1q�5        �� CornerRadius$  5��5      ._�5   
   �  � RowDefinitions 5      �� 5      $
��auto �6    �� 5      $
��auto �6    �� 5      $
��auto �6    .+�5      �� $��0��6   
���   �$��0 0 0 1q�5      $  0��5      
�������._�5      ��  �� Orientation$ 
Horizontal=�6   $��Center=�5      $��Center=�6:   .+�5      �� $��14(�6   $��SemiBold"�5      $��
10 10 0 10q�5      
���������5      ��  �� Path$ Item.IdReceipt� �� ElementName$ userControl���� 5      $��14(�6   $��SemiBold"�5       $��
0 10 10 10q�5!      
���������5#      �� 5$      ��5%   "   `� 5&   &    �� StringFormat$
 .{0}��63   .1�5'   *   �� $ NoIndex�62   $ userControl��6A   5(   '   5*      5,      5-      5.      �� 5/      $��1��6    � 	Resources 50       �� 
TargetType   	tblStyle1 �� BasedOn	 �� Property
 �� Value��%�   '��       7   �� 51      " ��6   8   .�53      �� 	 �@��Padding$
 0q�64   �� 54      	 � ��$
 
10 10 10 0q�63   �� 55      	 � ��$

 Left=�6@   56      57      .+�58      �� ��59      `� 5:      $ Product Name: {0}��6'   .1�5;      �� $ Item.Product.Name�6&   $ userControl��6?   5<      5>      �� 5?      ��5@      `� 5A      $ Quantity: {0:N0}��6'   .1�5B      �� $ Item.Quantity�6&   $ userControl��6;   5C      5E      �� 5F      $��10q�6   ��5G      `� 5H      $ Price: {0:N0} đ��6'   .1�5I      �� $ Item.TotalPrice.In�6&   $ userControl��6@   5J      5L      5M      �� 5N      -   5        $��stkDeleteButton��5N      $��2��5O      .+�5P      �� -   5        $��	btnDelete��5P        	btnDelete#��[ 5Q      $
��auto��5R      $��20 10q�5S      .��6(   Delete60   5T      5U      5V      5W      