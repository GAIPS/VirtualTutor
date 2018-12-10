using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;

public class ExpectancyStrategySelector : IEmpathicStrategySelector
{
    public int NumStrategiesPerDay = 2;
    private bool _shownIntro = false;
    private int _shownStrategies;

    public int ShownStrategies
    {
        get { return _shownStrategies; }
        set
        {
            _shownStrategies = value;

            var state = PersistentDataStorage.Instance.GetState();
            state["DailyTask"].AsObject["ShownStrategies"].AsObject[DateTime.Now.ToShortDateString()] =
                Convert.ToInt32(_shownStrategies);

            PersistentDataStorage.Instance.SaveState();
        }
    }

    public HashSet<string> PlayedStrategies = new HashSet<string>();

    private IEmpathicStrategy _selectedStrategy;

    private IEmpathicStrategy SelectedStrategy
    {
        get { return _selectedStrategy; }
        set
        {
            _selectedStrategy = value;
            _selectIntentIndex = 0;
        }
    }

    private int _selectIntentIndex;

    public ExpectancyStrategySelector()
    {
        // TODO remove comments for release
//        var state = PersistentDataStorage.Instance.GetState();
//        _shownStrategies = state["DailyTask"].AsObject["ShownStrategies"].AsObject[DateTime.Now.ToShortDateString()]
//            .AsInt;
    }

    public Intention SelectIntention(History history, ICollection<IEmpathicStrategy> strategies, User user)
    {
        if (SelectedStrategy != null)
        {
            return GetNextIntention();
        }

        if (!_shownIntro)
        {
            SelectedStrategy = GetStrategyByName(strategies, "welcome");
            _shownIntro = true;
        }
        else
        {
            // IF emotivector woke then choose strategy
            // TODO Deal with emotivector
            var expectancy = history.Get<Emotivector.Expectancy>("Expectancy");
            if (expectancy != null)
            {
                string name = expectancy.Owner.Name.ToLower(),
                    valence = expectancy.valence.ToString().ToLower(),
                    change;
                switch (expectancy.change)
                {
                    case Emotivector.Expectancy.Change.BetterThanExpected:
                        change = "greater";
                        break;
                    case Emotivector.Expectancy.Change.WorseThanExpected:
                        change = "lesser";
                        break;
                    case Emotivector.Expectancy.Change.AsExpected:
                    // Waterfall
                    default:
                        change = "as-expected";
                        break;
                }

                SelectedStrategy = GetStrategyByName(strategies,
                    name + "-" + valence + "-" + change);
                history.Set("Expectancy", null);
            }
            else
            {
                // ELSE choose interaction strategy
                if (_shownStrategies >= NumStrategiesPerDay)
                {
                    SelectedStrategy = GetStrategyByName(strategies, "exit");
                }
                else
                {
                    List<IEmpathicStrategy> introStrategies = GetStrategiesContainsName(strategies, "intro");
                    // Randomize List
                    introStrategies = introStrategies.OrderBy(a => Guid.NewGuid()).ToList();
                    foreach (var introStrategy in introStrategies)
                    {
                        if (introStrategy.IsValid() && !PlayedStrategies.Contains(introStrategy.Name))
                        {
                            PlayedStrategies.Add(introStrategy.Name);
                            ++ShownStrategies;
                            SelectedStrategy = introStrategy;
                            break;
                        }
                    }
                }
            }
        }

        return GetNextIntention();
    }

    private IEmpathicStrategy GetStrategyByName(ICollection<IEmpathicStrategy> strategies, string name)
    {
        foreach (var empathicStrategy in strategies)
        {
            if (empathicStrategy.Name.ToLower().Equals(name.ToLower()))
            {
                return empathicStrategy;
            }
        }

        return null;
    }

    private List<IEmpathicStrategy> GetStrategiesContainsName(ICollection<IEmpathicStrategy> strategies, string name)
    {
        List<IEmpathicStrategy> strats = new List<IEmpathicStrategy>();
        foreach (var empathicStrategy in strategies)
        {
            if (empathicStrategy.Name.ToLower().Contains(name.ToLower()))
            {
                strats.Add(empathicStrategy);
            }
        }

        return strats;
    }

    private Intention GetNextIntention()
    {
        if (SelectedStrategy == null)
        {
            return null;
        }

        var intentions = SelectedStrategy.GetIntentions();
        if (_selectIntentIndex >= intentions.Count)
        {
            SelectedStrategy = null;
            return null;
        }

        var intention = intentions.ElementAt(_selectIntentIndex);
        ++_selectIntentIndex;
        if (_selectIntentIndex >= intentions.Count)
        {
            SelectedStrategy = null;
        }

        return intention;
    }
}