   M S B A M L   `   `   `  ����L  HPolygonDrawWpfApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:PolygonDrawWpfAppPolygonDrawWpfApp  5   	   "    PolygonDrawWpfApp.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   1localclr-namespace:PolygonDrawWpfApp     5   	     � Title$  
MainWindow��5   	   $	��600��6   $	��900��6)   .��5	      �  � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��2��6   $��2��6)   .+�5      �� -   5         ��Name$ 
freeCanvas��5      �� 5      -   5         ��Name$ NextPol_btn��5      
����ح�
���   �$��1q�5   )    �� 	ClickMode$ Press=�5   3   $��45��6E   $��Next Polygon��6Q   $
��Left=�6h   $	��126��6�   $��	766,0,0,0q�6�   �� 5      -   5         ��Name$ namePolygon��5      $��56��6+   $��	766,0,0,0q�67   5      5      5      