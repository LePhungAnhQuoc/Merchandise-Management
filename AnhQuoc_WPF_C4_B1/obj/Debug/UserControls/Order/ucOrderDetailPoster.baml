   M S B A M L   `   `   `  ����L  HAnhQuoc_WPF_C4_B1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:AnhQuoc_WPF_C4_B1AnhQuoc_WPF_C4_B1  5      +    %AnhQuoc_WPF_C4_B1.ucOrderDetailPoster   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      1localclr-namespace:AnhQuoc_WPF_C4_B1     5      -   5        $��userControl��5      $
��auto��5	      $	��270��6   .��5
      �� 
���   �$��1q�5        �� CornerRadius$  5��5      ._�5   
   �  � RowDefinitions 5      �� 5      $��40 �6    �� 5      $
��auto �6    �� 5      $
��auto �6    .+�5      �� $��0��6   
���   �$��0 0 0 1q�5      $  0��5      
�������._�5      ��  �� Orientation$ 
Horizontal=�6   $��Center=�5      $��Center=�6:   .+�5      �� $��14(�6   $��SemiBold"�5      $��
10 10 0 10q�5      
���������5      ��  �� Path$ Item.IdOrder� �� ElementName$ userControl���� 5      $��14(�6   $��SemiBold"�5       $��
0 10 10 10q�5!      
���������5#      �� 5$      ��5%   "   `� 5&   &    �� StringFormat$
 .{0}��63   .1�5'   *   �� $ NoIndex�62   $ userControl��6A   5(   '   5*      5,      5-      5.      �� 5/      $��1��6    � 	Resources 50       �� 
TargetType   	tblStyle1 �� BasedOn	 �� Property
 �� Value��%�   '��       7   �� 51      " ��6   8   .�53      �� 	 �@��Padding$
 0q�64   �� 54      	 � ��$
 
10 10 10 0q�63   �� 55      	 � ��$

 Left=�6@   56      57      .+�58      �� ��59      `� 5:      $ Product Name: {0}��6'   .1�5;      �� $ Item.Product.Name�6&   $ userControl��6?   5<      5>      �� 5?      ��5@      `� 5A      $ Quantity: {0:N0}��6'   .1�5B      �� $ Item.Quantity�6&   $ userControl��6;   5C      5E      �� 5F      ��5G      `� 5H      $ TempPrice: {0:N0} đ��6'   .1�5I      �� $ Item.TempPrice.Out�6&   $ userControl��6@   5J      5L      �� 5M      ��5N      `� 5O      $  Discount Percent: {0:N0} %��6'   .1�5P      �� $ Item.PercentDiscount�6&   $ userControl��6B   5Q      5S      �� 5T      ��5U      `� 5V      $ Discount Price: {0:N0} đ��6'   .1�5W      �� $ Item.DiscountPrice�6&   $ userControl��6@   5X      5Z      �� 5[      $��10q�6   ��5\      `� 5]      $ TotalPrice: {0:N0} đ��6'   .1�5^      �� $ Item.TotalPrice.Out�6&   $ userControl��6A   5_      5a      5b      �� 5c      -   5        $��
stkButtons��5d      $��2��5c      .+�5e      �� -   5        $��	btnDelete��5e        	btnDelete#��[ 5f      $
��auto��5g      $��20 10q�5h      $��Delete��5i      5j      5k      5l      5m      