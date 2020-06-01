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
        private BatchHolder holder;

        public string CoverSafe
        {
            get
            {
                return holder.fields.Length >= 73 ? holder.fields[70] : "";
            }
        }
        public string ReboundDafe
        {
            get
            {
                return holder.fields.Length >= 73 ? holder.fields[71] : "";
            }
        }
        public string ForceSave
        {
            get
            {
                return holder.fields.Length >= 73 ? holder.fields[72] : "";
            }
        }
        public string DeepbindSave
        {
            get
            {
                return holder.fields.Length >= 74 ? holder.fields[73] : "";
            }
        }

        public BatchView(BatchHolder holder)
        {
            InitializeComponent();
            DataContext = holder;
            this.holder = holder;
        }
    }
}
