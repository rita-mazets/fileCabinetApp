using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    class Program
    {
        private static string outputType;
        private static string output;
        private static string recordAmount;
        private static string startId;
        private static Records records = new Records();

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            (outputType, output, recordAmount, startId) = ParseArgs(args);
            Generator();
            Export();
        }

        private static (string, string, string, string) ParseArgs(string[] args)
        {
            string nameOutputTypeParam = "csv";
            string nameOutputParam = "D:\\records.csv";
            string nameRecordAmountParam = "100";
            string nameStartIdParam = "1";

            if (args is not null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    nameOutputTypeParam = CheckParam(args, i, "--output-type=", "-t", nameOutputTypeParam);
                    nameOutputParam = CheckParam(args, i, "--output=", "-o", nameOutputParam);
                    nameRecordAmountParam = CheckParam(args, i, "--records-amount=", "-a", nameRecordAmountParam);
                    nameStartIdParam = CheckParam(args, i, "--start-id=", "-i", nameStartIdParam);
                }
            }

            return (nameOutputTypeParam, nameOutputParam, nameRecordAmountParam, nameStartIdParam);
        }

        private static string CheckParam(string[] args, int i, string fullNameParam, string shortNameParam, string valueParam)
        {
            bool isShort = false;

            if (args[i].ToLower(CultureInfo.CurrentCulture).Contains(fullNameParam))
            {
                valueParam = args[i].Split("=")[1];
            }

            if (args[i].ToLower(CultureInfo.CurrentCulture).Equals(shortNameParam))
            {
                isShort = true;
            }

            if (isShort)
            {
                if (i + 1 < args.Length)
                {
                    valueParam = args[i + 1];
                    isShort = false;
                }
            }

            return valueParam;
        }

        private static void Generator()
        {
            int id, amount;
            int.TryParse(startId, out id);
            int.TryParse(recordAmount, out amount);

            for (int i = id; i < amount + id; i++)
            {
                var record = new FileCabinetRecord();
                record.Id = i;
                record.FirstName = RandomString(RandomInt(2, 61));
                record.LastName = RandomString(RandomInt(2, 61));
                record.DateOfBirth = RandomDate();
                record.Height = RandomShort(250);
                record.Salary = RandomDecimal();
                record.Type = RandomChar();

                records.RecordList.Add(record);
            }
        }

        private static int RandomInt(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private static short RandomShort(short max)
        {
            Random random = new Random();
            return (short)random.Next(max);
        }

        private static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = RandomChar();
                builder.Append(ch);
            }

            return builder.ToString();
        }

        private static DateTime RandomDate()
        {
            Random random = new Random();
            int year = random.Next(1950, DateTime.Now.Year + 1);
            int month = random.Next(1, 13);
            int day;
            if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                day = random.Next(1, 32);
            }
            else if (month == 2)
            {
                day = random.Next(1, 29);
            }
            else
            {
                day = random.Next(1, 31);
            }

            return new DateTime(year, month, day);
        }

        private static char RandomChar()
        {
            Random random = new Random();
            return Convert.ToChar(Convert.ToInt32(Math.Floor((26 * random.NextDouble()) + 65)));
        }

        private static decimal RandomDecimal()
        {
            Random random = new Random();
            byte scale = (byte)random.Next(29);
            return new decimal(RandomInt(int.MinValue, int.MaxValue), RandomInt(int.MinValue, int.MaxValue), RandomInt(int.MinValue, int.MaxValue), false, scale);
        }

        private static void Export()
        {
            if (outputType.ToLower().Equals("csv"))
            {
                ExportCsv();
            }

            if (outputType.ToLower().Equals("xml"))
            {
                ExportXml();
            }
        }

        private static void ExportCsv()
        {
            try
            {
                using (StreamWriter sw = new (output, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine("Id,FirstName,LastName,DateOfBirth,Height,Salary,Type");

                    foreach (var record in records.RecordList)
                    {
                        sw.WriteLine(ToCsvString(record));
                    }

                    Console.WriteLine($"{recordAmount} records are exported to file {output}.");
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Export failed: can't open file {output}.");
            }
        }

        private static string ToCsvString(FileCabinetRecord record)
        {
            return record.Id + "," + record.FirstName + "," + record.LastName + "," + record.DateOfBirth + "," + record.Height + "," + record.Salary + "," + record.Type;
        }

        private static void ExportXml()
        {
            try
            {
                XmlSerializer formatter = new XmlSerializer(typeof(Records));
                using (StreamWriter fs = new StreamWriter(output))
                {
                    formatter.Serialize(fs, records);
                    Console.WriteLine($"{recordAmount} records are exported to file {output}.");
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Export failed: can't open file {output}.");
            }
        }
    }

    [XmlRoot("records")]
    public class Records
    {
        [XmlArray("record")]
        public List<FileCabinetRecord> RecordList = new ();

    }
}
