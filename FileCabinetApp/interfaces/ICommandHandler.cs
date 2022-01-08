using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public interface ICommandHandler
    {
        public ICommandHandler SetNext(ICommandHandler commandHandler);

        public object Handle(AppComandRequest appComandRequest);
    }
}
