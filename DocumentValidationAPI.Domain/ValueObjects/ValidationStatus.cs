namespace DocumentValidationAPI.Domain.ValueObjects
{
    public sealed class ValidationStatus
    {
        public string Value { get; }

        private ValidationStatus(string value)
        {
            Value = value;
        }

        public static readonly ValidationStatus Pending = new("P");
        public static readonly ValidationStatus Approved = new("A");
        public static readonly ValidationStatus Rejected = new("R");

        public static ValidationStatus From(string value)
        {
            return value switch
            {
                "P" => Pending,
                "A" => Approved,
                "R" => Rejected,
                _ => throw new ArgumentException($"Invalid validation status '{value}'")
            };
        }

        public override string ToString() => Value;

        public override bool Equals(object? obj)
        {
            return obj is ValidationStatus other && other.Value == Value;
        }

        public override int GetHashCode() => Value.GetHashCode();
    }
}
