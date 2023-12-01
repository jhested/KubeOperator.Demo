using System.Text.RegularExpressions;

namespace KubeOperator.Demo
{
    public partial class Status
    {
        public IEnumerable<Condition> Conditions { get; set; } = new List<Condition>();
        public long? ObservedGeneration { get; set; }

        /// <summary>
        /// Set or update a status condition based on Type.
        /// If the type has already been added to conditions it will update the existing condition
        /// </summary>
        /// <param name="reason">        
        /// Reason is
        /// intended to be a one-word, PascalCase representation of the category of cause of
        /// the current status. Reason is intended to be used in concise output, such as one-line
        /// `kubectl get` output, and in summarizing occurrences of causes</param>
        /// <param name="type">
        /// An example of an oscillating condition type is `Ready`, which indicates the
        /// object was believed to be fully operational at the time it was last probed.A
        /// possible monotonic condition could be `Succeeded`. A `True` status for
        /// `Succeeded` would imply completion and that the resource was no longer
        /// active.An object that was still active would generally have a `Succeeded`
        /// condition with status `Unknown`.
        /// 
        /// Condition type names should describe the current observed state of the
        /// resource, rather than describing the current state transitions.This
        /// typically means that the name should be an adjective("Ready", "OutOfDisk")
        /// or a past-tense verb("Succeeded", "Failed") rather than a present-tense verb
        /// ("Deploying"). Intermediate states may be indicated by setting the `status` of
        /// the condition to `Unknown`.
        /// </param>
        /// <param name="status"></param>
        public void SetCondition(string reason, string type, ConditionStatus status, string? message = null)
        {
            var condition = new Condition(reason, type, status, message);

            SetCondition(condition);
        }


        /// <summary>
        /// Set or update a status condition based on Type.
        /// If the type has already been added to conditions it will update the existing condition <br />
        /// This method is not thread safe
        /// </summary>
        /// <param name="condition"></param>
        /// <exception cref="InvalidOperationException">Thrown if condition.Reason is not PascalCase</exception>
        public void SetCondition(Condition condition)
        {
            if (!PascalCaseRegEx(condition.Reason))
            {
                throw new InvalidOperationException($"Reason should be PascalCase, the value was: '{condition.Reason}'");
            }

            var conditions = Conditions.ToDictionary(x => x.Type);

            condition = SetLastTransitionTime(conditions, condition);
            conditions[condition.Type] = condition;

            Conditions = conditions.Values;
        }

        public Condition? GetCondition(string type)
        {
            if (Conditions.Any(x => x.Type == type))
            {
                return Conditions.Single(x => x.Type == type);
            }
            return null;
        }

        private static Condition SetLastTransitionTime(Dictionary<string, Condition> conditions, Condition condition)
        {
            if (conditions.TryGetValue(condition.Type, out var existing) && existing.Status == condition.Status)
            {
                condition.LastTransitionTime = existing.LastTransitionTime;
            }
            else
            {
                condition.LastTransitionTime = DateTime.UtcNow;
            }

            return condition;
        }

        private static bool PascalCaseRegEx(string input)
        {
            var regex = GetPascalCaseRegEx();
            return regex.IsMatch(input);
        }

        [GeneratedRegex("^[A-Z][a-zA-Z0-9]*$")]
        private static partial Regex GetPascalCaseRegEx();
    }
}