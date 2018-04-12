using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stateless1
{
    public class CustomRemotingEvents
    {
        public static event EventHandler ReceiveRequest;

        public static event EventHandler SendResponse;

        
        internal static void RaiseReceiveRequest(string info)
        {
            ReceiveRequest?.Invoke(null, new CustomEventArgs(info));
        }
        
        internal static void RaiseSendResponse(string info)
        {
            SendResponse?.Invoke(null, new CustomEventArgs(info));
        }
    }
}
