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

        private PatternHolder()
        {
            Types = "";
            GlobalEvents = "";
            Sounds = "";
            Center = "";
        }

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
                    if (i > 0)
                        p.Types += "\n";
                    p.Types += stream.ReadLine(); //bullet type definitions
                }
                str1 = stream.ReadLine();
            }
            if (str1.Contains("GlobalEvents"))
            {
                for (int i = 0; i < int.Parse(str1.Split(' ')[0]); i++)
                {
                    if (i > 0)
                        p.Types += "\n";
                    p.GlobalEvents += stream.ReadLine(); //global event definitions
                }
                str1 = stream.ReadLine();
            }
            if (str1.Contains("Sounds"))
            {
                for (int i = 0; i < int.Parse(str1.Split(' ')[0]); i++)
                {
                    if (i > 0)
                        p.Types += "\n";
                    p.Sounds += stream.ReadLine(); //sound definitions
                }
                str1 = stream.ReadLine();
            }
            if (str1.Contains("Center"))
            {
                p.Center += str1.Remove(0, 7);
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
            StringBuilder ret = new StringBuilder("Crazy Storm Data 1.01\n");
            if (Types.Trim() != "")
            {
                ret.Append(Types.Split('\n').Length + " Types:\n");
                ret.Append(Types + "\n");
            }
            if (GlobalEvents.Trim() != "")
            {
                ret.Append(GlobalEvents.Split('\n').Length + " GlobalEvents:\n");
                ret.Append(GlobalEvents + "\n");
            }
            if (Sounds.Trim() != "")
            {
                ret.Append(Sounds.Split('\n').Length + " Sounds:\n");
                ret.Append(Sounds + "\n");
            }
            if (Center.Trim() != "")
            {
                ret.Append("Center:");
                ret.Append(Center + "\n");
            }
            ret.Append("Totalframe:" + TotalFrames + "\n");
            for (int i = 0; i < Layers.Length; i++)
            {
                ret.Append(Layers[i].GetString() + "\n");
            }

            return ret.ToString();
        }

        public IView GetView()
        {
            return View;
        }
    }
}
