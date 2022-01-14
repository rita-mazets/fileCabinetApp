using System;
using System.Globalization;
using System.Linq;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;

namespace FileCabinetApp.CommandHandlers
{
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        public InsertCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        public override object Handle(AppComandRequest appComandRequest)
        {
            if (appComandRequest is null)
            {
                throw new ArgumentNullException(nameof(appComandRequest));
            }

            if (appComandRequest.Command.Equals("insert"))
            {
                try
                {
                    var param = appComandRequest.Peremeters.Split("values", StringSplitOptions.RemoveEmptyEntries);
                    if (param[0].Trim().StartsWith('(') && param[1].Trim().StartsWith('(') && param[0].Trim().EndsWith(')') && param[1].Trim().EndsWith(')'))
                    {
                        FileCabinetRecord record = StringToRecord(param);

                        var result = this.fileCabinetService.CreateRecord(record);
                        return $"Record #{result} is insert.\n";
                    }
                    else
                    {
                        throw new ArgumentException("Error in syntax");
                    }
                }
                catch (ArgumentException e)
                {
                    return e.Message;
                }
            }
            else
            {
                return base.Handle(appComandRequest);
            }
        }

        private static FileCabinetRecord StringToRecord(string[] param)
        {
            var record = new FileCabinetRecord();
            var names = param[0].Trim(new char[] { '(', ')', ' ' }).Split(",", StringSplitOptions.RemoveEmptyEntries);
            var values = param[1].Trim(new char[] { '(', ')', ' ' }).Split("'", StringSplitOptions.RemoveEmptyEntries).ToList();

            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].Trim() == ",")
                {
                    values.RemoveAt(i);
                }
            }

            if (names.Length != values.Count)
            {
                throw new ArgumentException("Error: names length not equal values length.");
            }

            for (int i = 0; i < names.Length; i++)
            {
                var name = names[i].Trim(' ').ToLower(CultureInfo.CurrentCulture);
                switch (name)
                {
                    case "id":
                        record.Id = Convert.ToInt32(values[i], CultureInfo.CurrentCulture);
                        break;
                    case "firstname":
                        record.FirstName = values[i];
                        break;
                    case "lastname":
                        record.LastName = values[i];
                        break;
                    case "dateofbirth":
                        record.DateOfBirth = Convert.ToDateTime(values[i], CultureInfo.CurrentCulture);
                        break;
                    case "height":
                        record.Height = Convert.ToInt16(values[i], CultureInfo.CurrentCulture);
                        break;
                    case "salary":
                        record.Salary = Convert.ToDecimal(values[i], CultureInfo.CurrentCulture);
                        break;
                    case "type":
                        record.Type = values[i][0];
                        break;
                }
            }

            return record;
        }
    }
}
