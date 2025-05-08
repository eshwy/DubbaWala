using Microsoft.AspNetCore.Identity;
using Dabbawalla.Models;
using Dabbawalla.Dto;
using Microsoft.EntityFrameworkCore;
using Dabbawalla.Services;
using System.Security.Cryptography;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

public class UserService
{
    private readonly MyDbContext _dbContext;
    private readonly PasswordHasher<User> _passwordHasher;
    private readonly EmailListenerService _emailListenerService;

    public UserService(MyDbContext dbContext, EmailListenerService emailListenerService)
    {
        _dbContext = dbContext;
        _passwordHasher = new PasswordHasher<User>();
        _emailListenerService = emailListenerService;
    }

    public async Task<bool> RegisterUser(RegisterUserDto registerUserDto)
    {
        // Check if the user already exists
        var existingUser = _dbContext.Users.SingleOrDefault(u => u.EmailAddress.Trim().ToLower() == registerUserDto.EmailAddress.Trim().ToLower());
        if (existingUser != null)
        {
            return false;
        }

        var id = Guid.NewGuid();
        // Create new user entity
        var user = new User
        {
            Id = id,
            Name = registerUserDto.Name,
            EmailAddress = registerUserDto.EmailAddress,
            PhoneNumber = registerUserDto.PhoneNumber,
            PasswordHash=registerUserDto.Password,
            RoleId = 3, 
            CreatedDate = DateTime.Now,
        };

        // Save the user to the database
        _dbContext.Users.Add(user);
        var result = await _dbContext.SaveChangesAsync();
        if (result==1)
        {
            var addAdress = new Address 
                            { 
                                UserId=id,
                                AddressType= registerUserDto.Address.AddressType,
                                DoorNumber= registerUserDto.Address.DoorNumber,
                                Street = registerUserDto.Address.Street,
                                Area = registerUserDto.Address.Area,
                                City= registerUserDto.Address.City,
                                PostalCode= registerUserDto.Address.Postal,
            };
            _dbContext.Addresses.Add(addAdress);
            await _dbContext.SaveChangesAsync();
        }

        return true; // Returns true if the user was successfully saved
    }

    public async Task<bool> RegisterVendor(RegisterVendorDto registerUserDto)
    {
        // Check if the user already exists
        var existingUser = _dbContext.Users.SingleOrDefault(u => u.EmailAddress == registerUserDto.EmailAddress);
        if (existingUser != null)
        {
            throw new Exception("User with this email already exists.");
        }
        var id = Guid.NewGuid();
        // Create new user entity
        var user = new User
        {
            Id = id,
            Name = registerUserDto.Name,
            EmailAddress = registerUserDto.EmailAddress,
            RestrauntName=registerUserDto.RestrauntName,
            PhoneNumber = registerUserDto.PhoneNumber,
            PasswordHash=registerUserDto.Password,
            RoleId = 2,
            BankAccountNumber=registerUserDto.BankAccount,
            BankIfsc=registerUserDto.BankIFSC,
            CreatedDate = DateTime.Now,
        };

        // Save the user to the database
        _dbContext.Users.Add(user);
        var result = _dbContext.SaveChanges();

        //Adding Working Days
        VendorWorkingDays(registerUserDto.WorkingDays, id.ToString());

        //Adding Address
        AddAddress(registerUserDto.Address, id.ToString());

        return result > 0; // Returns true if the user was successfully saved
    }

    private bool VendorWorkingDays(List<string> days,string userId)
    {
        foreach (var day in days)
        {
            var aaadays = _dbContext.WorkingDays.ToList();
            var dayId = _dbContext.WorkingDays.Where(weekday => weekday.Day.ToLower() == day.ToLower()).FirstOrDefault().Id;

            var vendorWorkingDaysInsert = new VendorWorkingDay()
            {
                WorkingDayId=dayId,
                UserId=userId
            };
            _dbContext.Add(vendorWorkingDaysInsert);
            var result = _dbContext.SaveChanges();
        }
        return true;
    }
    private bool AddAddress(AddressDtoAdd address, string userId)
    {
        var addressToAdd = new Address()
        {
            DoorNumber = address.DoorNumber,
            Street = address.Street,
            Area = address.Area,
            City = address.City,
            State = address.State,
            PostalCode = address.PostalCode,
            AddressType = address.AddressType,
            UserId = Guid.Parse(userId)
        };
        _dbContext.Addresses.Add(addressToAdd);
        _dbContext.SaveChanges();
        return true;
    }

    public User? ValidateUser(string email, string plainPassword)
    {
        // Lookup the user in the database by email
        var user = _dbContext.Users.SingleOrDefault(u => u.EmailAddress.Trim().ToLower() == email.Trim().ToLower());

        if (user == null)
        {
            return null; // User not found
        }
        
        if (user.PasswordHash == plainPassword)
        {
            return user;
        }

        return null; 
    }

    public async Task<bool> ChangePassword(string email, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.EmailAddress.Trim().ToLower() == email.Trim().ToLower());

        if (user == null)
            return false;

        user.PasswordHash = GenerateRandomPassword();

        _emailListenerService.SendForgotPassword(user.EmailAddress, user.PasswordHash, cancellationToken);

        _dbContext.Users.Update(user);
        var result = await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public string GenerateRandomPassword(int length = 10)
    {
        const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lower = "abcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";
        const string special = "@#$%^&*!?";
        string allChars = upper + lower + digits + special;

        var passwordChars = new char[length];
        var random = RandomNumberGenerator.Create();

        byte[] buffer = new byte[length];

        random.GetBytes(buffer);

        for (int i = 0; i < length; i++)
        {
            int idx = buffer[i] % allChars.Length;
            passwordChars[i] = allChars[idx];
        }

        return new string(passwordChars);
    }



}
