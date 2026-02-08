namespace LibraryManagementSystem.Application.Common.Wrappers
{
    public class ApiResponse<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public T? Data { get; set; }

        public ApiResponse() { }

        public ApiResponse(T data, string message = "")
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public ApiResponse(string message)
        {
            Succeeded = false;
            Message = message;
        }

        public ApiResponse(string message, List<string> errors)
        {
            Succeeded = false;
            Message = message;
            Errors = errors;
        }
    }
}
