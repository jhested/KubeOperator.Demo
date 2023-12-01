using KubeOps.Operator.Entities.Annotations;

namespace KubeOperator.Demo.PrinterColumns
{
    public class AgePrinterColumnAttribute : GenericAdditionalPrinterColumnAttribute
    {
        public AgePrinterColumnAttribute()
            : base(".metadata.creationTimestamp", "Age", "date")
        {
        }
    }
}
