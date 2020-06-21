using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Emgu.CV.OCR;
using Emgu.CV;
using Emgu.CV.Structure;
using System.IO;
using System.Windows.Forms;
using System.Threading;



namespace test_and_try_project
{
    public class Program
    {
        static void GetUserData()
        {
            string s2 = "My other string";
            s2 = "New string value";
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }
        public void ConvertImageToText()
        {

            //Creating a Rectangle object which will  
            //capture our Current Screen
            Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
            //Creating a new Bitmap object
            Bitmap captureBitmap = new Bitmap(captureRectangle.Right - captureRectangle.Left, captureRectangle.Bottom - captureRectangle.Top, PixelFormat.Format32bppArgb);
            //Bitmap captureBitmap = new Bitmap(int width, int height, PixelFormat);
            //Creating a New Graphics Object
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);
            //Copying Image from The Screen
            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);
            //Saving the Image File

            captureBitmap.Save(@"C:\Users\Acer PC\Desktop\New folder\Capture.jpg", ImageFormat.Jpeg);
            using (var image = new Image<Bgr, byte>(Path.GetFullPath(@"C:\Users\Acer PC\Desktop\New folder\Capture.jpg")))
            {
                using (var tess = new Tesseract("C:\\Users\\Acer PC\\source\\repos\\test and try project\\test and try project\\obj\\Debug\\tessdata", "eng", OcrEngineMode.TesseractCubeCombined))
                {
                    tess.Recognize(image);
                    string text = tess.GetText().TrimEnd();
                    Console.WriteLine(text);
                }
            }
        }
        static void Main(string[] args)
        {
            
            Console.WriteLine("***** Basic Console I/O *****");
            GetUserData();
            Console.WriteLine("Starting the process...");
            Thread.Sleep(5000);
            Program obj = new Program();
            obj.ConvertImageToText();
            Console.WriteLine("Ended the process...");
            Console.ReadLine();
            File.Delete("C:\\Users\\Acer PC\\Desktop\\New folder\\Capture.jpg");
        }
    }
}
