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
    /// Interaktionslogik für XnaTabItem.xaml
    /// </summary>
    public partial class XnaTabItem : UserControl
    {
        Dictionary<TreeViewItem, IHolder> ViewMap;

        public XnaTabItem(PatternHolder pattern)
        {
            InitializeComponent();
            ViewMap = new Dictionary<TreeViewItem, IHolder>();
            TreeViewItem root = new TreeViewItem();
            root.Header = "root";
            ViewMap.Add(root, pattern);
            for (int i = 0; i < pattern.Layers.Length; i++)
            {
                TreeViewItem layerItem = new TreeViewItem();
                layerItem.Header = "layer " + i;
                ViewMap.Add(layerItem, pattern.Layers[i]);
                foreach (BatchHolder b in pattern.Layers[i].Batches)
                {
                    TreeViewItem batchItem = new TreeViewItem();
                    batchItem.Header = "batch";
                    ViewMap.Add(batchItem, b);
                    batchItem.Selected += TabItemSelected;
                    layerItem.Items.Add(batchItem);
                }
                layerItem.Selected += TabItemSelected;
                root.Items.Add(layerItem);
            }
            root.ExpandSubtree();
            TreeView.Items.Add(root);
        }

        private void TabItemSelected(object sender, RoutedEventArgs e)
        {
            ViewHolder.Items.Clear();
            Console.WriteLine(sender + " " + ViewMap[(TreeViewItem)sender]);
            ViewHolder.Items.Add(ViewMap[(TreeViewItem)sender].GetView());
            e.Handled = true;
        }

        public string Title { get; set; }
        public PatternHolder Pattern;

        public void Save()
        {

        }
    }
}
