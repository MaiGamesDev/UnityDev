using UnityEngine;

public class StoreItem : MonoBehaviour
{
    public NPC npc;
    private bool isMouseEntered = false;

    public int price = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isMouseEntered)
            npc.BuyItem(price);
    }

    private void OnMouseEnter()
    {
        isMouseEntered = true;
    }
    void OnMouseExit()
    {
        isMouseEntered = false;
    }
}
