using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using System.Security;
using System.Text.RegularExpressions; 


namespace SpriteMapGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "image"; // Default file name 
            dlg.DefaultExt = ".png"; // Default file extension 
            dlg.Filter = "Image Files (*.png;*.BMP;*.JPG;*.GIF)|*.png;*.BMP;*.JPG;*.GIF"; // Filter files by extension 

            dlg.Multiselect = true;
            // Show open file dialog box 
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // grab the file name of the image 
                string[] filename = dlg.FileNames;

                // and load the image 
               
                canvas1.LoadImage(filename[0]); // ... magics 
                canvas1.GenSheet(filename); 
            }

        }
        public MainWindow()
        {
            InitializeComponent();

        }
    }
}
