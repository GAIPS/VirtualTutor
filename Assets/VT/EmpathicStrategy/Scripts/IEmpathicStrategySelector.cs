using System.Collections.Generic;

public interface IEmpathicStrategySelector {
    Intention SelectIntention(History history, ICollection<IEmpathicStrategy> strategies, User user);
}
