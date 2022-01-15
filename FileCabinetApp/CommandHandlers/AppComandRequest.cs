using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for initialize command and action.
    /// </summary>
    public class AppComandRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppComandRequest"/> class.
        /// </summary>
        /// <param name="command">Parameter to initialize command.</param>
        /// <param name="parameters">Parameter to initialize parameters.</param>
        public AppComandRequest(string command, string parameters)
        {
            this.Command = command;
            this.Peremeters = parameters;
        }

        /// <summary>
        /// Gets initializes a new instance of the <see cref="AppComandRequest"/> class.
        /// </summary>
        /// <value>
        /// Initializes a new instance of the <see cref="AppComandRequest"/> class.
        /// </value>
        public string Command { get; }

        /// <summary>
        /// Gets initializes a new instance of the <see cref="AppComandRequest"/> class.
        /// </summary>
        /// <value>
        /// Initializes a new instance of the <see cref="AppComandRequest"/> class.
        /// </value>
        public string Peremeters { get; }
    }
}
