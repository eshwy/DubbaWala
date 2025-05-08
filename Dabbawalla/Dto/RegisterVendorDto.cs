global using System.ComponentModel.DataAnnotations;

namespace Dabbawalla.Dto
{
    public class RegisterVendorDto
    {   
        public string Name { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Pancard { get; set; } = null!;
        public string BankIFSC { get; set; } = null!;
        public string BankAccount { get; set; } = null!;
        public string RestrauntName { get; set; } = null!;
        public List<String> WorkingDays { get; set; } = null!;
        public AddressDtoAdd Address { get; set; }
        
    }
}
