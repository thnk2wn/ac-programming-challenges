using System;
using Android.Graphics;

namespace AvantCredit.Uploader.Cloud
{
    class DocumentUploadInfo
    {
        public Bitmap Bitmap { get; set; }

        public String ImageName { get; set; }

        public int UserId { get; set; }

        public string OriginalFilename { get; set; }
    }
}