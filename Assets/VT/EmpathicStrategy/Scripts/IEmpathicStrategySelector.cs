using System.Collections.Generic;

public interface IEmpathicStrategySelector {
    Intention SelectIntention(ICollection<History> history, ICollection<IEmpathicStrategy> strategies, Emotion userEmotion);
}
