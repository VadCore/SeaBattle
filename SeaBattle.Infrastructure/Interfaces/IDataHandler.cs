using SeaBattle.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.Interfaces
{
    public interface IDataHandler<TContext> where TContext : DataContext<TContext>
    {
        public void SaveContext(TContext context);

        public TContext Load();
    }
}
