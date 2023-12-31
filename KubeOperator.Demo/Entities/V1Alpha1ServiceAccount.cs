﻿using k8s.Models;
using KubeOperator.Demo.PrinterColumns;
using KubeOps.Operator.Entities;
using KubeOps.Operator.Entities.Annotations;

namespace KubeOperator.Demo
{
    [KubernetesEntity(Group = "activedirectory.operatordemo.dk", ApiVersion = "v1alpha1", Kind = "WindowsServiceAccount", PluralName = "windowsserviceaccounts")]
    [ReadyPrinterColumn, ReasonPrinterColumn, AgePrinterColumn]
    public class V1Alpha1ServiceAccount : CustomKubernetesEntity<V1Alpha1ServiceAccountSpec, V1Alpha1ServiceAccountStatus>
    {

    }

    public class V1Alpha1ServiceAccountSpec
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IEnumerable<V1Alpha1ServiceAccountMembership> Memberships { get; set; }  = new List<V1Alpha1ServiceAccountMembership>();

    }
    
    public class V1Alpha1ServiceAccountMembership
    {
        public string GroupName { get; set; } = string.Empty;
    }

    public class V1Alpha1ServiceAccountStatus : Status
    {
        public AzureAppRegistration AzureAppRegistration { get; set; } = new AzureAppRegistration();
    }

    public class AzureAppRegistration
    {
        public string? ApplictionName { get; set; }
        public string? ApplicationId { get; set; }  
    }
}
