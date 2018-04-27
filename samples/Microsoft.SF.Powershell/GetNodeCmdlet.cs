// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.ServiceFabric.Powershell
{
    using System;
    using System.Management.Automation;
    using Microsoft.ServiceFabric.Common;
    using Microsoft.ServiceFabric.Client;
    using Microsoft.ServiceFabric.Client.Http;

    /// <summary>
    /// Gets Service Farbic Nodes.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "SFNode")]
    public class GetNodeCmdlet : PSCmdlet
    {
        /// <inheritdoc />
        protected override void ProcessRecord()
        {
            var client =
                (IServiceFabricClient)this.SessionState.PSVariable.GetValue(Constants.ClusterConnectionVariableName);

            var continuationToken = ContinuationToken.Empty;

            do
            {
                var nodeList = client.Nodes.GetNodeInfoListAsync(continuationToken).GetAwaiter().GetResult();

                foreach (var item in nodeList.Data)
                {
                    this.WriteObject(item);
                }

                continuationToken = nodeList.ContinuationToken;
            }
            while (continuationToken.Next);
        }
    }
}
