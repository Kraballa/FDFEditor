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

        public string Types { get; set; }
        public string GlobalEvents { get; set; }
        public string Sounds { get; set; }
        public string Center { get; set; }

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
                p.Types += str1;
                for (int i = 0; i < int.Parse(str1.Split(' ')[0]); i++)
                {
                    p.Types += "\n" + stream.ReadLine(); //bullet type definitions
                }
                str1 = stream.ReadLine();
            }
            if (str1.Contains("GlobalEvents"))
            {
                p.GlobalEvents += str1;
                for (int i = 0; i < int.Parse(str1.Split(' ')[0]); i++)
                {
                    p.GlobalEvents += "\n" + stream.ReadLine(); //global event definitions
                }
                str1 = stream.ReadLine();
            }
            if (str1.Contains("Sounds"))
            {
                p.Sounds += str1;
                for (int i = 0; i < int.Parse(str1.Split(' ')[0]); i++)
                {
                    p.Sounds += "\n" + stream.ReadLine(); //sound definitions
                }
                str1 = stream.ReadLine();
            }
            if (str1.Contains(','))
            {
                p.Center += str1;
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

        public string GetString()
        {
            string ret = "Crazy Storm Data 1.01\n";
            if (Types != "")
            {
                ret += Types + "\n";
            }
            if (GlobalEvents != "")
            {
                ret += GlobalEvents + "\n";
            }
            if (Sounds != "")
            {
                ret += Sounds + "\n";
            }
            if (Center != "")
            {
                ret += Center + "\n";
            }
            for (int i = 0; i < Layers.Length; i++)
            {
                ret += Layers[i].GetString();
            }

            return ret;
        }

        public IView GetView()
        {
            return View;
        }
    }
}
