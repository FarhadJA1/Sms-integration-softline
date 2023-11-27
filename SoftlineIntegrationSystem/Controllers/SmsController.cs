using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SoftlineIntegrationSystem.Helpers;
using SoftlineIntegrationSystem.Models;
using SoftlineIntegrationSystem.Models.Dtos;
using SoftlineIntegrationSystem.Repositories;
using SoftlineIntegrationSystem.Services;
using System.Text.Json;

namespace SoftlineIntegrationSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SmsController : ControllerBase
{
    private readonly IVenueRepository _venueRepository;
    private readonly IActionRepository _actionRepository;
    private readonly ISmsService _smsService;
    private readonly IIikoService _iikoService;
    private readonly IMemoryCache _cache;


    public SmsController(IVenueRepository venueRepository, IActionRepository actionRepository, ISmsService smsService, IIikoService iikoService, IMemoryCache cache)
    {
        _venueRepository = venueRepository;
        _actionRepository = actionRepository;
        _smsService = smsService;
        _iikoService = iikoService;
        _cache = cache;
    }

    [HttpPost("{id}")]
    public async Task<ActionResult> Post([FromRoute] string id, [FromBody] WebhookSmsBody smsBody)
    {
        VenueDto? venue = await _venueRepository.GetByIdAsync(id);
        if (venue == null || !venue.IsActive || venue.HookPassword != smsBody.subscriptionPassword)
        {
            await _actionRepository.AddAsync(new Models.Entities.Action
            {
                Body = JsonSerializer.Serialize(smsBody),
                CreatedDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                VenueId = id,
                VenueName = "Icazesiz istek",
                IsNotified = false
            });
            return Accepted();
        }

        //first registration
        if (smsBody.sender is not null && smsBody.text is not null)
        {
            SoftlineResponse smsResult = await _smsService.SendSms(new SoftlineRequest(venue.Username, venue.Apikey, smsBody.sender, venue.SenderName, smsBody.text));
            if (smsResult.Balance <= 10 && smsResult.ErrNo == 100)
            {
                string text = $"Restoraniniz {venue.Name} ucun iiko sms integrasiyasi balansi bitmek uzredir";
                SoftlineResponse response = await _smsService.SendSms(new SoftlineRequest(venue.Username, venue.Apikey, venue.NotifiedPersonPhone, venue.SenderName, text));
                await _actionRepository.AddAsync(new Models.Entities.Action
                {
                    CreatedDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                    VenueId = venue.Id,
                    VenueName = venue.Name,
                    IsNotified = smsResult.ErrNo == 100,
                    Body = $"{venue.NotifiedPersonPhone} is notified",
                    Response = JsonSerializer.Serialize(response)
                });
            }
            await _actionRepository.AddAsync(new Models.Entities.Action
            {
                Body = JsonSerializer.Serialize(smsBody),
                CreatedDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                VenueId = venue.Id,
                VenueName = venue.Name,
                IsNotified = smsResult.ErrNo == 100,
                Response = JsonSerializer.Serialize(smsResult)
            });
            return Ok();
        }

        //get customerinfo from iiko and send sms
        if (smsBody.customerId is not null && smsBody.text is not null)
        {
            string token = await GetTokenAsync(venue);
            CustomerInfoResponse? customerInfo = await _iikoService.GetCustomerInfoAsync(smsBody.customerId!, venue.OrganizationId, token);
            if (customerInfo != null)
            {
                SoftlineResponse smsResult = await _smsService.SendSms(new SoftlineRequest(venue.Username, venue.Apikey, customerInfo.phone!, venue.SenderName, smsBody.text));
                if (smsResult.Balance <= 10 && smsResult.ErrNo == 100)
                {
                    string text = $"Restoraniniz {venue.Name} ucun iiko sms integrasiyasi balansi bitmek uzredir";
                    SoftlineResponse response = await _smsService.SendSms(new SoftlineRequest(venue.Username, venue.Apikey, venue.NotifiedPersonPhone, venue.SenderName, text));
                    await _actionRepository.AddAsync(new Models.Entities.Action
                    {
                        CreatedDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                        VenueId = venue.Id,
                        VenueName = venue.Name,
                        IsNotified = smsResult.ErrNo == 100,
                        Body = $"{venue.NotifiedPersonPhone} is notified",
                        Response = JsonSerializer.Serialize(response)
                    });
                }
                await _actionRepository.AddAsync(new Models.Entities.Action
                {
                    Body = JsonSerializer.Serialize(smsBody),
                    CreatedDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                    VenueId = venue.Id,
                    VenueName = venue.Name,
                    IsNotified = smsResult.ErrNo == 100,
                    Response = JsonSerializer.Serialize(smsResult)
                });
                return Ok();
            }
        }

        await _actionRepository.AddAsync(new Models.Entities.Action
        {
            Body = JsonSerializer.Serialize(smsBody),
            CreatedDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
            VenueId = venue.Id,
            VenueName = venue.Name,
            IsNotified = false
        });
        return Accepted();
    }
    [Authorize(Roles = Constants.Admin)]
    [HttpPost("manual/{id}")]
    public async Task<ActionResult> ManualSmsSend([FromRoute] string id, [FromBody] ManualSms manualSms)
    {

        VenueDto? venue = await _venueRepository.GetByIdAsync(id);
        if (venue is null)
            return BadRequest();

        string? email = HttpContext.GetUserEmail();
        if (email is null)
            return BadRequest();

        SoftlineResponse smsResult = await _smsService.SendSms(new SoftlineRequest(venue.Username, venue.Apikey, manualSms.PhoneNumber, venue.SenderName, manualSms.Text));

        await _actionRepository.AddAsync(new Models.Entities.Action
        {
            Body = JsonSerializer.Serialize(new
            {
                detail = $"Manual message sended by {email}",
                data = manualSms
            }),
            CreatedDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
            VenueId = venue.Id,
            VenueName = venue.Name,
            IsNotified = smsResult.ErrNo == 100,
            Response = JsonSerializer.Serialize(smsResult)
        });

        return Accepted();
    }
    private async Task<string> GetTokenAsync(VenueDto venueDto)
    {
        string? token = _cache.Get<string>(venueDto.Id);
        if (token is null)
        {
            token = await _iikoService.GetTokenAsync(venueDto.IIKOApikey);
            _cache.Set(venueDto.Id, token, TimeSpan.FromMinutes(45));
        }
        return token!;
    }

}
