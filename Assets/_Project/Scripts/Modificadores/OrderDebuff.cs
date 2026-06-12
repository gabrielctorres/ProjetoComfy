using System.Collections.Generic;

public abstract class OrderDebuff
{
    public string Name { get; protected set; }

    public virtual SpawnRules ModifySpawnRules(SpawnRules defaultRules)
    {
        return defaultRules;
    }

    public abstract Tab ApplyDebuffToTab(Tab originalTab, List<Food> foodIngredients, List<Beverage> beverageIngredients);
}