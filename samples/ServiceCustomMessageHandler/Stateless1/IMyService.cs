using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stateless1
{    
    public interface IMyService : IService
    {
        Task<int> ComputeSum(int value1, int value2);
    }
}
