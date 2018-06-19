
using System.Collections.Generic;
using System.Linq;

public class SS_SelectFirst : IEmpathicStrategySelector
{
    public Intention SelectIntention(History history, ICollection<IEmpathicStrategy> strategies, User user)
    {
        foreach (var empathicStrategy in strategies)
        {
            if (empathicStrategy.IsValid())
            {
                return empathicStrategy.GetIntentions().FirstOrDefault();
            }
        }
        return null;
    }
}
