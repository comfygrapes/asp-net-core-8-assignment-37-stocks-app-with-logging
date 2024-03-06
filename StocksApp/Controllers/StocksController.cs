using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using StocksApp.Options;
using System.Linq;

namespace StocksApp.Controllers
{
    [Route("[Controller]")]
    public class StocksController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IOptions<TradingOptions> _tradingOptions;

        public StocksController(IFinnhubService finnhubService, IOptions<TradingOptions> tradingOptions)
        {
            _finnhubService = finnhubService;
            _tradingOptions = tradingOptions;
        }

        [HttpGet]
        [Route("/")]
        [Route("[Action]")]
        public async Task<IActionResult> Explore(string? stockSymbol)
        {
            var retrievedStocks = await _finnhubService.GetStocks();

            var stockSymbols = _tradingOptions.Value.Top25PopularStocks?.Split(',');

            retrievedStocks = retrievedStocks?.Where(stock => stockSymbols.Contains(stock["symbol"])).ToList();

            var stocks = retrievedStocks?.Select(retrievedStock => new Stock()
            {
                StockSymbol = retrievedStock["symbol"],
                StockName = retrievedStock["description"],
            });

            if (stockSymbol != null)
            {
                ViewData["SelectedStockSymbol"] = stockSymbol;
            }

            return View(stocks);
        }
    }
}
