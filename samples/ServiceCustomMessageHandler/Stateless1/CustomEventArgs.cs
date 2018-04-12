using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stateless1
{
    public class CustomEventArgs : EventArgs
    {
        public string Info {get;}

        public CustomEventArgs(string info)
        {
            this.Info = info;
        }
    }
}
