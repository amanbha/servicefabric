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
    /// Gets Service Farbic Applciation.
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "SFApplication")]
    public class GetApplicationCmdlet : PSCmdlet
    {
        /// <summary>
        /// Application name to get information for.
        /// </summary>
        [Parameter(Position = 1)]
        public string ApplicationTypeName { get; set; } = string.Empty;

        /// <inheritdoc />
        protected override void ProcessRecord()
        {
            var client =
                (IServiceFabricClient)this.SessionState.PSVariable.GetValue(Constants.ClusterConnectionVariableName);

            var continuationToken = ContinuationToken.Empty;

            do
            {
                var appInfoList = client.Applications.GetApplicationInfoListAsync(
                        applicationTypeName: ApplicationTypeName,
                        continuationToken: continuationToken)
                    .GetAwaiter().GetResult();

                foreach (var item in appInfoList.Data)
                {
                    this.WriteObject(item);
                }

                continuationToken = appInfoList.ContinuationToken;
            }
            while (continuationToken.Next);
        }
    }
}
