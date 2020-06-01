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

        public string Types = "";
        public string GlobalEvents = "";
        public string Sounds = "";
        public string Center = "";

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
                p.Types += str1 + "\n";
                for (int i = 0; i < int.Parse(str1.Split(' ')[0]); i++)
                {
                    p.Types += stream.ReadLine() + "\n"; //bullet type definitions
                }
                str1 = stream.ReadLine();
            }
            if (str1.Contains("GlobalEvents"))
            {
                p.GlobalEvents += str1 + "\n";
                for (int i = 0; i < int.Parse(str1.Split(' ')[0]); i++)
                {
                    p.GlobalEvents += stream.ReadLine() + "\n"; //global event definitions
                }
                str1 = stream.ReadLine();
            }
            if (str1.Contains("Sounds"))
            {
                p.Sounds += str1 + "\n";
                for (int i = 0; i < int.Parse(str1.Split(' ')[0]); i++)
                {
                    p.Sounds += stream.ReadLine() + "\n"; //sound definitions
                }
                str1 = stream.ReadLine();
            }
            if (str1.Contains(','))
            {
                p.Center += str1 + "\n";
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
            ret += Types + GlobalEvents + Sounds + Center;
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
