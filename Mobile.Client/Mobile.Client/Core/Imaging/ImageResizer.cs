using Android.Graphics;

namespace AvantCredit.Uploader.Core.Imaging
{
    public static class ImageResizer
    {
        public static Bitmap LoadAndResizeBitmap(string filename, int width, int height)
        {
            // get the the dimensions of the file on disk
            var options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(filename, options);

            // calculate the ratio that we need to resize the image by in order to fit the requested dimensions
            var outHeight = options.OutHeight;
            var outWidth = options.OutWidth;
            var inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            // load the image and have BitmapFactory resize it for us
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            var resizedBitmap = BitmapFactory.DecodeFile(filename, options);
            return resizedBitmap;
        }
    }
}