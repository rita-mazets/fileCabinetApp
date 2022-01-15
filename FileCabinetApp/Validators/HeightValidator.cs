using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validates height.
    /// </summary>
    public class HeightValidator : IRecordValidator
    {
        private int min;
        private int max;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeightValidator
        /// "/> class.
        /// </summary>
        /// <param name="min">Parameter to initialize min parameter.</param>
        /// <param name="max">Parameter to initialize max parameter.</param>
        public HeightValidator(int min, int max)
        {
            this.min = min;
            this.max = max;
        }

        /// <summary>
        /// Validates parameter.
        /// </summary>
        /// <param name="value">Parameter to validate data.</param>
        public void ValidateParameters(FileCabinetRecord value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Height < this.min || value.Height > this.max)
            {
                throw new ArgumentException("incorrect height", nameof(value));
            }
        }
    }
}
