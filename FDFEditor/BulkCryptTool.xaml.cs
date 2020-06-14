using FDFEditor.Backend;
using FDFEditor.Control;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace FDFEditor
{
    public partial class BulkCryptTool : Window
    {
        private bool oldKeys;
        private int keyIndex;
        private MainWindow parent;

        const int OK = 0;
        const int ERROR = -1;

        public BulkCryptTool(bool oldKeys, int keyIndex, MainWindow parent)
        {
            InitializeComponent();
            this.oldKeys = oldKeys;
            this.keyIndex = keyIndex;
            this.parent = parent;
        }

        private void DecryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckPaths() != OK)
                return;

            string[] files = Directory.GetFiles(PathFromBox.Text, "*.*", SearchOption.AllDirectories);

            var result = MessageBox.Show("are you sure you want to decrypt " + files.Length + " files? It might take a couple of seconds", "Info", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
                return;


            int succeeded = 0;
            int failed = 0;
            StringBuilder failedPaths = new StringBuilder();
            failedPaths.Append("failed to convert the following files:\n");

            foreach (string path in files)
            {
                string destPath = Path.GetDirectoryName(path).Replace(PathFromBox.Text, PathToBox.Text) + "\\";
                if (!Directory.Exists(destPath))
                    Directory.CreateDirectory(destPath);
                if (ExtensionDest.Text.Trim() == "")
                {
                    destPath += Path.GetFileName(path);
                }
                else
                {
                    destPath += Path.GetFileNameWithoutExtension(path) + ExtensionDest.Text.Trim();
                }
                try
                {
                    Crypt.CryptAndCopyFile(path, destPath, true, oldKeys, keyIndex);
                    succeeded++;
                }
                catch (Exception)
                {
                    failedPaths.Append(path + "\n");
                    failed++;
                }
            }
            result = MessageBox.Show(string.Format("Converted a total of {0} files, {1} files failed. View error log?", succeeded, failed), "Report", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
                parent.OpenTab("Error Log", new TextEditorTabItem(failedPaths.ToString()));
        }

        private void EncryptButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckPaths() != OK)
                return;

            string[] files = Directory.GetFiles(PathToBox.Text, "*.*", SearchOption.AllDirectories);

            var result = MessageBox.Show("are you sure you want to encrypt " + files.Length + " files? It might take a couple of seconds", "Info", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
                return;


            int succeeded = 0;
            int failed = 0;
            StringBuilder failedPaths = new StringBuilder();
            failedPaths.Append("failed to convert the following files:\n");

            foreach (string path in files)
            {

                string destPath = Path.GetDirectoryName(path).Replace(PathToBox.Text, PathFromBox.Text) + "\\";
                if (!Directory.Exists(destPath))
                    Directory.CreateDirectory(destPath);
                if (ExtensionSrc.Text.Trim() == "")
                {
                    destPath += Path.GetFileName(path);
                }
                else
                {
                    destPath += Path.GetFileNameWithoutExtension(path) + ExtensionSrc.Text.Trim();
                }

                try
                {
                    Crypt.CryptAndCopyFile(path, destPath, false, oldKeys, keyIndex);
                    succeeded++;
                }
                catch (Exception)
                {
                    failedPaths.Append(path + "\n");
                    failed++;
                }
            }
            result = MessageBox.Show(string.Format("Converted a total of {0} files, {1} files failed. View error log?", succeeded, failed), "Report", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
                parent.OpenTab("Error Log", new TextEditorTabItem(failedPaths.ToString()));
        }

        private void BrowsePathFrom_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog from = new VistaFolderBrowserDialog();
            from.Description = "Select folder containing encrypted files";
            if (from.ShowDialog() == true)
            {
                PathFromBox.Text = from.SelectedPath;
            }
        }

        private void BrowsePathTo_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog to = new VistaFolderBrowserDialog();
            to.Description = "Select folder for decrypted files";
            if (to.ShowDialog() == true)
            {
                PathToBox.Text = to.SelectedPath;
            }
        }

        private int CheckPaths()
        {
            if (!Directory.Exists(PathFromBox.Text))
            {
                MessageBox.Show("Error, source path not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return ERROR;
            }
            if (!Directory.Exists(PathToBox.Text))
            {
                MessageBox.Show("Error, destination path not valid", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return ERROR;
            }
            return OK;
        }
    }
}
