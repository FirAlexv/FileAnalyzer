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
        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        public string[] arrayDelCopyFile;

        public MainWindow()
        {
            InitializeComponent();

            textBoxPathCopy.Visibility = Visibility.Hidden;
            buttonPathCopy.Visibility = Visibility.Hidden;
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
            else if ((bool)checkBoxCopy.IsChecked && textBoxPathCopy.Text == "")
            {
                System.Windows.MessageBox.Show("Нет пути для копирования файлов, определите целевую папку!", "Нет пути!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string[] arrayFiles = Directory.GetFiles(textBoxPath.Text);
                arrayDelCopyFile = new string[arrayFiles.Length];
                progressBar.Maximum = arrayFiles.Length;

                for (int i = 0; i < arrayFiles.Length; i++)
                {
                    fileFirst = new FileInfo(arrayFiles[i]);

                    for (int j = 0; j < arrayFiles.Length; j++)
                    {
                        fileSecond = new FileInfo(arrayFiles[j]);
                        progressBar.Value = i + 1;

                        if (i != j && i != last)
                        {
                            if (EqualsBytes(fileFirst, fileSecond))
                            {
                                counter++;
                                if ((bool)checkBoxCopy.IsChecked)
                                {
                                    arrayDelCopyFile[i] = arrayFiles[j];
                                }
                                else
                                {
                                    arrayDelCopyFile[i] = arrayFiles[j];
                                    last = j;
                                }
                            }
                        }
                    }
                }

                if (counter == 0)
                {
                    System.Windows.MessageBox.Show("Копии не найдены!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    labelCopiesFound.Content = counter;
                    if ((bool)checkBoxCopy.IsChecked)
                    {
                        for (int i = 0; i < arrayDelCopyFile.Length; i++)
                        {
                            if (arrayDelCopyFile[i] != null)
                            {
                                FileInfo fileCopy = new FileInfo(arrayDelCopyFile[i]);
                                fileCopy.MoveTo(textBoxPathCopy.Text + "\\" + fileCopy.Name);
                            }
                        }
                    }
                }
            }
        }

        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            int couter = 0;

            if (arrayDelCopyFile is null)
            {
                System.Windows.MessageBox.Show("Нет копий, не было проведено сканирование или копии не найдены!", "Нет копий!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                for (int i = 0; i < arrayDelCopyFile.Length; i++)
                {
                    if (arrayDelCopyFile[i] != null)
                    {
                        couter++;
                        File.Delete(arrayDelCopyFile[i]);
                    }
                }
                labelCopiesRemoved.Content = couter;
            }
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

        private void ButtonPathCopy_Click(object sender, RoutedEventArgs e)
        {
            folderBrowserDialog.ShowDialog();

            textBoxPathCopy.Text = folderBrowserDialog.SelectedPath;
        }

        private void CheckBoxCopy_Checked(object sender, RoutedEventArgs e)
        {
            textBoxPathCopy.Visibility = Visibility.Visible;
            buttonPathCopy.Visibility = Visibility.Visible;
            buttonDel.Visibility = Visibility.Hidden;

            //todo Баг интерфейса
            //labelCopiesRemovedContent.Content = "Перемещено копий:"; 
        }

        private void CheckBoxCopy_Unchecked(object sender, RoutedEventArgs e)
        {
            textBoxPathCopy.Visibility = Visibility.Hidden;
            buttonPathCopy.Visibility = Visibility.Hidden;
            buttonDel.Visibility = Visibility.Visible;

            labelCopiesRemovedContent.Content = "Удалено копий:";
        }

        private void ButtonPath_Click(object sender, RoutedEventArgs e)
        {
            folderBrowserDialog.ShowDialog();

            textBoxPath.Text = folderBrowserDialog.SelectedPath;
        }

        private void ButtonCansel_Click(object sender, RoutedEventArgs e)
        {

            textBoxPath.Clear();
            textBoxPathCopy.Clear();

            labelCopiesFound.Content = "0";
            labelCopiesRemoved.Content = "0";

            progressBar.Value = 0;
        }
    }
}
