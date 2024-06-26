using api.Data;
using api.Dtos.Stock;
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

        public Task<List<Stock>> GetAllAsync()
        {
            return _dbContext.Stock.Include(c=>c.Comments).ToListAsync();
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
