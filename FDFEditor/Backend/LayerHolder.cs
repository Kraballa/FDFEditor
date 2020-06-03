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
        public List<LayerContent> Content;

        public string name { get; set; }
        public string begin { get; set; }
        public string end { get; set; }
        public string[] numThings = new string[5];

        private LayerView View;
        private string index;

        private LayerHolder()
        {
            Content = new List<LayerContent>();
        }

        public static LayerHolder Parse(ref StreamReader stream)
        {
            LayerHolder l = new LayerHolder();

            string[] str1 = stream.ReadLine().Split(',');
            if (str1[0].Contains("empty"))
                return l;

            l.index = str1[0].Split(':')[0].Replace("Layer", "");
            l.name = str1[0].Split(':')[1];
            l.begin = str1[1];
            l.end = str1[2];

            for (int i = 0; i < 5; i++)
            {
                l.numThings[i] = str1[i + 3];
            }

            int num1 = int.Parse(str1[3]);
            for (int i = 0; i < num1; i++)
            {
                l.Content.Add(LayerContent.Parse(stream.ReadLine()));
            }

            num1 = int.Parse(str1[4]);
            for (int i = 0; i < num1; i++)
            {
                l.Content.Add(LayerContent.Parse(stream.ReadLine()));
            }

            num1 = int.Parse(str1[5]);
            for (int i = 0; i < num1; i++)
            {
                l.Content.Add(LayerContent.Parse(stream.ReadLine()));
            }

            num1 = int.Parse(str1[6]);
            for (int i = 0; i < num1; i++)
            {
                l.Content.Add(LayerContent.Parse(stream.ReadLine()));
            }

            num1 = int.Parse(str1[7]);
            for (int i = 0; i < num1; i++)
            {
                l.Content.Add(LayerContent.Parse(stream.ReadLine()));
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
            string ret = "Layer" + index + ":";
            if (Content.Count == 0)
            {
                ret += "empty\n";
                return ret;
            }
            else
            {
                ret += string.Format("{0},{1},{2},{3},0,0,0,0\n", name, begin, end, Content.Count);
                foreach (LayerContent b in Content)
                {
                    ret += b.GetString();
                }
                return ret;
            }
        }
    }
}
