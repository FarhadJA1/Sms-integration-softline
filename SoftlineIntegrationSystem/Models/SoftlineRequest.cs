namespace SoftlineIntegrationSystem.Models;

public class SoftlineRequest
{
    public SoftlineRequest(string username, string apikey, string number, string senderName, string text)
    {
        Username = username;
        Apikey = apikey;
        Number = number;
        SenderName = senderName;
        Text = text;
    }

    public string Username { get; set; }
    public string Apikey { get; set; }
    public string Number { get; set; }
    public string SenderName { get; set; }
    public string Text { get; set; }
}
