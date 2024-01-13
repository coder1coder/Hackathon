using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bogus;
using Hackathon.Common.Models.Event;
using Hackathon.Common.Models.User;
using Hackathon.DAL.Entities.Event;
using Hackathon.DAL.Mappings;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Hackathon.Tests.Integration;

public class TestFaker
{
    public const int EventTasksAmount = 5;

    private readonly IMapper _mapper;

    public TestFaker(IMapper mapper)
    {
        _mapper = mapper;
    }

    #region Models

    public static IEnumerable<SignUpModel> GetSignUpModels(int count)
    {
        var faker = new Faker<SignUpModel>();

        faker
            .RuleFor(x => x.UserName, f => f.Person.UserName)
            .RuleFor(x => x.Password, f => f.Random.String2(6, 20))
            .RuleFor(x => x.FullName, f => f.Person.FullName)
            .RuleFor(x => x.Email, f => f.Person.Email);

        return faker.Generate(count);
    }

    public IEnumerable<EventModel> GetEventModels(int count, long userId, EventStatus? eventStatus = EventStatus.Draft)
    {
        var eventEntities = GetEventEntities(count, userId, eventStatus);
        return _mapper.Map<List<EventModel>>(eventEntities);
    }

    public static IFormFile GetEmptyImage(MemoryStream stream, int width, int height)
    {
        using var image = new Image<Rgba32>(width, height);
        image.Mutate(ctx => ctx.BackgroundColor(Rgba32.ParseHex("#ffffff")));
        image.SaveAsJpeg(stream);

        var fileName = $"{Guid.NewGuid()}.jpg";
        var fileMock = new FormFile(stream, 0, stream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/jpeg",
            ContentDisposition = $"form-data; name=\"file\"; filename=\"{fileName}\""
        };

        return fileMock;
    }

    #endregion

    private static List<EventEntity> GetEventEntities(int count, long ownerId, EventStatus? eventStatus = EventStatus.Draft)
        => new Faker<EventEntity>()
            .RuleFor(x=>x.Id, f => f.Random.Long(1))
            .RuleFor(x => x.Name, f => f.Random.String2(6, 20))
            .RuleFor(x => x.Start, DateTime.UtcNow.AddDays(1))
            .RuleFor(x => x.IsCreateTeamsAutomatically, true)
            .RuleFor(x => x.MaxEventMembers, _ => 30)
            .RuleFor(x => x.MinTeamMembers, _ => 1)
            .RuleFor(x => x.Status, _ => eventStatus)
            .RuleFor(x => x.OwnerId, ownerId)
            .RuleFor(x => x.ChangeEventStatusMessages, _ => new List<ChangeEventStatusMessage>())
            .RuleFor(x => x.Award, f => f.Random.Number(1, 1000).ToString())
            .RuleFor(x => x.Description, f => f.Random.String2(400))
            .RuleFor(x => x.Agreement, f=>new EventAgreementEntity
            {
                Rules = f.Random.String2(10, 300),
                RequiresConfirmation = false
            })
            .RuleFor(x => x.Stages, (f, e) => new List<EventStageEntity>
            {
                new ()
                {
                    Id = f.Random.Long(1),
                    Name = "Этап №1",
                    Duration = 5,
                    EventId = e.Id
                }
            })
            .RuleFor(x=>x.Tasks, f=> f.Random.WordsArray(EventTasksAmount).Select(x=>new EventTaskItem
            {
                Title = x
            }).ToArray())
            .RuleFor(x=>x.Tags, f =>
                string.Join(TagsMappings.TagsSeparator, f.Make(3, () => f.Random.String2(3, 30))))
            .Generate(count);
}
