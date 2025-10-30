using Models.DTOs;
using Models.Entities;

namespace Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<EnrollmentBulkResponseDto> BulkEnrollStudentsAsync(
            EnrollmentBulkRequestDto request,
            CancellationToken ct = default);

        Task<List<EnrollmentResponseDto>> GetByCommissionIdAsync(int commissionId, CancellationToken ct = default);
        Task<List<EnrollmentDetailDto>> GetAllEnrollmentsWithDetailsAsync(CancellationToken ct = default);

    }
}

