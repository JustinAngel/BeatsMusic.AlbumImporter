   M S B A M L   `   `   `  ����  BeatsMusic.Importer#    BeatsMusic.Importer.RdioLogin   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5
      � $��20,0,0,0q�6     � RowDefinitions  5   
   �� 5      $	��196 �6   �� 5       � ColumnDefinitions 5   
   �� 5      $��329.566 �6   �� 5      $��* �6   .+�5   
   �� $��10,63,11.5,8q�6   $��Resources/Rdio.png��6&   $
��Fill=�6B   �� 5   
   $
��Left=�6   $��54��6/   $��	10,10,0,0q�6;   $
��Wrap=�6N   $��Enter your Rdio pin:��6b   $	��Top=�6~   $��309.566��6�   $��1��6�   $��34.667(�6�   �� 5   
   -   5         ��Name$ txtPin��5      $
��Left=�6#   $��45��6>   $��	10,64,0,0q�6J   $��1��6]    �� TextWrapping$
 Wrap=�6j   $	��Top=�6~   $��258.305��6�   $��32(�6�   �� 5   
   -   5        $��OK��5      $
��Left=�6    $��45��6;   $��283.805,64,0,0q�6G   $��1��6_   $	��Top=�6l   $��35.761��6�   6�   � 5   
   -   5         �Name$ 	grdAlbums��5      $��8.5,203,11.5,25q�6#   $��1��6<    5      �� 5      $��0* �6#   �� 5      $��0* �6#   �� 5      .+�5      � $
��Left=�6   $��26��65   $��2,54,-0.434,0q�6A   $	��Top=�6X   $	��308��6p    � IsIndeterminate . $��3��6�   �� 5      $
��Left=�6   $��54��63   $
��Wrap=�6?   $��Fetching albums...��6S   $	��Top=�6m   $	��310��6�   $��34.667(�6�   $��3��6�   $��0,0,-0.434,0q�6�   5       (  "System.Windows.Controls.WebBrowser  5!   
   -   5          Name$ 
webBrowser��5!      $��1��6)   $��2��69   $��62,0,0,0q�6J   5#      5$      