using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Threading;
using log4net;

namespace Framework.Utils
{
    /// <summary>
    /// Image utils.
    /// </summary>
    public static class ImageUtils
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Save screenshot.
        /// </summary>
        /// <param name="filePath">Path.</param>
        public static void SaveScreenshot(string filePath)
        {
            Image image = ScreenCapturer.CaptureDesktop();
            Bitmap bitmap = new Bitmap(image);
            bitmap.Save(filePath, ImageFormat.Png);
        }

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
        /// Compare images.
        /// </summary>
        /// <param name="screenshoter">Screenshoter function that returns Bitmap.</param>
        /// <param name="path">Path to expected image.</param>
        /// <param name="timeout">Timeout for image comparison.</param>
        /// <param name="tolerance">Allowed tolerance in percents.</param>
        /// <returns>ImageComparisonResult.</returns>
        public static bool Compare(Func<Bitmap> screenshoter, string path, int timeout = 30, double tolerance = 1)
        {
            bool success = false;
            ImageComparisonResult result = null;

            TimeSpan maxDuration = TimeSpan.FromSeconds(timeout);
            Stopwatch sw = Stopwatch.StartNew();

            while ((!success) && (sw.Elapsed < maxDuration))
            {
                Thread.Sleep(500);
                Bitmap actualImage = screenshoter();
                result = Compare(actualImage, path);
                success = result.Diff < tolerance;
            }

            // If images do not match then save actual, expected and diff relative to path folder.
            if (!success)
            {
                var basePath = path;
                result.ActualImage.Save(basePath.Replace(".png", "_actual.png"));
                result.DiffImage.Save(basePath.Replace(".png", "_diff.png"));
                result.ExpectedImage.Save(basePath.Replace(".png", "_expected.png"));
                Log.Info("Image comparison failed.");
                Log.Info("Actual, Diff and Expected images will be saved in folder of expected image.");
            }

            return success;
        }

        /// <summary>
        /// Compare images.
        /// </summary>
        /// <param name="actualImage">Bitmap object.</param>
        /// <param name="expectedImagePath">Path to expected image.</param>
        /// <returns>ImageComparisonResult object.</returns>
        public static ImageComparisonResult Compare(Bitmap actualImage, string expectedImagePath)
        {
            // Get expected image
            if (File.Exists(expectedImagePath))
            {
                var expectedImage = new Bitmap(expectedImagePath);

                // Compare images
                var result = Compare(actualImage, expectedImage);

                // Dispose images
                // actualImage.Dispose();
                // expectedImage.Dispose();

                // Return result
                return result;
            }
            else
            {
                // Save actual image as expected if expected image do not exists (and return false).
                actualImage.Save(expectedImagePath);
                actualImage.Dispose();

                // Log fatal message and throw exception.
                var message1 = "Can not find expected image at " + expectedImagePath;
                var message2 = "Actual image will be saved at " + expectedImagePath;
                Log.Fatal(message1);
                Log.Info(message2);
                throw new FileNotFoundException(message1);
            }
        }

        /// <summary>
        /// Compare two Bitmap images.
        /// </summary>
        /// <param name="actualImage">Actual image.</param>
        /// <param name="expectedImage">Expected image.</param>
        /// <param name="rgbSimilarityTolerance">
        /// Tolerance for simular pixels.
        /// If you have two different pixels, but they looks almost the same then Compare will count them as equal.
        /// This is useful for cases when there is anti aliasing of fonts and images.
        /// </param>
        /// <returns>ImageComparisonResult.</returns>
        public static ImageComparisonResult Compare(Bitmap actualImage, Bitmap expectedImage, int rgbSimilarityTolerance = 10)
        {
            int width = Math.Min(actualImage.Width, expectedImage.Width);
            int height = Math.Min(actualImage.Height, expectedImage.Height);

            int diferentPixels = 0;
            int similarPixels = 0;
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
                            similarPixels++;
                            diffImage.SetPixel(i, j, Color.Orange);
                        }
                        else
                        {
                            diferentPixels++;
                            diffImage.SetPixel(i, j, Color.Red);
                        }
                    }
                    else
                    {
                        diffImage.SetPixel(i, j, actualPixel);
                    }
                }
            }

            double diff = ((double)diferentPixels / (double)totalPixels) * 100;
            double similar = ((double)similarPixels / (double)totalPixels) * 100;

            return new ImageComparisonResult(diff: diff, actualImage: actualImage, diffImage: diffImage, expectedImage: expectedImage);
        }
    }
}
