using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FileCabinetApp.CommandHandlers.ServiceCommandHandlers;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Works with Error method.
    /// </summary>
    public class ErrorCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">Parameter to initialize fileCabinetService.</param>
        public ErrorCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <summary>
        /// Perform action.
        /// </summary>
        /// <param name="appComandRequest">Parameter to set nextHandler.</param>
        /// <returns>If nextHandler is null return null, else Handle().</returns>
        public override object Handle(AppComandRequest appComandRequest)
        {
            if (appComandRequest is null)
            {
                throw new ArgumentNullException(nameof(appComandRequest));
            }

            string command = appComandRequest.Command;

            Console.WriteLine($"\'{command}\' is not a git command. See \'help\'.\nThe most similar command is");
            this.CheckFunc(command, "help");
            this.CheckFunc(command, "exit");
            this.CheckFunc(command, "create");
            this.CheckFunc(command, "delete");
            this.CheckFunc(command, "export");
            this.CheckFunc(command, "select");
            this.CheckFunc(command, "import");
            this.CheckFunc(command, "insert");
            this.CheckFunc(command, "purge");
            this.CheckFunc(command, "stat");
            this.CheckFunc(command, "update");

            return string.Empty;
        }

        private void CheckFunc(string command, string answer)
        {
            var result = string.Empty;
            foreach (var word in this.Permute(answer))
            {
                if (command == word)
                {
                    result = answer;
                    break;
                }
            }

            foreach (var word in Substrings(answer))
            {
                if (command.Contains(word))
                {
                    result = answer;
                    break;
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                return;
            }

            Console.WriteLine($"    {result}");
        }

        private IEnumerable<string> Permute(string word)
        {
            if (word.Length > 1)
            {
                char character = word[0];
                foreach (string subPermute in this.Permute(word.Substring(1)))
                {
                    for (int index = 0; index <= subPermute.Length; index++)
                    {
                        string pre = subPermute.Substring(0, index);
                        string post = subPermute.Substring(index);

                        if (post.Contains(character))
                        {
                            continue;
                        }

                        yield return pre + character + post;
                    }
                }
            }
            else
            {
                yield return word;
            }
        }

        private static IEnumerable<string> Substrings(string word)
        {
            for (int l = word.Length; l > 0; --l)
            {
                for (int i = 0; i <= word.Length - l; ++i)
                {
                    yield return word[i.. (i + l)];
                }
            }
        }
    }
}
