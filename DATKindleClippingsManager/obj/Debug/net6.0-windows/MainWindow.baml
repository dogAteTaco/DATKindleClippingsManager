   M S B A M L   `   `   `  ����T  PDATKindleClippingsManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:DATKindleClippingsManagerDATKindleClippingsManager  5   	   *    $DATKindleClippingsManager.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   3local'clr-namespace:DATKindleClippingsManager  5   	     � Title$"  DAT Kindle Clippings Manager��5	   	   $	��720��6.   $
��1280��6;   $��14(�6H   .��5
      � ���#131517 � ColumnDefinitions 5   
   �� 5      $	��330 �6   6-   �� 5      6!    � RowDefinitions 5   
   �� 5      $��50 �6   6*   �� 5      6   �� 5      $��50 �6   6*   .+�5   
   � $��2��6   ���#101113.+�5      �� $��Center=�6    �� Orientation$ 
Horizontal=�64   .+�5      �� $��Center=�6   
�������.��6F   File6L   �� 5      $��Center=�6   $	��500��65   $��25��6A   $��5,5,10,5q�6M    �� 
IsReadOnly$
 True��6_   ��6q   �� *ClippingsFile+6�   �� 5      $��Center=�6   $��50��65   $��30��6@   
������ $��0q�6e   ��6y   �� *SelectClippingsFileCommand+.��5      �� $/��)pack://application:,,,/Resources/open.png��6   6Q    � ToolTip$@ :Select the location of your Clippings file on your Kindle.��5   &   5      �� 5      $��Center=�6   $��50��65   $��30��6@   
������ $��0q�6e   $��5,0,0,0q�6�   ��6y   �� *LoadClippingsCommand+.��5      �� $2��,pack://application:,,,/Resources/refresh.png��6   6T   $) #Refresh Clippings from your Kindle.��5"   &   5#      5$      5%      �� 5'   
   $��1��6|   ���#101113���#131517��6   �� *Books+ �� SelectedItem 61   �� *SelectedBook+ �� UpdateSourceTrigger$ PropertyChanged=�*  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5(      +  %Microsoft.Xaml.Behaviors.EventTrigger  5)      	   	EventName$	 SelectionChanged��6!   *  $Microsoft.Xaml.Behaviors.TriggerBase
   Actions.
 5*      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     PassEventArgsToCommand$
 True��6T      Command 6,   �� *BookSelectedCommand+6T   5+       � ContextMenu 5-      �� 5.      .@�5/      m� $��Paste Cover��6    m� Command 64   �� *PasteCoverCommand+m� 50      $��Select Cover...��6    68   �� *SelectCoverCommand+51      ��53      �� 54      .R�55      � 
 � Tag 6   ��  56      �� 57      $	��120 �6/   6=   �� 58      $	��150 �6/   6=    5:      �� 5;      $��* �6,   69   .+�5=      �� $��5,5,5,5q�6    $��2��61   $��Uniform=�6B   $	��150��6T    J� 	CacheMode-  'System.Windows.Media.CacheModeConverter$ BitmapCache 6�   ��6a   �� *CoverLocation+6�   �� 5>      $��1��6%   .+�5?      �� $��Italic$�6)   $��DemiBold"�6<   $
��Wrap=�6R   $
��Left=�6f   $	��Top=�6�   
���������6�   �� *Title+6�   �� 5@      $
��Left=�6(   
��������� 5A   "   $	�by ��6&    �� 5B   "   $��Italic$�6&   �69   �� *Author+5C      5D      5F      5G      5I      �� 5J   
   $��1��6=   $��1��6M   ���#101113���#131517��6   �� *FilteredClippings+��5K      �� 5L      .R�5M      �  5N      �� 5O      $��* �6,   69    5Q      �� 5R      $	��150 �6/   6=   �� 5S      $	��350 �6/   6=   �� 5T      $	��350 �6/   6=   .+�5V      �� $	��700��6%   .+�5W      �� $��20��6(   $��0��64   $	��350��6A   $
��Left=�6M   $	��Top=�6h   
��������� 5X   "   $��DemiBold"�6&   $�Page: ��6<    �� 5Y   "   �6&   �� *Page+5Z      �� 5[      $
��Left=�6(   $	��350��6C   $	��Top=�6O   
��������� 5\   "   $��DemiBold"�6&   $�
Location: ��6<    �� 5]   "   �6&   �� *StartingLocation+ �� 5^   "   $�-��6&    �� 5_   "   �6&   �� *EndingLocation+5`      5a      �� 5d      $��1��6"   $��2��62   $��Italic$�6C    �� TextWrapping$
 Wrap=�5e   "   $	��350��66   $	��100��6B   $
 True��6R   ���#131517���#131517
�������$��3,3,3,3q�5g   5   ��5f   "   �� *Text+5g   5   �� 5h      -   5        $��2��5h   "   $��2��62   $
 Wrap=�6C   $	��350��6W   $	��100��6c   $
 True��6s   ���#131517
����������#131517$��3,3,3,3q�5j   "   ��5i   "   �� *	Note.Text+5j   3   5k      5l      5n      �� 5o   
   $��2��6   $ 
Horizontal=�6"   $��2��6;   $
��Left=�6O   .+�5p      �� $��5,5,5,0q�6   $��50��6&   $��35��61   
������ $��0q�6V   ��6j   �� *BackUpClippingsCommand+.��5q      �� $3��-pack://application:,,,/Resources/download.png��6   6Q   $4 .BackUp your Clippings from your Kindle device.��5t   "   5u      �� 5v      $��5,5,5,0q�6   $��50��6&   $��35��61   
������ $��0q�6V   ��6j   �� *OpenBackUpsCommand+.��5w      �� $8��2pack://application:,,,/Resources/folder-backup.png��6   6V   $* $Open BackUp Folder in your computer.��5z   "   5{      �� 5|      $y�Hidden=�6   $��Right=�6)   $��5,5,5,0q�6E   $��50��6V   $��35��6a   
������ $��0q�6�   ��6�   �� *SaveClippingsCommand+.��5}      �� $:��4pack://application:,,,/Resources/tick-green-icon.png��6   6X   5~      5      5�      5�      