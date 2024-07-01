using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public StockRepository(ApplicationDBContext dbContext) { 
        _dbContext = dbContext;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
            await _dbContext.Stock.AddAsync(stock);
            await _dbContext.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> DeleteByIdAsync(int id)
        {
            var stockModel = await _dbContext.Stock.FirstOrDefaultAsync(s => s.Id == id);
            if (stockModel == null)
            {
                return null;
            }
             _dbContext.Stock.Remove(stockModel);
            await _dbContext.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            //return _dbContext.Stock.Include(c=>c.Comments).ToListAsync();
            var stocks = _dbContext.Stock.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(query.CompanyName)) { 
            stocks = stocks.Where(s=>s.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol)) {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy)) {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase)) { 
                stocks = query.IsDescending?stocks.OrderByDescending(s=>s.Symbol) : stocks.OrderBy(s=>s.Symbol);
                }
            }

            var skipNumber = (query.PageNumber-1)*query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
           return await _dbContext.Stock.Include(c=>c.Comments).FirstOrDefaultAsync(s=>s.Id==id);

        }

        public async Task<bool> StockExist(int id)
        {
            return await _dbContext.Stock.AnyAsync(c=>c.Id==id);
        }

        public async Task<Stock?> UpdateByIdAsync(int id, UpdateStockRequestDto updateDto)
        {
            var stockModel = await _dbContext.Stock.FirstOrDefaultAsync(s => s.Id == id);
            if(stockModel == null) return null;

            stockModel.Symbol = updateDto.Symbol;
            stockModel.CompanyName = updateDto.CompanyName;
            stockModel.Purchase = updateDto.Purchase;
            stockModel.LastDiv = updateDto.LastDiv;
            stockModel.Industry = updateDto.Industry;
            stockModel.MarketCap = updateDto.MarketCap;

            await _dbContext.SaveChangesAsync();
            return stockModel;

        }
    }
}
