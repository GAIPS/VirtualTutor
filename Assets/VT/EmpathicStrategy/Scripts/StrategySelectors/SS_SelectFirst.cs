
using System.Collections.Generic;
using System.Linq;

public class SS_SelectFirst : IEmpathicStrategySelector
{
    public Intention SelectIntention(ICollection<History> history, ICollection<IEmpathicStrategy> strategies, Emotion userEmotion)
    {
        IEmpathicStrategy strategy = strategies.FirstOrDefault();
        if (strategy == null) return null;

        return strategy.GetIntentions().FirstOrDefault();
    }
}
