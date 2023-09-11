using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FileUploadTrigger
{
    public class Function1
    {
        [FunctionName("Function1")]
        public void Run([BlobTrigger("fileupload/{name}", Connection = "DefaultEndpointsProtocol=https;AccountName=filesuploadstorageacc;AccountKey=hBExBBT33TWSHf1H8CHZdbt32A7Ytm5C5e6R7/wRdBQh3EvxalhiEyzlaepOo/hX2jaEzajC+WZP+ASthYNKPQ==;EndpointSuffix=core.windows.net")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
