using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using PointOfSale.Common;
using PointOfSale.DataAccess;
using PointOfSale.Domain;
using PointOfSale.Dto.Request;
using PointOfSale.Dto.Response;
using System.ComponentModel.DataAnnotations;
using PointOfSale.Domain.Models;
using Error = PointOfSale.Domain.Error;

namespace PointOfSale.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly BasketRepository basketRepository;

        public BasketController(BasketRepository basketRepository)
        {
            this.basketRepository = basketRepository;
        }

        /// <summary>
        /// Creates a new basket
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(BasketResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateBasket([FromBody] CreateBasket request)
        {

            var basketAr = new BasketAR();
            Result<BasketResponse, Error> result = await basketAr.Create(request.StoreId, request.DeviceId)
                .Tap(_ => basketRepository.Upsert(basketAr))
                .Map(r => new BasketResponse(r));

            return this.Result(result);
        }

        /// <summary>
        /// Gets basket by identifier
        /// </summary>
        /// <param name="basketId"></param>
        /// <returns></returns>
        [HttpGet("{basketId}")]
        public async Task<IActionResult> GetBasketAsync([FromRoute, Required] Guid basketId)
        {
            BasketAR basket = await basketRepository.Get(basketId);

            if (basket == null)
                return this.Result<BasketResponse>(BasketErrors.BasketNotFound(basketId));

            var response = new BasketResponse(basket.GetBasket().Value);

            return Ok(response);
        }

        /// <summary>
        /// Adds a new item to the basket
        /// </summary>
        [HttpPost("{basketId}/items")]
        [ProducesResponseType(typeof(BasketResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddItem([FromRoute, Required] Guid basketId, [FromBody] AddItem request)
        {
            BasketAR? basketAr = await basketRepository.Get(basketId);

            if (basketAr == null)
                return this.Result<BasketResponse>(BasketErrors.BasketNotFound(basketId));

            var product = new Product()
            {
                Code = request.Code,
                Category = request.Category,
                Discount = request.Discount,
                ExternalProductId = request.ExternalProductId,
                Name = request.Name,
                Price = request.Price
            };

            Result<BasketResponse, Error> result = await basketAr.AddItem(product)
                .Tap(_ => basketRepository.Upsert(basketAr))
                .Map(res => new BasketResponse(res));

            return this.Result(result);
        }
    }
}
