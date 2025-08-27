using IssueTracker.Models;
using IssueTracker.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private readonly IIssueService _issueService;

        public IssuesController(IIssueService issueService)
        {
            _issueService = issueService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Issue>>> GetIssues([FromQuery] IssueFilterDto filter)
        {
            var result = await _issueService.GetIssuesAsync(filter);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Issue>> GetIssue(int id)
        {
            var issue = await _issueService.GetIssueByIdAsync(id);
            if (issue == null)
                return NotFound();

            return Ok(issue);
        }

        [HttpPost]
        public async Task<ActionResult<Issue>> CreateIssue(CreateIssueDto createDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var issue = await _issueService.CreateIssueAsync(createDto);
            return CreatedAtAction(nameof(GetIssue), new { id = issue.Id }, issue);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Issue>> UpdateIssue(int id, UpdateIssueDto updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var issue = await _issueService.UpdateIssueAsync(id, updateDto);
            if (issue == null)
                return NotFound();

            return Ok(issue);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteIssue(int id)
        {
            var success = await _issueService.DeleteIssueAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}

