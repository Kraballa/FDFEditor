using FDFEditor.Control;
using System;
using System.Collections.Generic;
using System.Text;

namespace FDFEditor.Backend
{
    public interface IHolder
    {
        public IView GetView();
    }
}
