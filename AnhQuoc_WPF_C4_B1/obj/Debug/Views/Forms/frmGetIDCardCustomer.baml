   M S B A M L   `   `   `  ����L  HAnhQuoc_WPF_C4_B1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:AnhQuoc_WPF_C4_B1AnhQuoc_WPF_C4_B1  5   	   ,    &AnhQuoc_WPF_C4_B1.frmGetIDCardCustomer   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:AnhQuoc_WPF_C4_B1     5   	     � WindowStyle$
  None=�5   	    � WindowStartupLocation$ CenterScreen=�5	   	    � Title$ frmGetIDCardCustomer��5
   	   $��125.9��6&   $��320.391��65   .��5      �� .+�5   
   � $��
10 20 10 0q�6    � ColumnDefinitions 5      �� 5      $	��130 �6#   �� 5      $��1* �6#   .+�5      ��    	lblStyle1#��[  6   $��5q�5      $��0 0 0 0q�6    .��61   Customer ID card:6D   � 5      $��1��6   .+�5      �� -   5        $��	txtIdCard��5        	txtStyle1#��[ 5      $��5q�5        txtErrorTemplate3 =� ErrorTemplate# [ 5      .��6X   ��5      �� 5       �� Path$ txtIDCardInput�6"    �� UpdateSourceTrigger$ PropertyChanged=�68    �� ValidationRules 5      "   AnhQuoc_WPF_C4_B1.NumberRule  5   "      
IsCanEmpty . 63   5      .��5   $   5      �� 5       $$��Hit enter to create new guest!��6   $��Center=�5!      $��10 0 0 0q�5"      $
��Left=�5#      ��5$      �� 5%      	 �� 
TargetType"	 ��6      tblPlaceHolder
 �� BasedOn#
 [ 5&       .�5'      ��  �� Property � y� �� Value$ Hidden=�6;   �� 5(       � ��
 �����6;   �� 5)       � ��$ Italic$�6:    �� Triggers 5*      �� 5+   "    �� Binding 6.   ��  �� ElementName$ 	txtIdCard��$
 Text� �� Value  6c   .a�5,   &   ��  � y�$ Visible=�6C   5-   #   5/      51      52      �� 53      $��2��6   $��10 0 0 0q�54       �� Orientation$ 
Horizontal=�55      $��Center=�56      57      58      �� 59   
   $��	0 20 0 10q�6   $��Bottom=�5:      $ 
Horizontal=�5;      $��Center=�5<      .+�5=      �� -   5        $��
btnConfirm��5=        
btnConfirm#��[ 5>      $��Confirm��5?      $��90��5@      $y�	Collapsed=�5A      �� 5B      -   5        $��btnNewGuest��5B        	btnCreate#��[ 5C      $	��8 5q�5D      $��90��5E      $��	New Guest��5F      5G      5H      5I      