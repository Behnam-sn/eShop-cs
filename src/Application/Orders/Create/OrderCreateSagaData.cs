using Rebus.Sagas;

namespace Application.Orders.Create;

public class OrderCreateSagaData : ISagaData
{
    public Guid Id { get; set; }

    public int Revision { get; set; }

    public Guid OrderId { get; set; }

    public bool WelcomeEmailSent { get; set; }

    public bool PaymentRequestSent { get; set; }

    public bool ConfirmEmailSent { get; set; }
}
