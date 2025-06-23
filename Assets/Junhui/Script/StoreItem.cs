using System;
using UnityEngine;

public class StoreItem : MonoBehaviour
{
    public NPC npc;

    public int price = 10;

    private void OnMouseDown()
    {
        //마우스 클릭시 구매
        npc.BuyItem(price);
    }
}
