
namespace Billit_Net.DTO
{
    public class AccountInformation
    {
        public List<Company> Companies { get; set; } = new();
        public string Email { get; set; } = default!;
    }
}
