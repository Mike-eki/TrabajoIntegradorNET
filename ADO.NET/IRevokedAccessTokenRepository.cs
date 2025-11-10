using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET
{
    public interface IRevokedAccessTokenRepository
    {
        Task RevokeAsync(string token, CancellationToken ct = default);
        Task<bool> IsRevokedAsync(string token, CancellationToken ct = default);
    }
}
