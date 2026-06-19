public class TabRules
{
    public int minOrders;
    public int maxOrders;
    public int maxQuantity;

    public float veganChance;
    public float foodOnlyChance;
    public float drinkOnlyChance;
    public float mixedChance;

    public float sidelessChance; // Chance de adicionar um segundo acompanhamento extra ao pedido.  (tive que comentar pq essa gambiarra ficou cheio de coisa)
    public bool varySideCategory;  // Se true,  a categoria do acompanhamento pode ser sorteada independente da categoria do principal

    public TabRules(int minOrders = 1, int maxOrders = 1, int maxQuantity = 1, float veganChance = 0.5f, float foodOnlyChance = 0.4f, float drinkOnlyChance = 0.4f, float mixedChance = 0.2f, float sidelessChance = 0.1f, bool varySideCategory = false)
    {
        this.minOrders = minOrders;
        this.maxOrders = maxOrders;
        this.maxQuantity = maxQuantity;
        this.veganChance = veganChance;
        this.foodOnlyChance = foodOnlyChance;
        this.drinkOnlyChance = drinkOnlyChance;
        this.mixedChance = mixedChance;
        this.sidelessChance = sidelessChance;
        this.varySideCategory = varySideCategory;
    }
}