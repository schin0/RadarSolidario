using System.Text.Json;
using Core.DTOs;

namespace UnitTests.Dto;

public class MessageDtoTests
{
    [Fact]
    public void Properties_Should_Set_And_Get_Correctly()
    {
        var dto = new MessageDto
        {
            RecipientNumber = "5511999999999",
            MessageContent = "Olá, tudo bem?"
        };

        Assert.Equal("5511999999999", dto.RecipientNumber);
        Assert.Equal("Olá, tudo bem?", dto.MessageContent);
    }

    [Fact]
    public void Should_Serialize_And_Deserialize_Correctly()
    {
        var dto = new MessageDto
        {
            RecipientNumber = "5511888888888",
            MessageContent = "Mensagem de teste"
        };

        var json = JsonSerializer.Serialize(dto);
        var deserialized = JsonSerializer.Deserialize<MessageDto>(json)!;

        Assert.Equal(dto.RecipientNumber, deserialized.RecipientNumber);
        Assert.Equal(dto.MessageContent, deserialized.MessageContent);
    }
}
