using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers.ServiceCommandHandlers
{
    public class ServiceCommandHandlerBase:CommandHandlerBase
    {
        protected IFileCabinetService fileCabinetService;

        public ServiceCommandHandlerBase(IFileCabinetService fileCabinetService)
        {
            this.fileCabinetService = fileCabinetService;
        }
    }
}
