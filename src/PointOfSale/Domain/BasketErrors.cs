namespace PointOfSale.Domain
{
    public class BasketErrors
    {
        public static Error BasketNotCreated =>
            new Error(nameof(BasketNotCreated), "Basket is not created.");

        public static Error BasketAlreadyCreated =>
            new Error(nameof(BasketAlreadyCreated), "The basket is already created.");

        public static Error BasketNotFound(Guid basketId) =>
            new Error(nameof(BasketNotFound), $"The basket with id {basketId} is not found.");
    }
}
