﻿using FDFEditor.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace FDFEditor.Backend
{
    /// <summary>
    /// A batch is a group of bullets that are contained within Layers within a Pattern
    /// </summary>
    public class BatchHolder : IHolder, ILayerContent
    {

        #region values

        public string[] fields;

        #endregion

        private BatchView View;
        private BatchHolder() { }

        public static BatchHolder Parse(string batch)
        {
            BatchHolder b = new BatchHolder();
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
