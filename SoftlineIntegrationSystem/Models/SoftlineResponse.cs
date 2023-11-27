namespace SoftlineIntegrationSystem.Models;

//errno=100&errtext=OK&message_id=815845585&charge=1&balance=57376

public class SoftlineResponse
{
    public int ErrNo { get; set; }
    public string ErrText { get; set; } = default!;
    public long MessageId { get; set; }
    public int Charge { get; set; }
    public long Balance { get; set; }
}
