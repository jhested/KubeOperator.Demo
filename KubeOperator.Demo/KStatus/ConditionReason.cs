namespace KubeOperator.Demo.KStatus
{
    public static class ConditionReason
    {
        public const string ReconciliationSucceeded = nameof(ReconciliationSucceeded);
        public const string ReconciliationFailed = nameof(ReconciliationFailed);
        public const string Reconciling = nameof(Reconciling);

        public const string ServiceAccountRequested = nameof(ServiceAccountRequested);
        public const string AppRegistrationRequested = nameof(AppRegistrationRequested);
    }
}
