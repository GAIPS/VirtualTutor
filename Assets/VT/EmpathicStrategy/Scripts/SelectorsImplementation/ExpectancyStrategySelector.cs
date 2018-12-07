using System;
using System.Collections.Generic;
using System.Linq;

public class ExpectancyStrategySelector : IEmpathicStrategySelector
{
    public int NumStrategiesPerDay = 2;
    private bool _shownIntro = false;
    private int _shownStrategies;
    
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
            
            // ELSE choose interaction strategy
            if (_shownStrategies > NumStrategiesPerDay)
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
                        ++_shownStrategies;
                        SelectedStrategy = introStrategy;
                        break;
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