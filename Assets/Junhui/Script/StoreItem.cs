using System;
using UnityEngine;

public class StoreItem : MonoBehaviour
{
    public NPC npc;
    

    private void OnMouseDown()
    {
        //���콺 Ŭ���� ����
        npc.BuyItem();
    }
}
