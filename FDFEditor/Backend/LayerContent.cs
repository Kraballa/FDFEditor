﻿using FDFEditor.Control;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace FDFEditor.Backend
{
    /// <summary>
    /// A batch is a group of bullets that are contained within Layers within a Pattern
    /// </summary>
    public class LayerContent : IHolder
    {
        public enum ContentType
        {
            Batch,
            Laser,
            Cover,
            Rebound,
            Force
        }

        public string[] fields { get; set; }

        public string ParentEvent
        {
            get
            {
                string s = "";
                switch (Type)
                {
                    case ContentType.Batch:
                        s = fields[51];
                        break;
                    case ContentType.Laser:
                        s = fields[42];
                        break;
                    case ContentType.Cover:
                        s = fields[17];
                        break;
                    case ContentType.Rebound:
                        s = fields[13];
                        break;
                }
                return EventParser.Parse(s);
            }
            set
            {
                string s = EventParser.Encode(value);
                switch (Type)
                {
                    case ContentType.Batch:
                        fields[51] = s;
                        break;
                    case ContentType.Laser:
                        fields[42] = s;
                        break;
                    case ContentType.Cover:
                        fields[17] = s;
                        break;
                    case ContentType.Rebound:
                        fields[13] = s;
                        break;
                }
            }
        }

        public ContentType Type;

        private IView View;
        private LayerContent() { }

        public static LayerContent Parse(string batch, ContentType type)
        {
            LayerContent b = new LayerContent();
            string[] str = batch.Split(',');

            b.fields = new string[74];
            int i;
            for (i = 0; i < str.Length; i++)
            {
                b.fields[i] = str[i];
            }
            for (; i < str.Length; i++)
            {
                b.fields[i] = "False";
            }


            b.Type = type;
            switch (b.Type)
            {
                case ContentType.Batch:
                default:
                    b.View = new BatchView(b);
                    break;
                case ContentType.Laser:
                    b.View = new LaserView(b);
                    break;
                case ContentType.Cover:
                    b.View = new CoverView(b);
                    break;
                case ContentType.Rebound:
                    b.View = new ReboundView(b);
                    break;
                case ContentType.Force:
                    b.View = new ForceView(b);
                    break;
            }

            return b;
        }

        public string GetString()
        {
            return string.Join(',', fields);
        }

        public IView GetView()
        {
            return View;
        }
    }
}
