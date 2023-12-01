using KubeOps.Operator.Entities.Annotations;

namespace KubeOperator.Demo.PrinterColumns
{
    public class ReadyPrinterColumnAttribute : GenericAdditionalPrinterColumnAttribute
    {
        public ReadyPrinterColumnAttribute()
            : base(".status.conditions[?(@.type==\"Ready\")].status", "Ready", "string")
        {
        }
    }
}
