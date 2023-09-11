using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using FileUploadTrigger.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace FileUploadTrigger.Tests.Services
{
    public class BlobServiceTests
    {
        private readonly Mock<BlobServiceClient> _blobServiceClientMock;
        private readonly Mock<BlobContainerClient> _containerClientMock;
        private readonly Mock<BlobClient> _blobClientMock;
        private readonly Mock<IConfiguration> _configurationMock;

        public BlobServiceTests()
        {
            _blobServiceClientMock = new();
            _containerClientMock = new();
            _blobClientMock = new();
            _configurationMock = new();
        }

        [Fact]
        public void GetSasUri_Invoke_ReturnsValidUri()
        {
            // Arrange
            _configurationMock.Setup(c => c["BlobContainerName"])
                .Returns("testBlob");

            _blobServiceClientMock.Setup(x => x.GetBlobContainerClient(It.IsAny<string>()))
                .Returns(_containerClientMock.Object);

            _containerClientMock.Setup(x => x.GetBlobClient(It.IsAny<string>()))
                .Returns(_blobClientMock.Object);

            var uri = new Uri("https://example.com/sas-uri");

            _blobClientMock.Setup(x => x.GenerateSasUri(It.IsAny<BlobSasBuilder>()))
                .Returns(uri);

            var blobService = new BlobService(_blobServiceClientMock.Object, _configurationMock.Object);

            var blobName = "exampleBlob.txt";
            // Act
            var result = blobService.GetSasUri(blobName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(uri.AbsoluteUri, result);
        }
    }
}
