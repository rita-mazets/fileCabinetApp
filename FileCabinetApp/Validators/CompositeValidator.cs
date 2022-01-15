using System.Collections.Generic;
using System.Linq;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Validates custom parameter.
    /// </summary>
    public class CompositeValidator : IRecordValidator
    {
        private List<IRecordValidator> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="validators">Parameter to initialize valitors.</param>
        public CompositeValidator(IEnumerable<IRecordValidator> validators)
        {
            this.validators = validators.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeValidator"/> class.
        /// </summary>
        /// <param name="parameters">Parameter to validate data.</param>
        public void ValidateParameters(FileCabinetRecord parameters)
        {
            foreach (var validator in this.validators)
            {
                validator.ValidateParameters(parameters);
            }
        }
    }
}
