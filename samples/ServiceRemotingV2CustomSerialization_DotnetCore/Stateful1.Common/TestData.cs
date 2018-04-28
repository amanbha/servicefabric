using System;
using System.Collections.Generic;
using System.Text;

namespace Stateful1.Common
{
    public class TestData
    {
        private IEnumerable<IMyData> dataToSend = new List<IMyData>()
        {
            new MyData() { Data ="TestData1" }, 
            new MyData() { Data = "TestData2" }
        };        
    }
}
