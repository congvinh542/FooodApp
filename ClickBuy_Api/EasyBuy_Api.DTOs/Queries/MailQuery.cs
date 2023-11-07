using Microsoft.AspNetCore.Http;

namespace ClickBuy_Api.DTOs.Queries
{
    public class MailQuery
    {
        public string? Url { get; set; }
        public IEnumerable<string>? ToEmails { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
