using Microsoft.AspNetCore.Http;
using Application.Services;

namespace Tests.Application.ServicesTests.MessTests;
public class ImageConverterTests
{
    [Fact]
    public async Task ConvertImageToByteArrayAsync_NullImage_ReturnsNull()
    {
        var imageConverter = new MessUtils();

        var result = await imageConverter.ConvertImageToByteArrayAsync(null);

        Assert.Null(result);
    }

    [Fact]
    public async Task ConvertImageToByteArrayAsync_ValidImage_ReturnsByteArray()
    {
        // Arrange
        var imageConverter = new MessUtils();
        var content = "Hello World from a Fake File";
        var fileName = "test.jpg";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;
        var image = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

        // Act
        var result = await imageConverter.ConvertImageToByteArrayAsync(image);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(content.Length, result.Length);
    }
}
