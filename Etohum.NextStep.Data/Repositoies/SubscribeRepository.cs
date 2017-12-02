using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Etohum.NextStep.Data.Model;

namespace Etohum.NextStep.Data.Repositoies
{
    public class SubscribeRepository:RepositoryBase<int, Subscriber>
    {
        public override Subscriber GetById(int id)
        {
            return this.First(t => t.Id == id);
        }
    }
}
