using System;
using System.Collections.Generic;
using System.IO;
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

namespace FDFEditor.Control
{
    /// <summary>
    /// Interaktionslogik für TextEditorTabItem.xaml
    /// </summary>
    public partial class TextEditorTabItem : UserControl, ITabItem
    {
        public TextEditorTabItem(string text, bool readOnly = false)
        {
            InitializeComponent();
            MainTextBox.Text = text;
            MainTextBox.IsReadOnly = readOnly;
        }

        public TextEditorTabItem(StreamReader stream, bool readOnly = false) : this(stream.ReadToEnd(), readOnly) { }

        public string GetPlainText()
        {
            return MainTextBox.Text;
        }
    }
}
