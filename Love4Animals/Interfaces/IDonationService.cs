using Love4AnimalsApi.Dtos;

namespace Love4AnimalsApi.Interfaces;

public interface IDonationService
{
    List<GetDonationDto> GetDonations();
    GetDonationDto? GetDonationById(int id);
    GetDonationDto? CreateDonation(CreateDonationDto dto);
    GetDonationDto? UpdateDonation(int id, UpdateDonationDto dto);
    bool DeleteDonation(int id);
}
