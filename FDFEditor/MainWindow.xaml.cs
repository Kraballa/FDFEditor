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

        #region Commands

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
            MainTabControl.Items.Remove(MainTabControl.SelectedItem);
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
                MessageBox.Show("No tab opened.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                ITabItem current = (ITabItem)MainTabControl.SelectedContent;
                string text = current.GetPlainText();
                Console.WriteLine(text);
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Encrypted File (*.*)|*.*|Unencrypted File (*.*)|*.*";
                saveDialog.Title = "Save As";
                saveDialog.FileName = ((TabItem)MainTabControl.Items[MainTabControl.SelectedIndex]).Header as string;
                if (saveDialog.ShowDialog() == true)
                {
                    //((TabItem)MainTabControl.Items[MainTabControl.SelectedIndex]).Header = Path.GetFileNameWithoutExtension(saveDialog.FileName);
                    try
                    {
                        if (saveDialog.FilterIndex == 0)
                        {
                            Crypt.CryptContentToFile(saveDialog.FileName, text, false, FDF1Checkbox.IsChecked, GetSelectedKeyIndex());
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

        #endregion

        #region Opening Events

        private void OpenAsText(string path, bool encrypted = true)
        {
            try
            {
                Stream s;
                if (encrypted)
                {
                    s = Crypt.CryptStreamFromFile(path, true, FDF1Checkbox.IsChecked, GetSelectedKeyIndex());
                }
                else
                {
                    s = new MemoryStream(File.ReadAllBytes(path));
                }
                TextEditorTabItem tab = new TextEditorTabItem(s);
                OpenTab(Path.GetFileName(path), tab);
            }
            catch (Exception e)
            {
                string text = "Error reading file. It probably was already decrypted.\n" + e;
                MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenAsPattern(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Encrypted File (*.*)|*.*|Unencrypted File (*.*)|*.*";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == true)
            {
                foreach (string path in dialog.FileNames)
                {
                    try
                    {
                        Stream s;
                        if (dialog.FilterIndex == 0)
                        {
                            s = Crypt.CryptStreamFromFile(path, true, FDF1Checkbox.IsChecked, GetSelectedKeyIndex());
                        }
                        else
                        {
                            s = new MemoryStream(File.ReadAllBytes(path));
                        }
                        PatternHolder pHolder = PatternHolder.Parse(s);
                        OpenTab(Path.GetFileName(path), new PatternTabItem(pHolder));
                    }
                    catch (Exception ex)
                    {
                        string text = "Error. Either the file couldn't be parsed or the decryption failed.\n" + ex;
                        MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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

        private void OpenHowToDoThings(object sender, RoutedEventArgs e)
        {
            OpenTab("How to do Things", new TextEditorTabItem(new StreamReader("Resources/How to do Things.txt"), true));
        }

        #endregion

        #region Tools

        private void ToolEncrypt(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Any File (*.*)|*.*";
            openDialog.Title = "Select Decrypted File";
            if (openDialog.ShowDialog() == true)
            {
                string from = openDialog.FileName;

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Title = "Save " + Path.GetFileName(from) + " Encrypted As";
                saveDialog.Filter = "Any File (*.*)|*.*";
                saveDialog.FileName = Path.GetFileNameWithoutExtension(from);
                if (saveDialog.ShowDialog() == true)
                {
                    Crypt.CryptAndCopyFile(from, saveDialog.FileName, false, FDF1Checkbox.IsChecked, GetSelectedKeyIndex());
                }
            }
        }

        private void ToolDecrypt(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Any File (*.*)|*.*";
            openDialog.Title = "Select Encrypted File";
            if (openDialog.ShowDialog() == true)
            {
                string from = openDialog.FileName;

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Any File (*.*)|*.*";
                saveDialog.Title = "Save " + Path.GetFileName(from) + " Decrypted As";
                saveDialog.FileName = Path.GetFileNameWithoutExtension(from) + "_decrypted";
                if (saveDialog.ShowDialog() == true)
                {
                    try
                    {
                        Crypt.CryptAndCopyFile(from, saveDialog.FileName, true, FDF1Checkbox.IsChecked, GetSelectedKeyIndex());
                    }
                    catch (Exception ex)
                    {
                        string text = "Error decrypting file. It probably used a different key for encryption.\n" + ex;
                        MessageBox.Show(text, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        #endregion

        private void OpenTab(string header, ITabItem content)
        {
            TabItem tab = new TabItem();
            tab.Header = header;
            tab.Content = content;
            MainTabControl.Items.Add(tab);
            MainTabControl.SelectedIndex = MainTabControl.Items.IndexOf(tab);
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

            if (tabItemTarget != null && !tabItemTarget.Equals(tabItemSource))
            {
                var tabControl = tabItemTarget.Parent as TabControl;
                int sourceIndex = tabControl.Items.IndexOf(tabItemSource);
                int targetIndex = tabControl.Items.IndexOf(tabItemTarget);

                tabControl.Items.Remove(tabItemSource);
                tabControl.Items.Insert(targetIndex, tabItemSource);
            }
        }

        //There are no menu radiobuttons and this is the simplest way to do it
        private void FDFCheckbox_Click(object sender, RoutedEventArgs e)
        {
            MenuItem cb = sender as MenuItem;
            FDF1Checkbox.IsChecked = FDF1Checkbox.Name == cb.Name;
            FDF2Checkbox.IsChecked = FDF2Checkbox.Name == cb.Name;
        }

        private void CloseAllTabs(object sender, RoutedEventArgs e)
        {
            MainTabControl.Items.Clear();
        }

        private void NewItem(object sender, RoutedEventArgs e)
        {
            OpenTab("file", new TextEditorTabItem("", false));
        }

        //There are no menu radiobuttons and this is the simplest way to do it
        private void Key_Click(object sender, RoutedEventArgs e)
        {
            MenuItem cb = sender as MenuItem;
            Key0.IsChecked = Key0.Name == cb.Name;
            Key1.IsChecked = Key1.Name == cb.Name;
            Key2.IsChecked = Key2.Name == cb.Name;
            Key3.IsChecked = Key3.Name == cb.Name;
        }

        private int GetSelectedKeyIndex()
        {
            if (Key0.IsChecked)
                return 0;
            if (Key1.IsChecked)
                return 1;
            if (Key2.IsChecked)
                return 2;
            return 3;
        }
    }
}
