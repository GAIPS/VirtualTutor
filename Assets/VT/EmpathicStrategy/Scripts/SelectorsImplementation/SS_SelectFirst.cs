
using System.Collections.Generic;
using System.Linq;

public class SS_SelectFirst : IEmpathicStrategySelector
{
    public Intention SelectIntention(History history, ICollection<IEmpathicStrategy> strategies, User user)
    {
        IEmpathicStrategy strategy = strategies.FirstOrDefault();
        if (strategy == null) return null;

        return strategy.GetIntentions().FirstOrDefault();
    }
}
