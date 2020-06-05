using FDFEditor.Backend;
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
    /// Interaktionslogik für BatchView.xaml
    /// </summary>
    public partial class BatchView : UserControl, IView
    {
        private LayerContent holder;

        public BatchView(LayerContent holder)
        {
            InitializeComponent();
            DataContext = holder;
            this.holder = holder;
        }
    }
}
