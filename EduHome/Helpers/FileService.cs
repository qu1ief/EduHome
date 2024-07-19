namespace EduHome.Helpers;

public static class FileService
{
    public static bool ValidateSize(this IFormFile file, int mb)
    {
        return file.Length < mb * 1024 * 1024;
    }

    public static bool ValidateType(this IFormFile file, string type = "image")
    {
        return file.ContentType.Contains(type);
    }

    public static bool ValidateImage(this IFormFile file, int mb = 2, string type = "image")
    {
        return ValidateSize(file, mb) && ValidateType(file, type);
    }

    public static async Task<string> CreateFileAsync(this IFormFile file, string path)
    {
        string filename = Guid.NewGuid().ToString() + file.FileName.Substring(file.FileName.LastIndexOf('.'));

        path = Path.Combine(path, filename);

        using (FileStream stream = new(path, FileMode.CreateNew))
        {
            await file.CopyToAsync(stream);
        }

        return filename;
    }

}
