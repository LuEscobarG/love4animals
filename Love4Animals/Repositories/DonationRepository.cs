using Love4AnimalsApi.Data;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Repositories;

public class DonationRepository : IDonationRepository
{
    private readonly AppDbContext _context;

    public DonationRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<Donation> GetDonations() =>
        _context.Donations.ToList();

    public Donation? GetDonationById(int id) =>
        _context.Donations.FirstOrDefault(d => d.Id == id);

    public Donation CreateDonation(Donation donation)
    {
        _context.Donations.Add(donation);
        _context.SaveChanges();
        return donation;
    }

    public bool UpdateDonation(int id, Donation donation)
    {
        var existing = _context.Donations.FirstOrDefault(d => d.Id == id);
        if (existing == null) return false;

        existing.Amount = donation.Amount;
        _context.SaveChanges();
        return true;
    }

    public bool DeleteDonation(int id)
    {
        var donation = _context.Donations.FirstOrDefault(d => d.Id == id);
        if (donation == null) return false;

        _context.Donations.Remove(donation);
        _context.SaveChanges();
        return true;
    }
}
