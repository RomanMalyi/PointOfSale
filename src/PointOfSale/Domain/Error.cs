namespace PointOfSale.Domain
{
    public sealed class Error
    {
        public Error(string name, string message, object details = null)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Error name can not be null or empty");
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("Error message can not be null or empty");
            this.Name = name;
            this.Message = message;
            this.Details = details;
        }

        public string Name { get; }

        public string Message { get; }

        public object Details { get; }

        public bool Is(Error error) => this.Name == error.Name;

        public override string ToString() => "Name: '" + this.Name + "'. Message: '" + this.Message + "'.";
    }
}
