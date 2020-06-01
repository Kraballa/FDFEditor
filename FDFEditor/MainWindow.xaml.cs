﻿using FDFEditor.Backend;
using FDFEditor.Control;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace FDFEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string pathToData;
        private PatternHolder pHolder;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile(string path)
        {
            try
            {
                //decrypt and parse
                Stream s = Crypt.Decrypt(path);
                pHolder = PatternHolder.Parse(s);
                TabItem tab = new TabItem();
                tab.Header = Path.GetFileName(path);
                tab.Content = new XnaTabItem(pHolder);
                MainTabControl.Items.Add(tab);
                MainTabControl.SelectedIndex = MainTabControl.Items.IndexOf(tab);
            }
            catch (Exception)
            {
                string text = "Error parsing file. Either it's invalid or it was already decrypted.";
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
            if (dialog.ShowDialog() == true)
            {
                foreach (string path in dialog.FileNames)
                {
                    OpenFile(path);
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
            if (pHolder == null)
            {
                MessageBox.Show("no pattern opened", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                Console.WriteLine(pHolder.GetString());
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
                    Crypt.DecryptAndMove(from, to);
                }
            }
        }
    }
}
