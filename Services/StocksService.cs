using Entities;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTOs;
using Services.Helpers;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly IStocksRepository _stocksRepository;

        public StocksService(IStocksRepository stocksRepository)
        {
            _stocksRepository = stocksRepository;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if (buyOrderRequest == null) throw new ArgumentNullException(nameof(buyOrderRequest));
            ValidationHelper.ValidateModel(buyOrderRequest);

            var buyOrder = buyOrderRequest.ToBuyOrder();
            buyOrder.BuyOrderId = Guid.NewGuid();

            var buyOrderFromRepository = await _stocksRepository.CreateBuyOrder(buyOrder);

            return buyOrder.ToBuyOrderResponse();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null) throw new ArgumentNullException(nameof(sellOrderRequest));
            ValidationHelper.ValidateModel(sellOrderRequest);

            var sellOrder = sellOrderRequest.ToSellOrder();
            sellOrder.SellOrderId = Guid.NewGuid();

            var sellOrderFromRepository = await _stocksRepository.CreateSellOrder(sellOrder);

            return sellOrder.ToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetAllBuyOrders()
        {
            var allBuyOrders = (await _stocksRepository.GetBuyOrders()).Select(buyOrder => buyOrder.ToBuyOrderResponse()).ToList();

            return allBuyOrders;
        }

        public async Task<List<SellOrderResponse>> GetAllSellOrders()
        {
            var allSellOrders = (await _stocksRepository.GetSellOrders()).Select(sellOrder => sellOrder.ToSellOrderResponse()).ToList();

            return allSellOrders;
        }
    }
}
