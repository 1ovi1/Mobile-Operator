// using Microsoft.EntityFrameworkCore;
// using MobileOperator.Domain.Entities;
//
// namespace MobileOperator.Infrastructure.services
// {
//     public class ClientService : IClientService
//     {
//         private readonly Infrastructure.MobileOperator _db;
//
//         public ClientService(Infrastructure.MobileOperator context)
//         {
//             _db = context;
//         }
//
//         public async Task<List<Client>> GetAllClientsAsync()
//         {
//             return await _db.Client.ToListAsync();
//         }
//
//         public async Task<List<FL>> GetAllFLAsync()
//         {
//             return await _db.FL.Include(f => f.Client).ToListAsync();
//         }
//
//         public async Task<List<UL>> GetAllULAsync()
//         {
//             return await _db.UL.Include(u => u.Client).ToListAsync();
//         }
//
//         public async Task<Client> GetByIdAsync(int id)
//         {
//             return await _db.Client.FindAsync(id);
//         }
//
//         public async Task<List<Client>> GetByRateIdAsync(int rateId)
//         {
//             return await _db.Client.Where(c => c.RateId == rateId).ToListAsync();
//         }
//
//         public async Task<bool> SaveAsync(Client client)
//         {
//             if (client.UserId== 0)
//                 _db.Client.Add(client);
//             else
//                 _db.Client.Update(client);
//
//             return await _db.SaveChangesAsync() > 0;
//         }
//
//         public async Task<bool> DeleteAsync(int id)
//         {
//             var client = await _db.Client.FindAsync(id);
//             if (client == null) return false;
//
//             _db.Client.Remove(client);
//             return await _db.SaveChangesAsync() > 0;
//         }
//
//         public async Task<bool> AddRateHistoryAsync(int clientId, int rateId)
//         {
//             var oldHistory = await _db.RateHistory
//                 .Where(r => r.ClientId == clientId && r.TillDate == null)
//                 .FirstOrDefaultAsync();
//
//             if (oldHistory != null)
//                 oldHistory.TillDate = DateTime.Now;
//             
//             _db.RateHistory.Add(new RateHistory
//             {
//                 ClientId = clientId,
//                 RateId = rateId,
//                 FromDate = DateTime.Now
//             });
//
//             return await _db.SaveChangesAsync() > 0;
//         }
//     }
// }
