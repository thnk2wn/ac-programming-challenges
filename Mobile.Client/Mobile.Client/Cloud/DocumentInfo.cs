using System;

namespace AvantCredit.Uploader.Cloud
{
    class DocumentInfo
    {
        public String ImageName { get; set; }

        public int UserId { get; set; }

        public string OriginalFilename { get; set; }

        public string TimeText { get; set; }

        public Uri Uri { get; set; }
    }
}