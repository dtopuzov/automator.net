using System.Drawing;

namespace Automator.Visual
{
    public class ImageComparisonResult
    {
        public ImageComparisonResult(double diff, Bitmap actualImage, Bitmap diffImage, Bitmap expectedImage)
        {
            Diff = diff;
            ActualImage = actualImage;
            DiffImage = diffImage;
            ExpectedImage = expectedImage;
        }

        public double Diff { get; set; }

        public Bitmap ActualImage { get; set; }

        public Bitmap DiffImage { get; set; }

        public Bitmap ExpectedImage { get; set; }
    }
}
