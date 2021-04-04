using System.IO;
using Automator.Shared.Utils;
using Microsoft.Extensions.Configuration;

namespace Automator.Shared.VisualTesting
{
    /// <summary>
    /// Visual testing settings.
    /// </summary>
    public class VisualTestingSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisualTestingSettings"/> class.
        /// </summary>
        /// <param name="visualTestingSection">Visual testing section.</param>
        public VisualTestingSettings(IConfigurationSection visualTestingSection)
        {
            var imagePath = visualTestingSection.GetValue<string>("ImagePath");
            var relativeImagePath = imagePath
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);
            ImagePath = Path.Combine(FileSystem.ProjectRoot, relativeImagePath);
            Tolerance = visualTestingSection.GetValue<double>("Tolerance");
            ImageVerificationType = visualTestingSection.GetValue<ImageComparisonType>("VerificationType");
        }

        /// <summary>
        /// Gets expected images path.
        /// </summary>
        public string ImagePath { get; private set; }

        /// <summary>
        /// Gets ImageVerificationType.
        /// </summary>
        public double Tolerance { get; private set; }

        /// <summary>
        /// Gets ImageVerificationType.
        /// </summary>
        public ImageComparisonType ImageVerificationType { get; private set; }
    }
}
