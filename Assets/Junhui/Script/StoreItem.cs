using System;
using UnityEngine;

public class StoreItem : MonoBehaviour
{
    public NPC npc;

    public int price = 10;

    private void OnMouseDown()
    {
        //���콺 Ŭ���� ����
        npc.BuyItem(price);
    }
}
