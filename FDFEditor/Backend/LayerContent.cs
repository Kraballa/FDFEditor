using FDFEditor.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace FDFEditor.Backend
{
    /// <summary>
    /// A batch is a group of bullets that are contained within Layers within a Pattern
    /// </summary>
    public class LayerContent : IHolder
    {

        #region values

        public string[] fields { get; set; }

        #endregion

        private IView View;
        private LayerContent() { }

        public static LayerContent Parse(string batch)
        {
            LayerContent b = new LayerContent();
            string[] str = batch.Split(',');

            b.fields = new string[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                b.fields[i] = str[i];
            }

            b.View = new BatchView(b);
            return b;
        }

        public string GetString()
        {
            return string.Join(',', fields) + "\n";
        }

        public IView GetView()
        {
            return View;
        }
    }
}
