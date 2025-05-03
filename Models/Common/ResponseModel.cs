namespace Mero_Doctor_Project.Models.Common
{
    public class ResponseModel<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }  // Nullable type, can be null when not needed
    }
}
