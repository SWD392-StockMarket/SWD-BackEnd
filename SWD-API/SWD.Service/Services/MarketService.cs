using SWD.Data.DTOs.Market;
using SWD.Data.DTOs;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using SWD.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD.Data.DTOs.Stock;

namespace SWD.Service.Services
{
    public class MarketService : IMarketService
    {
        private readonly IMarketRepository _marketRepository;

        public MarketService(IMarketRepository marketRepository)
        {
            _marketRepository = marketRepository;
        }

        public async Task<PageListResponse<MarketDTO>> GetMarketsAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20)
        {
            var markets = await _marketRepository.GetAllAsync();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                markets = markets.Where(m => m.MarketName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                if (sortOrder?.ToLower() == "desc")
                {
                    markets = markets.OrderByDescending(GetSortProperty(sortColumn));
                }
                else
                {
                    markets = markets.OrderBy(GetSortProperty(sortColumn)).ToList();
                }
            }

            var totalCount = markets.Count();

            // Apply pagination
            var paginatedMarkets = markets
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PageListResponse<MarketDTO>
            {
                Items = MapMarketsToDTOs(paginatedMarkets),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                HasNextPage = (page * pageSize) < totalCount,
                HasPreviousPage = page > 1
            };
        }

        public async Task<MarketDTO> GetMarketByIdAsync(int id)
        {
            var market = await _marketRepository.GetAsync(m => m.MarketId == id)
                          ?? throw new KeyNotFoundException("Market not found.");

            return MapMarketToDTO(market);
        }

        public async Task<MarketDTO> CreateMarketAsync(CreateMarketDTO dto)
        {
            var market = new Market
            {
                MarketName = dto.MarketName,
                Address = dto.Address,
                EstablishedDate = dto.EstablishedDate,
                PhoneNumber = dto.PhoneNumber,
                Website = dto.Website
            };

            await _marketRepository.AddAsync(market);
            return MapMarketToDTO(market);
        }

        public async Task<MarketDTO> UpdateMarketAsync(int id, UpdateMarketDTO dto)
        {
            var market = await _marketRepository.GetAsync(m => m.MarketId == id)
                          ?? throw new KeyNotFoundException("Market not found.");

            market.MarketName = dto.MarketName;
            market.Address = dto.Address;
            market.EstablishedDate = dto.EstablishedDate;
            market.PhoneNumber = dto.PhoneNumber;
            market.Website = dto.Website;

            var updatedMarket = await _marketRepository.UpdateAsync(market);
            return MapMarketToDTO(updatedMarket);
        }

        public async Task<bool> DeleteMarketAsync(int id)
        {
            var market = await _marketRepository.GetAsync(m => m.MarketId == id);
            if (market == null) return false;

            await _marketRepository.DeleteAsync(market);
            return true;
        }
        public async Task<List<StockDTO>> GetMarketStocksAsync(int marketId)
        {
            var market = await _marketRepository.GetAsync(m => m.MarketId == marketId, includeProperties: "Stocks");
            if (market == null)
            {
                throw new KeyNotFoundException("Market not found.");
            }

            return market.Stocks.Select(s => new StockDTO
            {
                StockId = s.StockId,
                StockSymbol = s.StockSymbol,
                MarketId = s.MarketId,
                ListedDate = s.ListedDate,
                CompanyId = s.CompanyId
            }).ToList();
        }

        private static Func<Market, object> GetSortProperty(string sortColumn)
        {
            return sortColumn?.ToLower() switch
            {
                "marketname" => market => market.MarketName,
                "address" => market => market.Address,
                "establisheddate" => market => market.EstablishedDate,
                _ => market => market.MarketId
            };
        }

        private static MarketDTO MapMarketToDTO(Market market)
        {
            return new MarketDTO
            {
                MarketId = market.MarketId,
                MarketName = market.MarketName,
                Address = market.Address,
                EstablishedDate = market.EstablishedDate,
                PhoneNumber = market.PhoneNumber,
                Website = market.Website
            };
        }

        private List<MarketDTO> MapMarketsToDTOs(List<Market> markets)
        {
            return markets.Select(MapMarketToDTO).ToList();
        }
    }
}