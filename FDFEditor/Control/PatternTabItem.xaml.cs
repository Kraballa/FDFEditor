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
    public partial class PatternTabItem : UserControl, ITabItem
    {
        Dictionary<TreeViewItem, IHolder> ViewMap;

        public PatternTabItem(PatternHolder pattern)
        {
            InitializeComponent();
            ViewMap = new Dictionary<TreeViewItem, IHolder>();
            TreeViewItem root = new TreeViewItem();
            root.Header = "root";
            root.Selected += TabItemSelected;
            ViewMap.Add(root, pattern);
            for (int i = 0; i < pattern.Layers.Length; i++)
            {
                TreeViewItem layerItem = new TreeViewItem();
                layerItem.Header = "layer " + i;
                ViewMap.Add(layerItem, pattern.Layers[i]);
                foreach (LayerContent b in pattern.Layers[i].Content)
                {
                    TreeViewItem contentItem = new TreeViewItem();
                    switch (b.Type)
                    {
                        case LayerContent.ContentType.Batch:
                            contentItem.Header = "batch";
                            break;
                        case LayerContent.ContentType.Laser:
                            contentItem.Header = "laser";
                            break;
                        case LayerContent.ContentType.Cover:
                            contentItem.Header = "cover";
                            break;
                        case LayerContent.ContentType.Rebound:
                            contentItem.Header = "rebound";
                            break;
                        case LayerContent.ContentType.Force:
                            contentItem.Header = "force";
                            break;
                    }
                    ViewMap.Add(contentItem, b);
                    contentItem.Selected += TabItemSelected;
                    layerItem.Items.Add(contentItem);
                }
                layerItem.Selected += TabItemSelected;
                root.Items.Add(layerItem);
            }
            root.ExpandSubtree();
            TreeView.Items.Add(root);
            Pattern = pattern;
        }

        private void TabItemSelected(object sender, RoutedEventArgs e)
        {
            ViewHolder.Children.Clear();
            if (ViewMap[(TreeViewItem)sender].GetView() != null)
            {
                ViewHolder.Children.Add((UIElement)ViewMap[(TreeViewItem)sender].GetView());
            }
            e.Handled = true;
        }

        public PatternHolder Pattern { get; private set; }

        public string GetPlainText()
        {
            return Pattern.GetString();
        }
    }
}
