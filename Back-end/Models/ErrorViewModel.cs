namespace Backend.Models
{
    public class ErrorViewModel
    {
        public string? Message { get; set; }

        public ErrorViewModel() { }

        public ErrorViewModel(string? message)
        {
            Message = message;
        }
    }
}