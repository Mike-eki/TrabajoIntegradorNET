using Models.DTOs;
using Models.Entities;

namespace Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<EnrollmentBulkResponseDto> BulkEnrollStudentsAsync(
            EnrollmentBulkRequestDto request,
            CancellationToken ct = default);
        Task SelfEnrollAsync(int studentId, int commissionId, CancellationToken ct);
        Task<List<EnrollmentResponseDto>> GetByCommissionIdAsync(int commissionId, CancellationToken ct = default);
        Task<List<EnrollmentDetailDto>> GetAllEnrollmentsWithDetailsAsync(CancellationToken ct = default);

        Task<List<int>> GetUserCommissionIdsAsync(int userId, CancellationToken ct);

        Task<List<AcademicStatusDto>> GetAcademicStatusByCareerAsync(
                int userId,
                int careerId,
                CancellationToken ct = default);
    }
}

