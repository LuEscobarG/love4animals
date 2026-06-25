using Love4AnimalsApi.Dtos;
using Love4AnimalsApi.Interfaces;
using Love4AnimalsApi.Models;

namespace Love4AnimalsApi.Services;

public class DonationService : IDonationService
{
    private readonly IDonationRepository _donationRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICampaignRepository _campaignRepository;

    public DonationService(IDonationRepository donationRepository, IUserRepository userRepository, ICampaignRepository campaignRepository)
    {
        _donationRepository = donationRepository;
        _userRepository = userRepository;
        _campaignRepository = campaignRepository;
    }

    private GetDonationDto MapToDto(Donation d)
    {
        var user = _userRepository.GetUserById(d.UserId);
        var campaign = _campaignRepository.GetCampaignById(d.CampaignId);
        return new GetDonationDto(d.Id, d.UserId, user?.Name ?? "Unknown", d.CampaignId, campaign?.Title ?? "Unknown", d.Amount);
    }

    public List<GetDonationDto> GetDonations() =>
        _donationRepository.GetDonations().Select(MapToDto).ToList();

    public GetDonationDto? GetDonationById(int id)
    {
        var donation = _donationRepository.GetDonationById(id);
        return donation == null ? null : MapToDto(donation);
    }

    public GetDonationDto? CreateDonation(CreateDonationDto dto)
    {
        var user = _userRepository.GetUserById(dto.UserId);
        if (user == null || user.UserType != UserType.Donor) return null;

        var donation = new Donation { UserId = dto.UserId, CampaignId = dto.CampaignId, Amount = dto.Amount };
        var created = _donationRepository.CreateDonation(donation);
        return MapToDto(created);
    }

    public GetDonationDto? UpdateDonation(int id, UpdateDonationDto dto)
    {
        var existing = _donationRepository.GetDonationById(id);
        if (existing == null) return null;

        var updated = new Donation { Id = id, UserId = existing.UserId, CampaignId = existing.CampaignId, Amount = dto.Amount };
        var success = _donationRepository.UpdateDonation(id, updated);
        return success ? GetDonationById(id) : null;
    }

    public bool DeleteDonation(int id) =>
        _donationRepository.DeleteDonation(id);
}
