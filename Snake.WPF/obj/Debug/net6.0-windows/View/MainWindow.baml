   M S B A M L   `   `   `  ����D  @Snake.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:Snake.WPF	Snake.WPF  5   	       Snake.WPF.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:Snake.WPF     5   	     � Title$  Snake��5   	   $	��525��6   $	��575��6$    J� InputBindings 5	      �� 5
   
   
 �� Key$ A��69    �� Command 6   �� *TurnLeftCommand+69   �� 5   
   $
 Left��69    6   �� *TurnLeftCommand+69   �� 5   
   $ D��6:    6   �� *TurnRightCommand+6:   �� 5   
   $ Right��6:    6   �� *TurnRightCommand+6:   �� 5   
   $	 Tab��66    6   �� *PauseCommand+66   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
   o� $��0��6   .@�5      m� $
��File��6   .@�5      m� $��Új játék betöltése��6    m� Command 67   �� *NewGameCommand+m� 5      $��	Kilépés��6    6,   �� *ExitCommand+5      5      �� 5   
   $��1��6   
���   ���6$   �� *
MapTiles+��5      �� 5      .R�5      A�  A� Rows 6"   �� *	MapSize+ A� Columns 6;   �� *	MapSize+5       ��5"      �� 5#      .R�5$      �� $|�False��6    $
{�True��62   $��1,1,1,1q�6C   z�5%      �� 5&       �� ScaleX$ 1Q�6-   	 �� ScaleY$	 1Q�68   ��5(      �� 5)      
 �� 
TargetType"
 ��6$   .�5*   "   ��  �� Property � �� �� Value 6?   �� *Color+5+      5-      5.      50      51      52      