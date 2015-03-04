using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Android.Graphics;
using EnsureThat;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AvantCredit.Uploader.Cloud
{
    class DocumentUploadService : IDocumentUploadService
    {
        private const string ORIGINAL_FILENAME = "origFilename";
        private const string TIME_TEXT = "localTimeText";

        public void UploadImage(DocumentUploadInfo docInfo)
        {
            ValidateDocInfo(docInfo);

            var container = GetContainer(docInfo.UserId, createIfNotFound:true);
            var blob = CreateBlob(docInfo, container);

            using (var stream = new MemoryStream())
            {
                docInfo.Bitmap.Compress(Bitmap.CompressFormat.Jpeg, 0, stream);
                stream.Seek(0, SeekOrigin.Begin);

                // think we may need a "non-hacked up to run on this platform version of azure dll"
                // http://stackoverflow.com/questions/23987351/strange-sudden-error-the-number-of-bytes-to-be-written-is-greater-than-the-spec
                try
                {
                    blob.UploadFromStream(stream);
                }
                catch (ProtocolViolationException protocolEx)
                {
                    System.Diagnostics.Trace.WriteLine(protocolEx);
                }
                catch (StorageException storagEx)
                {
                    System.Diagnostics.Trace.WriteLine(storagEx);
                }
                finally
                {
                    try
                    {
                        SetBlobMetadata(blob, docInfo);
                    }
                    catch (Exception metaEx)
                    {
                        // not critical currently
                        System.Diagnostics.Trace.WriteLine(metaEx);
                    }
                }
            }
        }

        private static CloudBlobContainer GetContainer(int userId, bool createIfNotFound = false)
        {
            var client = CreateClient();
            var container = client.GetContainerReference(string.Format("user-{0}", userId));

            if (createIfNotFound)
                container.CreateIfNotExists();
            return container;
        }

        private static string GetMetadata(ICloudBlob blob, string key)
        {
            var value = blob.Metadata.ContainsKey(key) ? blob.Metadata[key] : null;
            return value;
        }

        public IList<DocumentInfo> GetUploadedImages(int userId)
        {
            var container = GetContainer(userId);
            var blobs = container.ListBlobs().ToList();
            var list = new List<DocumentInfo>();

            foreach (var item in blobs)
            {
                var blockBlob = item as CloudBlockBlob;

                if (null != blockBlob)
                {
                    blockBlob.FetchAttributes();
                    var docInfo = new DocumentInfo
                        {
                            ImageName = blockBlob.Name,
                            OriginalFilename = GetMetadata(blockBlob, ORIGINAL_FILENAME),
                            TimeText = GetMetadata(blockBlob, TIME_TEXT),
                            Uri = blockBlob.Uri
                        };
                    list.Add(docInfo);
                }
            }

            return list;
        }

        private static CloudBlockBlob CreateBlob(DocumentUploadInfo docInfo, CloudBlobContainer container)
        {
            var blobName = string.Format("{0}.jpg", docInfo.ImageName);
            var blob = container.GetBlockBlobReference(blobName);
            return blob;
        }

        // this has to be done after the blob is created, can't be done on insert
        private void SetBlobMetadata(ICloudBlob blob, DocumentUploadInfo docInfo)
        {
            blob.Metadata["userId"] = docInfo.UserId.ToString();
            blob.Metadata[ORIGINAL_FILENAME] = docInfo.OriginalFilename;
            blob.Metadata[TIME_TEXT] = DateTime.Now.ToString("G");
            blob.Metadata["utcTime"] = DateTime.UtcNow.ToString("O");
            blob.SetMetadata();
            // might consider other like device name, etc.
        }

        private static void ValidateDocInfo(DocumentUploadInfo docInfo)
        {
            Ensure.That(docInfo, "docInfo").IsNotNull();
            Ensure.That(docInfo.UserId, "UserId").IsGt(0);
            Ensure.That(docInfo.ImageName, "ImageName").IsNotNullOrWhiteSpace();
            Ensure.That(docInfo.Bitmap, "Bitmap").IsNotNull();

            var imageNameRegEx = new Regex(@"^[A-Za-z\d_-]+$");
            if (!imageNameRegEx.IsMatch(docInfo.ImageName))
                throw new InvalidOperationException("Image name should be alphanumeric or with hypens or underscores only");
        }

        private static CloudBlobClient CreateClient()
        {
            var creds = new StorageCredentials("acupload", "GZ0qUvuUfef3RPwfRmaHpWbSCTjWIZx9x4vaUdN8k/+zLdeqzbsUwru2AXTmSD0VrKdR2xfJKq6XQjTFqy3rVA==");
            var client = new CloudStorageAccount(creds, true).CreateCloudBlobClient();
            return client;
        }
    }
}