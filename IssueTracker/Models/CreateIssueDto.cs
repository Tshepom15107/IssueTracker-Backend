using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Models
{
    public class CreateIssueDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public Priority Priority { get; set; } = Priority.Medium;
    }

    public class UpdateIssueDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public IssueStatus Status { get; set; }

        public Priority Priority { get; set; }
    }

    public class IssueFilterDto
    {
        public IssueStatus? Status { get; set; }
        public Priority? Priority { get; set; }
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class PagedResult<T>
    {
        public List<T> Data { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}

