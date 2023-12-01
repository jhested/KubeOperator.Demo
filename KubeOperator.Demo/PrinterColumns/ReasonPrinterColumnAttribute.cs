using KubeOps.Operator.Entities.Annotations;

namespace KubeOperator.Demo.PrinterColumns
{
    public class ReasonPrinterColumnAttribute : GenericAdditionalPrinterColumnAttribute
    {
        public ReasonPrinterColumnAttribute()
            : base(".status.conditions[?(@.type==\"Ready\")].message", "Reason", "string")
        {
        }
    }
}
