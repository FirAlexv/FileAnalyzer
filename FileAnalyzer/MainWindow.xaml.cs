using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Windows.Forms;

namespace FileAnalyzer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[] arrayDelFile;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            FileInfo fileFirst, fileSecond;
            int counter = 0;
            int last = -1;

            if (textBoxPath.Text == "")
            {
                System.Windows.MessageBox.Show("Нет пути для поиска файлов, определите целевую папку!", "Нет пути!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string[] arrayFiles = Directory.GetFiles(textBoxPath.Text);
                arrayDelFile = new string[arrayFiles.Length];

                for (int i = 0; i < arrayFiles.Length; i++)
                {
                    fileFirst = new FileInfo(arrayFiles[i]);

                    for (int j = 0; j < arrayFiles.Length; j++)
                    {
                        fileSecond = new FileInfo(arrayFiles[j]);
                        if (i != j && i != last)
                        {
                            if (EqualsBytes(fileFirst, fileSecond))
                            {
                                counter++;
                                arrayDelFile[i] = arrayFiles[j];
                                last = j;
                            }
                        }
                    }
                }
            }
            labelCopiesFound.Content = counter;
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            int couter = 0;

            if (arrayDelFile is null)
            {
                System.Windows.MessageBox.Show("Нет копий, не было проведено сканирование или копии не найдены!", "Нет копий!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                for (int i = 0; i < arrayDelFile.Length; i++)
                {
                    if (arrayDelFile[i] != null)
                    {
                        couter++;
                        File.Delete(arrayDelFile[i]);
                    }
                }
                labelCopiesRemoved.Content = couter;
            }
        }

        private void ButtonPath_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            folderBrowserDialog.ShowDialog();

            textBoxPath.Text = folderBrowserDialog.SelectedPath;
        }

        private void ButtonCansel_Click(object sender, RoutedEventArgs e)
        {
            textBoxPath.Clear();

            labelCopiesFound.Content = "0";
            labelCopiesRemoved.Content = "0";
        }

        private bool EqualsBytes(FileInfo fileFirst, FileInfo fileSecond)
        {
            byte[] fileF = File.ReadAllBytes(fileFirst.FullName);
            byte[] fileS = File.ReadAllBytes(fileSecond.FullName);

            if (fileF.Length == fileS.Length)
            {
                int counter = 0;
                for (int i = 0; i < fileF.Length; i++)
                {
                    if (fileF[i] == fileS[i])
                    {
                        counter++;
                    }
                }
                if (counter == fileS.Length)
                {
                    return true;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
