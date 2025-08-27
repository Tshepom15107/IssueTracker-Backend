
using IssueTracker.Data;
using IssueTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace IssueTracker.Services
   
{
    public class IssueService : IIssueService
    {
        private readonly IssueContext _context;

        public IssueService(IssueContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Issue>> GetIssuesAsync(IssueFilterDto filter)
        {
            var query = _context.Issues.AsQueryable();

            // Apply filters
            if (filter.Status.HasValue)
                query = query.Where(i => i.Status == filter.Status.Value);

            if (filter.Priority.HasValue)
                query = query.Where(i => i.Priority == filter.Priority.Value);

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.ToLower();
                query = query.Where(i =>
                    i.Title.ToLower().Contains(search) ||
                    (i.Description != null && i.Description.ToLower().Contains(search)));
            }

            var totalCount = await query.CountAsync();

            var issues = await query
                .OrderByDescending(i => i.CreatedAt)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResult<Issue>
            {
                Data = issues,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<Issue?> GetIssueByIdAsync(int id)
        {
            return await _context.Issues.FindAsync(id);
        }

        public async Task<Issue> CreateIssueAsync(CreateIssueDto createDto)
        {
            var issue = new Issue
            {
                Title = createDto.Title,
                Description = createDto.Description,
                Priority = createDto.Priority,
                Status = IssueStatus.Open,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Issues.Add(issue);
            await _context.SaveChangesAsync();
            return issue;
        }

        public async Task<Issue?> UpdateIssueAsync(int id, UpdateIssueDto updateDto)
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null) return null;

            issue.Title = updateDto.Title;
            issue.Description = updateDto.Description;
            issue.Status = updateDto.Status;
            issue.Priority = updateDto.Priority;
            issue.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return issue;
        }

        public async Task<bool> DeleteIssueAsync(int id)
        {
            var issue = await _context.Issues.FindAsync(id);
            if (issue == null) return false;

            _context.Issues.Remove(issue);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
