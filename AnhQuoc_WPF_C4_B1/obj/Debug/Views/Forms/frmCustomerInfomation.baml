   M S B A M L   `   `   `  ����L  HAnhQuoc_WPF_C4_B1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:AnhQuoc_WPF_C4_B1AnhQuoc_WPF_C4_B1  5   	   -    'AnhQuoc_WPF_C4_B1.frmCustomerInfomation   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:AnhQuoc_WPF_C4_B1     5   	     � WindowStyle$
  None=�5   	    � WindowStartupLocation$ CenterScreen=�5	   	    � Title$ frmCustomerInfomation��5
   	   $��536.087��6'   $	��600��68   .��5      �� 
���   �$��2q�5      ._�5   
   �� .+�5      ��    	lblHeader#��[  6   $��5q�5      $��	0 30 0 30q�6    .��63   Customer Infomation6H   �� 5      $��
60 0 60 10q�6   $��0 0 0 1q�5      
���   �._�5      � $��	0 10 0 10q�6    � ColumnDefinitions 5      �� 5      $��1* �6+   �� 5      $��2* �6+   .+�5      ��   	lblStyle1#��[ 6   $
��Left=�5      $��20 5 5 5q�5      .��6/   
Id Card:69   �� 5      $��1��6   #��[ 5      $
��Left=�5      $��5q�5      ��5      �� *newItem.IDCard+5       5!      �� 5"      $��60 10 60 10q�6   $��0 0 0 1q�5#      
���   �._�5%      � $
��0 10q�6    5&      �� 5'      $��1* �6+   �� 5(      $��2* �6+   .+�5*      �� #��[ 6   $
��Left=�5+      $��20 5 5 5q�5,      .��6/   Name:66   �� 5-      $��1��6   #��[ 5.      $��5q�5/      $
��Left=�50      ��51      �� *newItem.Name+52      53      �� 54      $��60 10 60 10q�6   $��0 0 0 1q�55      
���   �._�57      � $
��0 10q�6    58      �� 59      $��1* �6+   �� 5:      $��2* �6+   .+�5<      �� #��[ 6   $
��Left=�5=      $��20 5 5 5q�5>      .��6/   Phone number:6>   �� 5?      $��1��6   #��[ 5@      $��5q�5A      $
��Left=�5B      ��5C      �� *newItem.Phone+5D      5E      �� 5F      $��60 10 60 10q�6   $��0 0 0 1q�5G      
���   �._�5I      � $
��0 10q�6    5J      �� 5K      $��1* �6+   �� 5L      $��2* �6+   .+�5N      �� #��[ 6   $
��Left=�5O      $��20 5 5 5q�5P      .��6/   Point:67   �� 5Q      $��1��6   #��[ 5R      $��5q�5S      $
��Left=�5T      ��5U      �� *newItem.Point+ �� StringFormat$ {0:N0}��5V      5W      �� 5X      $��60 10 60 10q�6   $��0 0 0 1q�5Y      
���   �._�5[      � $
��0 10q�6    5\      �� 5]      $��1* �6+   �� 5^      $��2* �6+   .+�5`      �� #��[ 6   $
��Left=�5a      $��20 5 5 5q�5b      .��6/   Card:66   �� 5c      $��1��6   #��[ 5d      $��5q�5e      $
��Left=�5f      ��5g      �� *newItem.Card+5h      5i      �� 5j      $��
60 30 60 0q�6   .+�5k      �� -   5        $��btnBack��5k        btnBack#��[ 5l      $	��100��5m      $
��Back��5n      5o      5p      5q      5r      