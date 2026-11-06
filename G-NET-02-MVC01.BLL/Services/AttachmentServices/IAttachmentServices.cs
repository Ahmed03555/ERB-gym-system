using System;
using System.Collections.Generic;
using System.Text;

namespace G_NET_02_MVC01.BLL.Services.AttachmentServices
{
    public interface IAttachmentServices
    {
        Task<string?> UpdateAsync(Stream fileStream,string fileName,string folderName,CancellationToken cancellationToken);

        bool Delete(string fileName, string folderName);

        (Stream Stream, string ContentType)? GetFile(string fileName, string folderName);
    }
}
