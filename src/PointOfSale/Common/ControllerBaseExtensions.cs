using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using PointOfSale.Domain;

namespace PointOfSale.Common
{
    public static class ControllerBaseExtensions
    {
        public static ActionResult Result<TData>(this ControllerBase controller, Result<TData, Error> result)
        {
            if (result.IsFailure)
            {
                return new BadRequestObjectResult(result.Error.Message);
            }

            if (result.Value is EmptyResult)
            {
                return new OkResult();
            }

            return new OkObjectResult(result.Value);
        }
    }
}
