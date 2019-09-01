using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace BookWave.Desktop.Util
{
    public class ImageConverter
    {
        public static bool FileIsValid(string path)
        {
            return Regex.IsMatch(Path.GetExtension(path), ConfigurationManager.AppSettings.Get("allowed_image_extensions_regex"));
        }

        public static void SaveCompressedImage(Image image, string saveToPath, int quality=80)
        {
            ImageCodecInfo jpegCodec = GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);

            EncoderParameter qualityParameter = new EncoderParameter(
                        System.Drawing.Imaging.Encoder.Quality, quality);

            EncoderParameters codecParameter = new EncoderParameters(1);
            codecParameter.Param[0] = qualityParameter;

            if (File.Exists(saveToPath))
            {
                File.Delete(saveToPath);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(saveToPath));

            image.Save(saveToPath, jpegCodec, codecParameter);
        }

        private static ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        public static Image Resize(Image image, int newWidth, int maxHeight, bool onlyResizeIfWider = false)
        {
            if (onlyResizeIfWider && image.Width <= newWidth) newWidth = image.Width;

            var newHeight = image.Height * newWidth / image.Width;
            if (newHeight > maxHeight)
            {
                newWidth = image.Width * maxHeight / image.Height;
                newHeight = maxHeight;
            }

            var res = new Bitmap(newWidth, newHeight);

            using (var graphic = Graphics.FromImage(res))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return res;
        }

    }
}
