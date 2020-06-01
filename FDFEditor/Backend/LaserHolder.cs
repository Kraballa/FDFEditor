using FDFEditor.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace FDFEditor.Backend
{
    /// <summary>
    /// A batch is a group of bullets that are contained within Layers within a Pattern
    /// </summary>
    public class LaserHolder : IHolder, ILayerContent
    {

        #region values

        public string id;
        public string parentId;
        public string bound;
        public string bindId;
        public string boundWithSpeed;

        #endregion

        private LaserView View;
        private LaserHolder() { }

        public static LaserHolder Parse(string batch)
        {
            LaserHolder b = new LaserHolder();



            //parse stuff

            b.View = new LaserView(b);
            return b;
        }

        public string GetString()
        {
            return "this is a batch\n";
        }

        public IView GetView()
        {
            return View;
        }
    }
}
