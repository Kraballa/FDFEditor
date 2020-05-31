using FDFEditor.Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FDFEditor.Backend
{
    public class PatternHolder : IHolder
    {
        public LayerHolder[] Layers = new LayerHolder[4];
        public int TotalFrames;
        private PatternView View;

        private PatternHolder() { }

        public static PatternHolder Parse(Stream script)
        {
            PatternHolder p = new PatternHolder();
            StreamReader stream = new StreamReader(script);
            if (!stream.ReadLine().Equals("Crazy Storm Data 1.01"))
                throw new Exception("error, invalid script format");

            string str1 = stream.ReadLine();
            if (str1.Contains("Types"))
            {
                for (int i = 0; i < int.Parse(str1.Split(' ')[0]); i++)
                {
                    stream.ReadLine(); //bullet type definitions
                }
                str1 = stream.ReadLine();
            }
            if (str1.Contains("GlobalEvents"))
            {
                for (int i = 0; i < int.Parse(str1.Split(' ')[0]); i++)
                {
                    stream.ReadLine(); //global event definitions
                }
                str1 = stream.ReadLine();
            }
            if (str1.Contains("Sounds"))
            {
                for (int i = 0; i < int.Parse(str1.Split(' ')[0]); i++)
                {
                    stream.ReadLine(); //sound definitions
                }
                str1 = stream.ReadLine();
            }
            if (str1.Contains(','))
            {
                //"Center" Definition, whatever that is
            }
            p.TotalFrames = int.Parse(stream.ReadLine().Split(':')[1]);
            for (int i = 0; i < p.Layers.Length; i++)
            {
                p.Layers[i] = LayerHolder.Parse(ref stream);
            }
            stream.Close();
            p.View = new PatternView(p);
            return p;
        }

        public static string ToFile()
        {
            return "";
        }

        public IView GetView()
        {
            return View;
        }
    }
}
