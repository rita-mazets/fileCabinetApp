using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators
{
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators;

        public IRecordValidator Create()
        {
            return new CompositeValidator(this.validators);
        }

        public ValidatorBuilder ValidateFirstName(int min, int max)
        {
            this.validators.Add(new FirstNameValidator(min, max));
            return this;
        }

        public ValidatorBuilder ValidateLastName(int min, int max)
        {
            this.validators.Add(new LastNameValidator(min, max));
            return this;
        }

        public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
            return this;
        }

        public ValidatorBuilder ValidateHeight(int min, int max)
        {
            this.validators.Add(new HeightValidator(min, max));
            return this;
        }

        public ValidatorBuilder ValidateSalary(int min)
        {
            this.validators.Add(new SalaryValidator(min));
            return this;
        }

        public ValidatorBuilder ValidateType()
        {
            this.validators.Add(new TypeValidator());
            return this;
        }

        public IRecordValidator CreateDefault(ValidateParam param)
        {
            return new DefaultValidator(param);
        }

        public IRecordValidator CreateCustom(ValidateParam param)
        {
            return new CustomValidator(param);
        }
    }
}
