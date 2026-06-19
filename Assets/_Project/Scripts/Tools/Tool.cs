using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Tool : MonoBehaviour
{
    protected virtual void OnMouseEnter()
    {
        if (DragManager.Instance.IsDragging())
        {
            Execute(DragManager.Instance.CurrentIngredient);
        }
    }

    protected virtual void Execute(Ingredient ingredient) { }
}