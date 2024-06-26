using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync();
        Task<Stock?> GetByIdAsync(int id);
        Task<Stock> CreateAsync(Stock stock);
        Task<Stock?> UpdateByIdAsync(int id, UpdateStockRequestDto updateDto);
        Task<Stock?> DeleteByIdAsync(int id);
        Task<bool> StockExist(int id);

    }
}
