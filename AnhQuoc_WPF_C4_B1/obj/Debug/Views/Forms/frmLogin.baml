   M S B A M L   `   `   `  ����L  HAnhQuoc_WPF_C4_B1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:AnhQuoc_WPF_C4_B1AnhQuoc_WPF_C4_B1  5   	        AnhQuoc_WPF_C4_B1.frmLogin   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:AnhQuoc_WPF_C4_B1     5   	   -   5        $��	frmLogin1��5   	   $	��450��5	   	   $��514.905��6     � WindowStartupLocation$  CenterScreen=�5   	   .��5      �� 
���   �$��1q�5      ���#d4ebff._�5   
   �� 
���   � �� CornerRadius$ 0��5      $��0q�5      $��Center=�5      $��Center=�5      ._�5      �� $	��400��6   $	��430��6%   ���#e6f3ff.+�5      �� $��Center=�6   $
��Left=�5      $
��40 0q�5      $��50(�5      $
��0 15q�5      $��-1��5      
���   �.��5   +   	Sign in64   �� 5      $��
40 20 40 0q�6   $��0q�5         	lblStyle1#��[  5       $��20(�5!      $
��Left=�5"      $��Username��5#      �� 5$      -   5        $��txtUsername��5$      $��
40 10 40 0q�5%      $��50��5&      $��10q�5'      $��0��5(      $��20(�5)        txtErrorTemplate3 =� ErrorTemplate# [ 5*      .��6X   ��5+      �� 5,       �� Path$ Account.Username�6"    �� UpdateSourceTrigger$ PropertyChanged=�6:    �� ValidationRules 5-      !   AnhQuoc_WPF_C4_B1.LoginRule  5.   "   50      .��51   $   52      �� 53      -   5        $��
lbPassword��53      #��[  54      $
��Left=�55      $��
40 20 40 0q�56      $��0q�57      $��20(�58      $��Password��59      G� 5:      -   5        $��boxPassword��5:      $��
40 10 40 0q�5;      $��50��5<      $��10q�5=      $��1��5>      $��20(�5?      5@      �� 5A      -   5        $��	btnSignIn��5A      $
��0 28q�5B      ���#007FFD
�������$��20(�5E      $��50 20q�5F      $��2��5G      $��Center=�5H      .��5I   1   	Sign in6:   5J      5K      5L      5M      