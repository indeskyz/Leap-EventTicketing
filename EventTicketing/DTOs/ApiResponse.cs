namespace EventTicketing.DTOs
{
    public class ApiResponse<T>
    {
        public T Items { get; set; }
        public bool Success { get; set; } = true;
        public string? Message { get; set; }

        public ApiResponse(T items, string? message = null)
        {
            Items = items;
            Message = message;
        }
    }
}
