using System;

namespace LiveClinic.Shared.Common
{
    public static class LiveUtils
    {
        public static long GenerateId() => new Random().NextInt64(1000, 9999);
        public static string GenerateMemberNo(this long id) => $"KE-UHC-{id}";
        public static string GenerateInvoice(this long id) => $"INV-{id}";
        public static string GeneratePayment(this long id) => $"RP-{id}";
    }
}