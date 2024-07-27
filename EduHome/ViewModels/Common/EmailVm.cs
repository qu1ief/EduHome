namespace EduHome.ViewModels.Common;

public class EmailVm
{
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }

    public EmailVm(string to,string subject,string body)
    {
        Body = body;
        To = to;
        Subject = subject;
    }
}
