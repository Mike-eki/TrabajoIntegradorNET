using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET
{
    public interface IRefreshTokenRepository
    {
        Task CreateAsync(RefreshToken token, CancellationToken ct = default);
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default);
        Task RevokeAsync(string token, CancellationToken ct = default);
        Task RevokeAllByUserIdAsync(int userId, CancellationToken ct = default);
    }
}
