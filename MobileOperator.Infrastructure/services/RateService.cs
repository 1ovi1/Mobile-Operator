// using Microsoft.EntityFrameworkCore;
// using MobileOperator.Domain.Entities;
//
// namespace MobileOperator.Infrastructure.services
// {
//     public class RateService : IRateService
//     {
//         private readonly Infrastructure.MobileOperator _db;
//
//         public RateService(Infrastructure.MobileOperator context)
//         {
//             _db = context;
//         }
//
//         public async Task<List<Rate>> GetAllAsync()
//         {
//             return await _db.Rate.ToListAsync();
//         }
//
//         public async Task<Rate> GetByIdAsync(int id)
//         {
//             return await _db.Rate.FindAsync(id);
//         }
//
//         public async Task<bool> SaveAsync(Rate rate)
//         {
//             if (rate.Id == 0)
//                 _db.Rate.Add(rate);
//             else
//                 _db.Rate.Update(rate);
//
//             return await _db.SaveChangesAsync() > 0;
//         }
//
//         public async Task<bool> DeleteAsync(int id)
//         {
//             var rate = await _db.Rate.FindAsync(id);
//             if (rate == null) return false;
//
//             _db.Rate.Remove(rate);
//             return await _db.SaveChangesAsync() > 0;
//         }
//     }
// }