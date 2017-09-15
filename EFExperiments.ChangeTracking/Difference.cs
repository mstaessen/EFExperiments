using System.Diagnostics;

namespace EFExperiments.ChangeTracking {
    [DebuggerDisplay("{PropertyName}: {OldValue} -> {NewValue} (Changed: {IsChanged})")]
    public class Difference
    {
        public string PropertyName { get; }

        public object OldValue { get; }

        public object NewValue { get; }

        public bool IsChanged => !Equals(OldValue, NewValue);

        public Difference(string propertyName, object oldValue, object newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
            PropertyName = propertyName;
        }
    }
}