using SeaBattle.Domain.Interfaces;
using SeaBattle.Infrastructure.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.Interfaces
{
    public interface IDataHandler<TContext> where TContext : SerializationContext<TContext>
    {
        public void SaveContext(TContext context);

        public TContext Load();
    }
}
