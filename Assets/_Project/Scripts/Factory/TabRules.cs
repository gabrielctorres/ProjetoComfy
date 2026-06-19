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
    public float extraSideChance; // Chance de adicionar um segundo acompanhamento extra ao pedido.
    public bool varySideCategory;  // Se true,  a categoria do acompanhamento pode ser sorteada independente da categoria do principal

    public TabRules(int minOrders = 1, int maxOrders = 4, int maxQuantity = 3, float veganChance = 0.5f, float foodOnlyChance = 0.4f, float drinkOnlyChance = 0.4f, float mixedChance = 0.2f, float sidelessChance = 0.1f, float extraSideChance = 0f, bool varySideCategory = false)
    {
        this.minOrders = minOrders;
        this.maxOrders = maxOrders;
        this.maxQuantity = maxQuantity;
        this.veganChance = veganChance;
        this.foodOnlyChance = foodOnlyChance;
        this.drinkOnlyChance = drinkOnlyChance;
        this.mixedChance = mixedChance;
        this.sidelessChance = sidelessChance;
        this.extraSideChance = extraSideChance;
        this.varySideCategory = varySideCategory;
    }
}