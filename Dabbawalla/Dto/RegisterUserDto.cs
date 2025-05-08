using System.ComponentModel.DataAnnotations;

namespace Dabbawalla.Dto
{
    public class RegisterUserDto
    {
        public string Name { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public AddressDto Address { get; set; }

    }

    public class AddressDto
    {
        public string AddressType { get; set; }
        public string DoorNumber { get; set; }
        public string Street { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string Postal { get; set; }
    }

}
