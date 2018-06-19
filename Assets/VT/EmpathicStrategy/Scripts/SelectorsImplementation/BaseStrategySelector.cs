using System.Collections.Generic;
using System.Linq;

public class BaseStrategySelector : IEmpathicStrategySelector
{
    public HashSet<string> PlayedStrategies = new HashSet<string>();
    
    public Intention SelectIntention(History history, ICollection<IEmpathicStrategy> strategies, User user)
    {
        foreach (var empathicStrategy in strategies)
        {
            if (empathicStrategy.IsValid() && !PlayedStrategies.Contains(empathicStrategy.Name))
            {
                PlayedStrategies.Add(empathicStrategy.Name);
                return empathicStrategy.GetIntentions().FirstOrDefault();
            }
        }

        return new Intention("exit");
    }
}