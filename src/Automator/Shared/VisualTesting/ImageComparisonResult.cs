using System.Drawing;

namespace Automator.Shared.VisualTesting
{
    /// <summary>
    /// Representation of image comparison result.
    /// </summary>
    public class ImageComparisonResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageComparisonResult"/> class.
        /// </summary>
        /// <param name="diff">Percent difference as double.</param>
        /// <param name="actualImage">Actual image as Bitmap.</param>
        /// <param name="diffImage">Diff image as Bitmap.</param>
        /// <param name="expectedImage">Expected image as Bitmap.</param>
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
