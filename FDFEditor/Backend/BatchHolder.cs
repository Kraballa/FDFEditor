using FDFEditor.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace FDFEditor.Backend
{
    /// <summary>
    /// A batch is a group of bullets that are contained within Layers within a Pattern
    /// </summary>
    public class BatchHolder : IHolder
    {
        private BatchView View;

        private BatchHolder() { }

        public static BatchHolder Parse(string batch)
        {
            BatchHolder b = new BatchHolder();



            //parse stuff

            b.View = new BatchView(b);
            return b;
        }

        public string GetString()
        {
            throw new NotImplementedException();
        }

        public IView GetView()
        {
            return View;
        }
    }
}
