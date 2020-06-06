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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenAsPattern(string path, bool encrypted = true)
        {
            try
            {
                Stream s;
                if (encrypted)
                {
                    s = Crypt.OpenCryptFile(path, encrypted, FDF1Checkbox.IsChecked);
                }
                else
                {
                    s = new MemoryStream(File.ReadAllBytes(path));
                }
                PatternHolder pHolder = PatternHolder.Parse(s);
                OpenTab(Path.GetFileName(path), new PatternTabItem(pHolder));
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
                Stream s;
                if (encrypted)
                {
                    s = Crypt.OpenCryptFile(path, true, FDF1Checkbox.IsChecked);
                }
                else
                {
                    s = new MemoryStream(File.ReadAllBytes(path));
                }
                OpenTab(Path.GetFileName(path), new TextEditorTabItem(s));
            }
            catch (Exception e)
            {
                string text = "Error reading file. It probably was already decrypted.\n" + e;
                MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenCommand(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Encrypted Files (*.xna)|*.xna|Plain Text Files (*.txt)|*.txt";
            dialog.Multiselect = true;
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
                }
            }
        }

        private void CloseCommand(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            if (MainTabControl.Items.Count > 0)
            {
                Console.WriteLine(MainTabControl.SelectedIndex);
                MainTabControl.Items.Remove(MainTabControl.SelectedItem);
            }
            else
            {
                System.Windows.Application.Current.Shutdown();
            }
        }

        private void ExitCommand(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            InputGestureCollection command = ((RoutedCommand)e.Command).InputGestures;
            for (var enumerator = command.GetEnumerator(); enumerator.MoveNext();)
            {
                KeyGesture gesture = enumerator.Current as KeyGesture;
                if (gesture.DisplayString.Equals("Esc")) //not exit when pressing escape. this is a hack. Fix by implementing a custom command since pressing escape seems to trigger this automatically.
                    return;
            }

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
                ITabItem current = (ITabItem)MainTabControl.SelectedContent;
                string text = current.GetPlainText();
                Console.WriteLine(text);
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Xna File (*.xna)|*.xna|Text File (*.txt)|*.txt";
                saveDialog.Title = "Save As";
                saveDialog.FileName = ((TabItem)MainTabControl.Items[MainTabControl.SelectedIndex]).Header as string;
                if (saveDialog.ShowDialog() == true)
                {
                    try
                    {
                        if (Path.GetExtension(saveDialog.FileName).Contains("xna"))
                        {
                            Crypt.CryptToFile(saveDialog.FileName, text, FDF1Checkbox.IsChecked);
                        }
                        else
                        {
                            File.WriteAllText(saveDialog.FileName, text);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Something went wrong.\n" + ex, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
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
                saveDialog.Title = "Save " + Path.GetFileName(from) + " Encrypted As";
                saveDialog.Filter = "Xna Files (*.xna)|*.xna";
                saveDialog.FileName = Path.GetFileNameWithoutExtension(from);
                if (saveDialog.ShowDialog() == true)
                {
                    string to = saveDialog.FileName;
                    Crypt.CryptAndMove(from, to, false, FDF1Checkbox.IsChecked);
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
                saveDialog.Title = "Save " + Path.GetFileName(from) + " Decrypted As";
                saveDialog.FileName = Path.GetFileNameWithoutExtension(from);
                if (saveDialog.ShowDialog() == true)
                {
                    string to = saveDialog.FileName;
                    try
                    {
                        Crypt.CryptAndMove(from, to, true, FDF1Checkbox.IsChecked);
                    }
                    catch (Exception ex)
                    {
                        string text = "Error decrypting file. It probably used a different key for encryption.\n" + ex;
                        MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void OpenAsPattern(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Pattern Files (b*.xna)|b*.xna";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == true)
            {
                foreach (string path in dialog.FileNames)
                {
                    OpenAsPattern(path);
                }
            }
        }

        private void OpenScratchpad(object sender, RoutedEventArgs e)
        {
            OpenTab("Scratchpad", new TextEditorTabItem("Scratchpad\n----------\n\n"));
        }

        private void OpenAbout(object sender, RoutedEventArgs e)
        {
            OpenTab("About", new TextEditorTabItem(new StreamReader("Resources/About.txt"), true));
        }

        private void OpenLicense(object sender, RoutedEventArgs e)
        {
            OpenTab("License", new TextEditorTabItem(new StreamReader("Resources/License.txt"), true));
        }

        private void OpenModdingGuide(object sender, RoutedEventArgs e)
        {
            OpenTab("Modding Guide", new TextEditorTabItem(new StreamReader("Resources/Modding Guide.txt"), true));
        }

        private void TabItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var tabItem = e.Source as TabItem;

            if (tabItem == null)
                return;

            try
            {
                if (Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
                {
                    DragDrop.DoDragDrop(tabItem, tabItem, DragDropEffects.All);
                }
            }
            catch (Exception) { }
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

        private void OpenTab(string header, ITabItem content)
        {
            TabItem tab = new TabItem();
            tab.Header = header;
            tab.Content = content;
            MainTabControl.Items.Add(tab);
            MainTabControl.SelectedIndex = MainTabControl.Items.IndexOf(tab);
        }

        private void FDFCheckbox_Click(object sender, RoutedEventArgs e)
        {
            MenuItem cb = sender as MenuItem;
            if (cb.Name.Contains("FDF1"))
            {
                FDF1Checkbox.IsChecked = true;
                FDF2Checkbox.IsChecked = false;
            }
            else
            {
                FDF1Checkbox.IsChecked = false;
                FDF2Checkbox.IsChecked = true;
            }
        }
    }
}
