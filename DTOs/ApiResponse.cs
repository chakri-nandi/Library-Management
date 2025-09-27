namespace LibraryManagement.DTOs
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }

        private ApiResponse(bool success, string message, object? data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ApiResponse Ok(object? data = null, string message = "Success")
            => new ApiResponse(true, message, data);

        
        public static ApiResponse Fail(string message, object? data = null)
            => new ApiResponse(false, message, data);
    }
}
