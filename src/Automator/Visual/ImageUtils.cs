using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using OpenQA.Selenium;

namespace Automator.Visual
{
    public static class ImageUtils
    {
        /// <summary>
        /// Read image from file.
        /// </summary>
        /// <param name="filePath">Path.</param>
        /// <returns>Bitmap.</returns>
        public static Bitmap ReadImageFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                return new Bitmap(filePath);
            }
            else
            {
                throw new FileNotFoundException("Failed to find image at path: " + filePath);
            }
        }

        /// <summary>
        /// Convert a screenshot to Bitmap image.
        /// </summary>
        /// <param name="screenshot">The screenshot to convert.</param>
        /// <returns>Bitmap.</returns>
        public static Bitmap ConvertToBitmap(Screenshot screenshot)
        {
            return Image.FromStream(new MemoryStream(screenshot.AsByteArray)) as Bitmap;
        }

        /// <summary>
        /// Wait for image compare.
        /// </summary>
        /// <param name="screenshot">Screenshot function that returns Bitmap.</param>
        /// <param name="path">Path to expected image.</param>
        /// <param name="timeout">Timeout for image comparison.</param>
        /// <param name="tolerance">Allowed tolerance in percents.</param>
        /// <returns>ImageComparisonResult.</returns>
        public static bool Compare(Func<Bitmap> screenshot, string path, int timeout = 30, double tolerance = 0.1)
        {
            bool success = false;
            ImageComparisonResult result = null;

            TimeSpan maxDuration = TimeSpan.FromSeconds(timeout);
            Stopwatch sw = Stopwatch.StartNew();

            if (!File.Exists(path))
            {
                Thread.Sleep(3000);
                Bitmap actualImage = screenshot();
                actualImage.Save(path);
                actualImage.Dispose();

                var message = $"Can not find expected image at {path}, save actual as expected!";
                throw new FileNotFoundException(message);
            }

            var expectedImage = new Bitmap(path);
            while ((!success) && (sw.Elapsed < maxDuration))
            {
                try
                {
                    Thread.Sleep(250);
                    Bitmap actualImage = screenshot();
                    result = CompareBitmapImages(actualImage, expectedImage);
                    if (result == null)
                    {
                        // Exit the loop if the target image is not available. The actual image is already saved and no need to wait.
                        success = true;
                    }
                    else
                    {
                        success = result.Diff < tolerance;
                    }
                }
                catch
                {
                    success = false;
                }
            }

            // If images do not match then save actual and diff relative to path folder.
            if (!success && result != null)
            {
                var basePath = path;
                result.ActualImage.Save(basePath.Replace(".png", "_actual.png"), ImageFormat.Png);
                result.DiffImage.Save(basePath.Replace(".png", "_diff.png"), ImageFormat.Png);
                result.ExpectedImage.Save(basePath.Replace(".png", "_expected.png"), ImageFormat.Png);
            }

            var matchPercent = (100 - result.Diff).ToString("0.##");
            Console.WriteLine($"Screenshot matches {path.Split('\\').Last()} at {matchPercent} percents.");

            return success;
        }

        /// <summary>
        /// Compare two Bitmap images.
        /// </summary>
        /// <param name="actualImage">Actual image.</param>
        /// <param name="expectedImage">Expected image.</param>
        /// <param name="rgbSimilarityTolerance">
        /// Tolerance for similar pixels.
        /// If you have two different pixels, but they looks almost the same then Compare will count them as equal.
        /// This is useful for cases when there is anti aliasing of fonts and images.
        /// </param>
        /// <returns>ImageComparisonResult.</returns>
        private static ImageComparisonResult CompareBitmapImages(Bitmap actualImage, Bitmap expectedImage, int rgbSimilarityTolerance = 10)
        {
            int width = Math.Min(actualImage.Width, expectedImage.Width);
            int height = Math.Min(actualImage.Height, expectedImage.Height);

            int differentPixels = 0;
            int totalPixels = width * height;

            Bitmap diffImage = new Bitmap(width, height);
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    Color actualPixel = actualImage.GetPixel(i, j);
                    Color expectedPixel = expectedImage.GetPixel(i, j);

                    if (actualPixel != expectedPixel)
                    {
                        if ((Math.Abs(actualPixel.A - expectedPixel.A) < rgbSimilarityTolerance)
                            && (Math.Abs(actualPixel.B - expectedPixel.B) < rgbSimilarityTolerance)
                            && (Math.Abs(actualPixel.G - expectedPixel.G) < rgbSimilarityTolerance)
                            && (Math.Abs(actualPixel.R - expectedPixel.R) < rgbSimilarityTolerance))
                        {
                            diffImage.SetPixel(i, j, Color.Orange);
                        }
                        else
                        {
                            differentPixels++;
                            diffImage.SetPixel(i, j, Color.Red);
                        }
                    }
                    else
                    {
                        diffImage.SetPixel(i, j, actualPixel);
                    }
                }
            }

            var diff = ((double)differentPixels) / ((double)totalPixels) * 100;
            return new ImageComparisonResult(diff: diff, actualImage: actualImage, diffImage: diffImage, expectedImage: expectedImage);
        }
    }
}
