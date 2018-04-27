// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License (MIT). See License.txt in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.ServiceFabric.Powershell
{
    using System;
    using System.Management.Automation;
    using Microsoft.ServiceFabric.Client;
    using Microsoft.ServiceFabric.Client.Http;

    /// <summary>
    /// Cmdlet to connect to Service Fabric cluster.
    /// </summary>
    [Cmdlet(VerbsCommunications.Connect, "SFCluster")]
    public class ConnectClusterCmdlet : PSCmdlet
    {
        /// <summary>
        /// Service Faric cluster http gateway endpoint address.
        /// </summary>
        [Parameter(Position = 1)]
        public string Endpoint { get; set; } = string.Empty;

        /// <inheritdoc />
        protected override void ProcessRecord()
        {
            var client = ServiceFabricClientFactory.Create(new Uri(Endpoint));
            
            client.Cluster.GetClusterManifestAsync().GetAwaiter().GetResult();
            this.SessionState.PSVariable.Set(Constants.ClusterConnectionVariableName, client);
        }
    }
}
