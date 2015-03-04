using System.Collections.Generic;

namespace AvantCredit.Uploader.Cloud
{
    interface IDocumentUploadService
    {
        void UploadImage(DocumentUploadInfo docInfo);
        IList<DocumentInfo> GetUploadedImages(int userId);
    }
}