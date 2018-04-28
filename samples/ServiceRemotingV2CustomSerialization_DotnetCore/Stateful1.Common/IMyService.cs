using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.FabricTransport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Stateful1.Common
{    
    public interface IMyService : IService
    {        
        Task<IMyData> GetDataAsync(CancellationToken cancellationToken);

        
        Task SetDataAsync(IMyData data, CancellationToken cancellationToken);
    }
}
