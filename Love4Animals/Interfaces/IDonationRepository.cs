using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Interfaces;

public interface IDonationRepository
{
    List<Donation> GetDonations();
    Donation? GetDonationById(int id);
    Donation CreateDonation(Donation donation);
    bool UpdateDonation(int id, Donation donation);
    bool DeleteDonation(int id);
}
