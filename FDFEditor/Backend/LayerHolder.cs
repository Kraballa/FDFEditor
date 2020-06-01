using FDFEditor.Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace FDFEditor.Backend
{
    public class LayerHolder : IHolder
    {
        public List<BatchHolder> Batches;

        public string name { get; set; }
        public string begin { get; set; }
        public string end { get; set; }

        private LayerView View;

        private LayerHolder()
        {
            Batches = new List<BatchHolder>();
        }

        public static LayerHolder Parse(ref StreamReader stream)
        {
            LayerHolder l = new LayerHolder();

            string[] str1 = stream.ReadLine().Split(',');
            if (str1[0].Contains("empty"))
                return l;

            l.name = str1[0].Split(':')[1];
            l.begin = str1[1];
            l.end = str1[2];
            int numBatches = int.Parse(str1[3]);

            for (int i = 0; i < numBatches; i++)
            {
                try
                {
                    l.Batches.Add(BatchHolder.Parse(stream.ReadLine()));
                }
                catch (Exception)
                {
                    MessageBox.Show("error while parsing batch " + i, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            l.View = new LayerView(l);
            return l;
        }

        public IView GetView()
        {
            return View;
        }

        public string GetString()
        {
            string ret = "Layer:";
            if (Batches.Count == 0)
            {
                ret += "empty\n";
                return ret;
            }
            else
            {
                ret += string.Format("{0},{1},{2},{3},0,0,0,0\n", name, begin, end, Batches.Count);
                foreach (BatchHolder b in Batches)
                {
                    ret += b.GetString();
                }
                return ret;
            }
        }
    }
}
