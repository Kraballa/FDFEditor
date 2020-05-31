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
    /// Interaktionslogik für LayerView.xaml
    /// </summary>
    public partial class LayerView : UserControl, IView
    {
        private LayerHolder holder;
        public LayerView(LayerHolder holder)
        {
            InitializeComponent();
            this.holder = holder;
        }
    }
}
