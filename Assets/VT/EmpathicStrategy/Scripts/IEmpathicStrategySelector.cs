using System.Collections.Generic;

public interface IEmpathicStrategySelector {
    Intention SelectIntention(List<History> history, List<IEmpathicStrategy> strategies, Emotion emotion);
}
