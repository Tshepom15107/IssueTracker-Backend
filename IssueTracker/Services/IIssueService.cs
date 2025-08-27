using IssueTracker.Models;

namespace IssueTracker.Services
{
    public interface IIssueService
    {
        Task<PagedResult<Issue>> GetIssuesAsync(IssueFilterDto filter);
        Task<Issue?> GetIssueByIdAsync(int id);
        Task<Issue> CreateIssueAsync(CreateIssueDto createDto);
        Task<Issue?> UpdateIssueAsync(int id, UpdateIssueDto updateDto);
        Task<bool> DeleteIssueAsync(int id);
    }
}
