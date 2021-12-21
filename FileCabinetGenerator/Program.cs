using System;
using System.Globalization;

namespace FileCabinetGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            string outputType, output, recordAmounr, startId;
            (outputType, output, recordAmounr, startId) = ParseArgs(args);
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
    }
}
