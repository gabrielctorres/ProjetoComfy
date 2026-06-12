public class SpawnRules
{
    public int minOrders;
    public int maxOrders;
    public int maxQuantity;


    public float veganChance;
    public float foodOnlyChance;
    public float drinkOnlyChance;
    public float mixedChance;

    public SpawnRules(int minOrders = 1, int maxOrders = 4, int maxQuantity = 3, float veganChance = 0.5f, float foodOnlyChance = 0.4f, float drinkOnlyChance = 0.4f, float mixedChance = 0.2f)
    {
        this.minOrders = minOrders;
        this.maxOrders = maxOrders;
        this.maxQuantity = maxQuantity;
        this.veganChance = veganChance;
        this.foodOnlyChance = foodOnlyChance;
        this.drinkOnlyChance = drinkOnlyChance;
        this.mixedChance = mixedChance;
    }
}