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
    /// Cmdlet to test connection to Service Fabric cluster. 
    /// </summary>
    [Cmdlet(VerbsDiagnostic.Test, "SFClusterConnection")]
    public class TestClusterConnectionCmdlet : PSCmdlet
    {
        /// <inheritdoc />
        protected override void ProcessRecord()
        {
            var client = (IServiceFabricClient)this.SessionState.PSVariable.GetValue(Constants.ClusterConnectionVariableName);
            this.WriteObject(client != null);
        }
    }
}
