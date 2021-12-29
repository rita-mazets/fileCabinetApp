using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class AppComandRequest
    {
        public AppComandRequest(string command, string parameters)
        {
            this.Command = command;
            this.Peremeters = parameters;
        }

        public string Command { get; }

        public string Peremeters { get; }
    }
}
