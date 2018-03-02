﻿
using System.Collections.Generic;

public class BasicStrategy : IEmpathicStrategy
{
    public ICollection<Intention> Intentions { get; set; }

    public BasicStrategy()
    {
        Intentions = new List<Intention>();
    }

    public ICollection<Intention> GetIntentions()
    {
        return Intentions;
    }
}
