
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace CMS.Utilities.Helpers
{
    public class ImageResize
    {
        private const int OrientationKey = 0x0112;
        private const int NotSpecified = 0;
        private const int NormalOrientation = 1;
        private const int MirrorHorizontal = 2;
        private const int UpsideDown = 3;
        private const int MirrorVertical = 4;
        private const int MirrorHorizontalAndRotateRight = 5;
        private const int RotateLeft = 6;
        private const int MirorHorizontalAndRotateLeft = 7;
        private const int RotateRight = 8;

        public enum Dimensions
        {
            Width,
            Height
        }

        public enum AnchorPosition
        {
            Top,
            Center,
            Bottom,
            Left,
            Right
        }

        [STAThread]

        public static Image ScaleByPercent(Image imgPhoto, int Percent)
        {
            try
            {
                float nPercent = (float)Percent / 100;

                int sourceWidth = imgPhoto.Width;
                int sourceHeight = imgPhoto.Height;
                int sourceX = 0;
                int sourceY = 0;

                int destX = 0;
                int destY = 0;
                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

                grPhoto.DrawImage(imgPhoto,
                    new Rectangle(destX, destY, destWidth, destHeight),
                    new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);

                grPhoto.Dispose();
                return bmPhoto;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public static Image ConstrainProportions(Image imgPhoto, int Size, Dimensions Dimension)
        {
            try
            {
                int sourceWidth = imgPhoto.Width;
                int sourceHeight = imgPhoto.Height;
                int sourceX = 0;
                int sourceY = 0;
                int destX = 0;
                int destY = 0;
                float nPercent = 0;

                switch (Dimension)
                {
                    case Dimensions.Width:
                        nPercent = Size / (float)sourceWidth;
                        break;
                    default:
                        nPercent = Size / (float)sourceHeight;
                        break;
                }

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

                grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

                grPhoto.Dispose();
                return bmPhoto;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            try
            {
                int sourceWidth = imgPhoto.Width;
                int sourceHeight = imgPhoto.Height;
                int sourceX = 0;
                int sourceY = 0;
                int destX = 0;
                int destY = 0;

                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;

                nPercentW = Width / (float)sourceWidth;
                nPercentH = Height / (float)sourceHeight;

                //if we have to pad the height pad both the top and the bottom
                //with the difference between the scaled height and the desired height
                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentH;
                    destX = (int)((Width - sourceWidth * nPercent) / 2);
                }
                else
                {
                    nPercent = nPercentW;
                    destY = (int)((Height - sourceHeight * nPercent) / 2);
                }

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.Clear(Color.Red);
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

                grPhoto.DrawImage(imgPhoto,
                    new Rectangle(destX, destY, destWidth, destHeight),
                    new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);

                grPhoto.Dispose();
                return bmPhoto;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public static Image Crop(Image imgPhoto, int Width, int Height, AnchorPosition Anchor)
        {
            try
            {
                int sourceWidth = imgPhoto.Width;
                int sourceHeight = imgPhoto.Height;
                int sourceX = 0;
                int sourceY = 0;
                int destX = 0;
                int destY = 0;

                float nPercent = 0;
                float nPercentW = 0;
                float nPercentH = 0;

                nPercentW = Width / (float)sourceWidth;
                nPercentH = Height / (float)sourceHeight;

                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentW;
                    switch (Anchor)
                    {
                        case AnchorPosition.Top:
                            destY = 0;
                            break;
                        case AnchorPosition.Bottom:
                            destY = (int)(Height - sourceHeight * nPercent);
                            break;
                        default:
                            destY = (int)((Height - sourceHeight * nPercent) / 2);
                            break;
                    }
                }
                else
                {
                    nPercent = nPercentH;
                    switch (Anchor)
                    {
                        case AnchorPosition.Left:
                            destX = 0;
                            break;
                        case AnchorPosition.Right:
                            destX = (int)(Width - sourceWidth * nPercent);
                            break;
                        default:
                            destX = (int)((Width - sourceWidth * nPercent) / 2);
                            break;
                    }
                }

                int destWidth = (int)(sourceWidth * nPercent);
                int destHeight = (int)(sourceHeight * nPercent);

                Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

                grPhoto.DrawImage(imgPhoto,
                    new Rectangle(destX, destY, destWidth, destHeight),
                    new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);

                grPhoto.Dispose();
                return bmPhoto;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public static Image Thumbnail(Image image, int Width, int Height)
        {
            try
            {
                Image thumbnailImage = image.GetThumbnailImage(Width, Height, new Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
                return thumbnailImage;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }

        public static bool ThumbnailCallback()
        {
            return true;
        }
        public static Image ResizeImage(Image image, int Width, int Height)
        {
            try
            {
                var newBitmap = new Bitmap(Width, Height);
                using (var imageScaler = Graphics.FromImage(newBitmap))
                {
                    imageScaler.CompositingQuality = CompositingQuality.HighQuality;
                    imageScaler.SmoothingMode = SmoothingMode.HighQuality;
                    imageScaler.InterpolationMode = InterpolationMode.HighQualityBicubic;

                    var imageRectangle = new Rectangle(0, 0, Width, Height);
                    imageScaler.DrawImage(image, imageRectangle);

                    // Fix orientation if needed.
                    bool containsNumber = false;
                    foreach (int n in image.PropertyIdList)
                    {
                        if (n == OrientationKey)
                        {
                            containsNumber = true;
                            break;
                        }
                    }
                    if (containsNumber)
                    {
                        var orientation = (int)image.GetPropertyItem(OrientationKey).Value[0];
                        switch (orientation)
                        {
                            case NotSpecified: // Assume it is good.
                            case NormalOrientation:
                                // No rotation required.
                                break;
                            case MirrorHorizontal:
                                newBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                break;
                            case UpsideDown:
                                newBitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                break;
                            case MirrorVertical:
                                newBitmap.RotateFlip(RotateFlipType.Rotate180FlipX);
                                break;
                            case MirrorHorizontalAndRotateRight:
                                newBitmap.RotateFlip(RotateFlipType.Rotate90FlipX);
                                break;
                            case RotateLeft:
                                newBitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                                break;
                            case MirorHorizontalAndRotateLeft:
                                newBitmap.RotateFlip(RotateFlipType.Rotate270FlipX);
                                break;
                            case RotateRight:
                                newBitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                                break;
                            default:
                                throw new NotImplementedException("An orientation of " + orientation + " isn't implemented.");
                        }
                    }
                }
                return newBitmap;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog(ex.ToString(), new System.Diagnostics.StackTrace().GetFrames()[0].GetMethod().Name);
                throw;
            }
        }
    }
}
