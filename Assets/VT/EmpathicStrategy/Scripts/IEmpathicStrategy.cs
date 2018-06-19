using System.Collections.Generic;

public interface IEmpathicStrategy
{
    string Name { get; set; }
    ICollection<Intention> GetIntentions();
    bool IsValid();
}