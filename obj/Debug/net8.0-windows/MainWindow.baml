   M S B A M L   `   `   `  ����A  =Tetris, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:TetrisTetris  5   	       Tetris.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:Tetris     5   	   -   5          � Title$  Tetris��5	   	   $	��600��6   $	��800��6%   $	��600��5
   	   $	��600��6   
�������$��Segoe UI Light)�5   	   $��28(�6%   .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $
��auto �6   �� 5      $��* �6   ��5#   
   �� 5$       �� ImageSource$ Assets/Background.png��6   .+�5(   
   (� $��1��6   $��1��5)      $��0,0,0,20q�5*      .��5+      �� -   5         ��Name$ 
GameCanvas��5+      $��1��5,      $��1��5-      ���#101010$	��250��5/      $	��510��50      $
}�True��51      52      53      �� 57   
   -   5         ��Name$ 	ScoreText��57      $��0��58      $��1��59      $��Score: ��5:      $��0, 10q�5;       �� TextAlignment$ Center=�5<      �� 5?   
   $��1��6   $��0��5@      $��Center=�5A      $��Right=�5B      .+�5C      �� $
��Hold��6   $ Center=�5D      �� 5E      -   5         ��Name$ 	HoldImage��5E      $��20q�5F      $	��125��5G      5H      �� 5K   
   $��1��6   $��2��5L      $��Center=�5M      $
��Left=�5N      .+�5O      �� $
��Next��6   $ Center=�5P      �� 5Q      -   5        $ 	NextImage��5Q      $��20q�5R      $	��125��5S      5T      � 5W   
   -   5         �Name$ GameOverMenu��5W      ���	#CC000000$��2��5Y      $��3��5Z      $y�Hidden=�5[      .+�5\      �� $��Center=�6   $��Center=�5]      .+�5^      �� $��	Game Over��6   $��48(�5_      $ Center=�5`      �� 5a      -   5        $ FinalScoreText��5a      $��Score: ��5b      $��36(�5c      $ Center=�5d      �� 5e      -   5        $��
Play Again��5e      
������$��0,20,0,0q�5g      $��5q�5h      5i      5j      5k      5l      5m      