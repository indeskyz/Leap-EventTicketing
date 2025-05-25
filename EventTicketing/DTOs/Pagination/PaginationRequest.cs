using System.ComponentModel.DataAnnotations;

namespace EventTicketing.DTOs.Pagination
{
    public class PaginationRequest
    {
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1")]
        public int PageNumber { get; set; } = PaginationConstants.DefaultPageNumber;

        [Range(1, PaginationConstants.MaxPageSize, ErrorMessage = "Page size must be between 1 and " + nameof(PaginationConstants.MaxPageSize))]
        public int PageSize { get; set; } = PaginationConstants.DefaultPageSize;
    }
}
