using System;
using System.Collections.Generic;
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
    /// Interaktionslogik für TitledTextBox.xaml
    /// </summary>
    public partial class TitledTextBox : UserControl
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(TitledTextBox), new PropertyMetadata(""));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TitledTextBox), new PropertyMetadata(""));



        public bool Readonly
        {
            get { return (bool)GetValue(ReadonlyProperty); }
            set { SetValue(ReadonlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Readonly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ReadonlyProperty =
            DependencyProperty.Register("Readonly", typeof(bool), typeof(TitledTextBox), new PropertyMetadata(false));



        public TitledTextBox()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }
    }
}
