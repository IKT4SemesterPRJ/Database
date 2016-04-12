using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogInClass;
using Pristjek220Data;

namespace LogIn
{
    public class LogInClass : ILogIn
    {
        private readonly IUnitOfWork _unit;

        public LogInClass(UnitOfWork unit)
        {
            _unit = unit;
        }



    }
}
