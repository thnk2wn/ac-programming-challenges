using System;
using System.Linq;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Views;
using Android.Widget;
using AvantCredit.Uploader.Cloud;
using AvantCredit.Uploader.Commands;
using AvantCredit.Uploader.Core.DI;
using AvantCredit.Uploader.Core.Imaging;
using AvantCredit.Uploader.Core.Security;
using AvantCredit.Uploader.Core.Window;
using Java.IO;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;

namespace AvantCredit.Uploader.Activities
{
    [Activity(Label = "Upload New Document")]
    public class UploadNewDocActivity : Activity
    {
        private readonly IMessageBoxService _messageBoxService;
        private readonly IDocumentUploadService _documentUploadService;
        private readonly IUserAuthService _userAuthService;

        public UploadNewDocActivity()
        {
            _messageBoxService = IoC.Get<IMessageBoxService>();
            _documentUploadService = IoC.Get<IDocumentUploadService>();
            _userAuthService = IoC.Get<IUserAuthService>();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            EnsureStillAuthenicated();
            SetContentView(Resource.Layout.UploadNewDoc);
            PictureImageViewer.Visibility = ViewStates.Invisible;
            this.CanTakePictures = HasCameraApp();

            if (this.CanTakePictures)
            {
                EnsurePictureDirectory();
            }
            else
            {
                ShowCameraUnavailableMessage();
            }

            var takePictureButton = FindViewById<Button>(Resource.Id.takePictureButton);
            takePictureButton.Click += (sender, args) => TakePicture();
            SendButton.Click += (sender, args) => SendDocument();
            SendButton.Enabled = false;
        }

        private void EnsureStillAuthenicated()
        {
            if (!_userAuthService.Authenticate().Success)
            {
                new LogoutCommand(_userAuthService, this).Execute();
            }
        }

        private void SendDocument()
        {
            EnsureStillAuthenicated();
            SendButton.Text = "Sending...";
            var progressDialog = ProgressDialog.Show(this, "Uploading...", "This may take a minute, please wait...", true);
            Exception error = null;

            new Thread(new ThreadStart(delegate
            {
                try
                {
                    using (var bitmap = GetResizedBitmap())
                    {
                        var docInfo = GetDocInfo(bitmap);
                        _documentUploadService.UploadImage(docInfo);
                    }
                }
                catch (Exception threadEx)
                {
                    error = threadEx;
                    System.Diagnostics.Trace.WriteLine(error);
                }

                RunOnUiThread(() =>
                {
                    progressDialog.Hide();

                    if (null != error)
                    {
                        SendButton.Text = "Send";
                        _messageBoxService.ShowAlert("Your image failed to upload. Please try again in a few minutes.", this);
                    }
                    else
                    {
                        SendButton.Text = "Sent";
                        SendButton.Enabled = false;
                        _messageBoxService.ShowOk("Image uploaded. Please allow 24 hours for processing.", this);
                    }
                });
            })).Start();
        }

        private DocumentUploadInfo GetDocInfo(Bitmap bitmap)
        {
            var docInfo = new DocumentUploadInfo
                {
                    UserId = _userAuthService.CurrentUser.UserId,
                    Bitmap = bitmap,
                    ImageName = PictureData.ImageName,
                    OriginalFilename = PictureData.File.Path
                };
            return docInfo;
        }

        private void ShowCameraUnavailableMessage()
        {
            _messageBoxService.ShowOk("No camera apps detected; unable to take a photo.", this);
        }

        private ImageView PictureImageViewer
        {
            get { return FindViewById<ImageView>(Resource.Id.pictureImageViewer); }
        }

        private Button SendButton
        {
            get { return FindViewById<Button>(Resource.Id.sendButton); }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            // make it available in the gallery
            var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            var contentUri = Uri.FromFile(PictureData.File);
            mediaScanIntent.SetData(contentUri);
            SendBroadcast(mediaScanIntent);

            PictureImageViewer.Visibility = ViewStates.Visible;

            using (var bitmap = GetResizedBitmap())
            {
                PictureImageViewer.SetImageBitmap(bitmap);
            }

            //TODO: rotate option
            SendButton.Enabled = true;
        }

        private Bitmap GetResizedBitmap()
        {
            // Display in ImageView. We will resize the bitmap to fit the display. Loading the  
            // full-sized image will consume too much memory and cause app to crash.
            // even keeping a reference to bitmap which we save first time seemed to cause issues
            // resizing again for now
            var bitmap = ImageResizer.LoadAndResizeBitmap(PictureData.File.Path, PictureImageViewer.Width,
                                                          Resources.DisplayMetrics.HeightPixels);
            return bitmap;
        }

        //TODO: move some of the camera and imaging stuff out into more reusable classes later
        private bool HasCameraApp()
        {
            var availableActivities = PackageManager.QueryIntentActivities(new Intent(MediaStore.ActionImageCapture), 
                PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Any();
        }

        private static void EnsurePictureDirectory()
        {
            PictureData.Dir = new File(Environment.GetExternalStoragePublicDirectory(Environment.DirectoryPictures), 
                "AvantCreditUploader");
            if (!PictureData.Dir.Exists())
            {
                PictureData.Dir.Mkdirs();
            }
        }

        private void TakePicture()
        {
            if (!CanTakePictures)
            {
                ShowCameraUnavailableMessage();
                return;
            }

            var intent = new Intent(MediaStore.ActionImageCapture);
            PictureData.ImageName = string.Format("Document_{0}", DateTime.Now.Ticks);
            PictureData.File = new File(PictureData.Dir, string.Format("{0}.jpg", PictureData.ImageName));
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(PictureData.File));
            StartActivityForResult(intent, 0);
        }

        private static class PictureData
        {
            public static File File { get; set; }
            public static File Dir { get; set; }
            public static string ImageName { get; set; }
        }

        private bool CanTakePictures { get; set; }
    }
}