namespace FileUploadTrigger.Services
{
    public interface IBlobService
    {
        string GetSasUri(string blobName);
    }
}
