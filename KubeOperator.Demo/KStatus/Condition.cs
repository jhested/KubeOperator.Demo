namespace KubeOperator.Demo
{
    public struct Condition
    {
        /// <summary>
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
        public Condition(string reason, string type, ConditionStatus status, string? message = null)
        {
            Reason = reason;
            Type = type;
            Status = status;
            Message = message;
        }

        /// <summary>
        /// lastTransitionTime is the last time the condition transitioned from one status to another.
        /// This should be when the underlying condition changed.  If that is not known, then using the 
        /// time when the API field changed is acceptable.
        /// +required
        /// </summary>
        public DateTime LastTransitionTime { get; set; }

        /// <summary>
        /// Message`** is intended to be a human-readable phrase
        /// or sentence, which may contain specific details of the individual occurrence.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Reason is
        /// intended to be a one-word, PascalCase representation of the category of cause of
        /// the current status. Reason is intended to be used in concise output, such as one-line
        /// `kubectl get` output, and in summarizing occurrences of causes
        /// </summary>
        public string Reason { get; set; }


        public ConditionStatus Status { get; set; }

        /// <summary>
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
        /// </summary>
        public string Type { get; set; }
    }
}
