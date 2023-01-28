using LiveClinic.Shared.Domain;

namespace LiveClinic.Billing.Application.Dtos
{
    public class NewBillItemDto
    {
        public long BillId { get;  set;}
        public long EncounterId { get;  set;}
        public Service Service { get;  set;}
    }
}