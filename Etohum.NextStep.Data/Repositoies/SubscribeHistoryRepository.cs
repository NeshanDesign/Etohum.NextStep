using Etohum.NextStep.Data.Model;

namespace Etohum.NextStep.Data.Repositoies
{
    public class SubscribeHistoryRepository : RepositoryBase<int, SubscriptionHistory>
    {
        public override SubscriptionHistory GetById(int id)
        {
            return this.First(t => t.Id == id);
        }
    }
}