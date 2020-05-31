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




        private LayerView View;

        private LayerHolder()
        {
            Batches = new List<BatchHolder>();
        }

        public static LayerHolder Parse(ref StreamReader stream)
        {
            LayerHolder l = new LayerHolder();

            string str1 = stream.ReadLine();
            if (str1.Contains("empty"))
                return l;

            string num1 = str1.Split(',')[3];
            int numBatches = int.Parse(num1);
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
    }
}
