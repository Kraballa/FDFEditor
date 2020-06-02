using FDFEditor.Backend;
using FDFEditor.Control;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FDFEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string pathToData;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenAsPattern(string path, bool encrypted = true)
        {
            try
            {
                //decrypt and parse
                Stream s;
                if (encrypted)
                {
                    s = Crypt.Decrypt(path);
                }
                else
                {
                    s = new MemoryStream(File.ReadAllBytes(path));
                }
                PatternHolder pHolder = PatternHolder.Parse(s);
                TabItem tab = new TabItem();
                tab.Header = Path.GetFileName(path);
                tab.Content = new PatternTabItem(pHolder);
                MainTabControl.Items.Add(tab);
                MainTabControl.SelectedIndex = MainTabControl.Items.IndexOf(tab);
            }
            catch (Exception e)
            {
                string text = "Error. Either the file couldn't be parsed or the decryption failed.\n" + e;
                MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenAsText(string path, bool encrypted = true)
        {
            try
            {
                //decrypt and parse
                Stream s;
                if (encrypted)
                {
                    s = Crypt.Decrypt(path);
                }
                else
                {
                    s = new MemoryStream(File.ReadAllBytes(path));
                }
                TabItem tab = new TabItem();
                tab.Header = Path.GetFileName(path);
                tab.Content = new TextEditorTabItem(s);
                MainTabControl.Items.Add(tab);
                MainTabControl.SelectedIndex = MainTabControl.Items.IndexOf(tab);
            }
            catch (Exception e)
            {
                string text = "Error reading file. It was already decrypted.\n" + e;
                MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenCommand(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (pathToData != null)
                dialog.InitialDirectory = pathToData;
            dialog.Filter = "Pattern Files (b*.xna)|b*.xna";
            dialog.Multiselect = true;
            dialog.FileName = "b174.xna";
            if (dialog.ShowDialog() == true)
            {
                foreach (string path in dialog.FileNames)
                {
                    OpenAsPattern(path);
                    pathToData = Path.GetFullPath(path);
                }
            }
        }



        private void CloseCommand(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (MainTabControl.Items.Count > 0)
            {
                MainTabControl.Items.RemoveAt(MainTabControl.SelectedIndex);
            }
            else
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void ExitCommand(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void SaveCommand(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (MainTabControl.Items.Count == 0)
            {
                MessageBox.Show("No tab opened.", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string text = ((ITabItem)MainTabControl.Items[MainTabControl.SelectedIndex]).GetPlainText();
                Console.WriteLine(text);
            }
            //generate file content
            //encrypt
            //write to disk
        }

        private void ToolEncrypt(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Txt Files (*.txt)|*.txt|Any File (*.*)|*.*";
            openDialog.Title = "Select Decrypted File";
            if (openDialog.ShowDialog() == true)
            {
                string from = openDialog.FileName;

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Title = "Save Encrypted As";
                saveDialog.Filter = "Xna Files (*.xna)|*.xna";
                if (saveDialog.ShowDialog() == true)
                {
                    string to = saveDialog.FileName;
                    Crypt.EncryptAndMove(from, to);
                }
            }
        }

        private void ToolDecrypt(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Xna Files (*.xna)|*.xna";
            openDialog.Title = "Select Encrypted File";
            if (openDialog.ShowDialog() == true)
            {
                string from = openDialog.FileName;

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Txt Files (*.txt)|*.txt|Any File (*.*)|*.*";
                saveDialog.Title = "Save Decrypted As";
                if (saveDialog.ShowDialog() == true)
                {
                    string to = saveDialog.FileName;
                    try
                    {
                        Crypt.DecryptAndMove(from, to);
                    }
                    catch (Exception ex)
                    {
                        string text = "Error decrypting file. It probably used a different key for encryption.\n" + ex;
                        MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void OpenAsText(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (pathToData != null)
                dialog.InitialDirectory = pathToData;
            dialog.Filter = "Encrypted Files (*.xna)|*.xna|Plain Text Files (*.txt)|*.txt";
            dialog.Multiselect = true;
            dialog.FileName = "s1.xna";
            if (dialog.ShowDialog() == true)
            {
                foreach (string path in dialog.FileNames)
                {
                    if (Path.GetExtension(path).Contains("xna"))
                    {
                        OpenAsText(path);
                    }
                    else
                    {
                        OpenAsText(path, false);
                    }

                    pathToData = Path.GetFullPath(path);
                }
            }
        }

        private void OpenScratchpad(object sender, RoutedEventArgs e)
        {
            TabItem tab = new TabItem();
            tab.Header = "Scratchpad";
            tab.Content = new TextEditorTabItem("Scratchpad\n----------\n\n");
            MainTabControl.Items.Add(tab);
            MainTabControl.SelectedIndex = MainTabControl.Items.IndexOf(tab);
        }

        private void TabItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var tabItem = e.Source as TabItem;

            if (tabItem == null)
                return;

            if (Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(tabItem, tabItem, DragDropEffects.All);
            }
        }
        private void TabItem_Drop(object sender, DragEventArgs e)
        {
            var tabItemTarget = e.Source as TabItem;
            var tabItemSource = e.Data.GetData(typeof(TabItem)) as TabItem;

            if (!tabItemTarget.Equals(tabItemSource))
            {
                var tabControl = tabItemTarget.Parent as TabControl;
                int sourceIndex = tabControl.Items.IndexOf(tabItemSource);
                int targetIndex = tabControl.Items.IndexOf(tabItemTarget);

                tabControl.Items.Remove(tabItemSource);
                tabControl.Items.Insert(targetIndex, tabItemSource);
            }
        }
    }
}
