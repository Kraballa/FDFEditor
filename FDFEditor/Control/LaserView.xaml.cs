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
    /// Interaktionslogik für LaserView.xaml
    /// </summary>
    public partial class LaserView : UserControl, IView
    {
        private LaserHolder holder;
        public LaserView(LaserHolder holder)
        {
            InitializeComponent();
            DataContext = holder;
            this.holder = holder;
        }
    }
}
