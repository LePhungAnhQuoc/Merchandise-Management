   M S B A M L   `   `   `  ����L  HAnhQuoc_WPF_C4_B1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:AnhQuoc_WPF_C4_B1AnhQuoc_WPF_C4_B1  5   	        AnhQuoc_WPF_C4_B1.frmAdmin   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:AnhQuoc_WPF_C4_B1     5   	     � Title$  
Admin View��5   	   $
��1250��5	   	   $	��700��5
   	    � WindowStartupLocation$ CenterScreen=�5   	   .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   .+�5   
   � $��0��6    � ColumnDefinitions 5      �� 5      $
��auto �6#   �� 5      $��* �6#   .+�5      �  5      �� 5      $
��auto �6$   �� 5      $��* �6$   .+�5      �� $��0��6   $��0 0 0 0q�5      ��5      ��  �� ElementName$ tvDashBoard�� �� Path$ 
Background�.+�5      A� $��Center=�6   $��	2 30 2 10q�5       $��45��5!      $��45��6)    �� Stretch$ UniformToFill=�5"      
�������$��2��5#   -   ��5$      �� 5%       �� ImageSource$ /Assets/1.png��6)   5'      �� 5(         	lblHeader#��[  6   $��20(�5)      $��0 0 0 10q�5*      ��5+      ��  �� RelativeSource �� *FindAncestor+	 �� AncestorType"	 �$ Account.Username�5,      5-      R� 5.      -   5        $��tvDashBoard��50      $��1��5.      $��0q�5/        TreeViewItemStyle1#��[ 51      .@�52      Q� -   5        $��
tvHomePage��52   #   $��	Home Page��53   #   

 � Tag$8
 2/AnhQuoc_WPF_C4_B1;component/Assets/Icons/home.png��54   #   55   #   Q� 56      -   5        $��	tvProduct��56   #   $��Product��57   #   $;
 5/AnhQuoc_WPF_C4_B1;component/Assets/Icons/product.png��58   #   59   #   Q� 5:      -   5        $��tvInventory��5:   #   $��	Inventory��5;   #   $=
 7/AnhQuoc_WPF_C4_B1;component/Assets/Icons/inventory.png��5<   #   5=   #   Q� 5>      -   5        $��tvInventoryStatus��5>   #   $��Inventory Status��5?   #   $=
 7/AnhQuoc_WPF_C4_B1;component/Assets/Icons/inventory.png��5@   #   5A   #   Q� 5B      -   5        $��tvRevenueProfit��5B   #   $;
 5/AnhQuoc_WPF_C4_B1;component/Assets/Icons/revenue.png��6:   $��Revenue Profit��6v   6�   Q� 5C      -   5        $��tvLogOut��5C   #   $��Log Out��5D   #   $;
 5/AnhQuoc_WPF_C4_B1;component/Assets/Icons/log-out.png��5E   #   5F   #   5G      5H      � 5I      -   5        $��gdView��5J      $��1��5I      5K      5L      5M      5N      