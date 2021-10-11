using SeaBattle.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.Interfaces
{
    public interface IDataHandler
    {
        public void SaveContext(IUnitOfWork unitOfWork);

        public IUnitOfWork Load();
    }
}
