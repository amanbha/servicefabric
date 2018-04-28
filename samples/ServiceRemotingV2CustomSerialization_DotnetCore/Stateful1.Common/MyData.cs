using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Stateful1.Common
{
    [DataContract]
    public class MyData : IMyData
    {
        [DataMember]
        public string Data { get ; set; }
    }
}
