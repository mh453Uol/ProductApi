using Newtonsoft.Json;

namespace RefactorThis.Core.Dtos
{
    public class ErrorMessageDto
    {
        public ErrorMessageDto(int statusCode, string message)
        {
            this.StatusCode = statusCode;
            this.Message = message;
        }

        public string Message { get; set; }
        public int StatusCode { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}