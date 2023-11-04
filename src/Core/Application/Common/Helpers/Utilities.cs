using Microsoft.AspNetCore.Http;

namespace Application.Common.Helpers;

public static class Utilities
{
    public static string GenerateRandomString(int length)
    {
        if (length <= 0)
            throw new ArgumentException("can't generate random string less than 1 length.");
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var stringChars = new char[length];
        var random = new Random();

        for (var i = 0; i < stringChars.Length; i++) stringChars[i] = chars[random.Next(chars.Length)];

        var finalString = new string(stringChars);
        return finalString;
    }

    public static int GenerateRandomNumbers(int length)
    {
        if (length <= 0)
            throw new ArgumentException("can't generate random string less than 1 length.");
        var chars = "0123456789";
        var stringChars = new char[length];
        var random = new Random();

        for (var i = 0; i < stringChars.Length; i++) stringChars[i] = chars[random.Next(chars.Length)];

        var finalString = new string(stringChars);
        return int.Parse(finalString);
    }
    public static  async Task<Stream> ConvertToStream(this IFormFile formFile)
    {
        if (formFile == null || formFile.Length == 0)
        {
            return null;
        }

        var stream = new MemoryStream();

        await using (var fileStream = formFile.OpenReadStream())
        {
            await fileStream.CopyToAsync(stream);
        }

        stream.Seek(0, SeekOrigin.Begin); // Reset the stream position to the beginning

        return stream;
    }
   






}