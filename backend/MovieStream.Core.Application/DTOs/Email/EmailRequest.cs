namespace MovieStream.Core.Application.DTOs.Email
{
    public class EmailRequest
    {
        public string To { get; set; }
        public string? Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string HtmlBodyTemplateName { get; set; }
        public Dictionary<string, object> TemplateData { get; set; }
    }
}
