using System.Collections;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Peak : MonoBehaviour
{
    private float posX = 0;
    private bool isFall = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        Fall();
        if (transform.position.y <= -4)
            StartCoroutine(ResetPos());
    }

    void Fall()
    {
        if (!isFall)
            return;
        Vector2 pos = new Vector2(posX, -4);
        transform.position = Vector2.MoveTowards(transform.position, pos, Time.deltaTime * 10);
    }
    IEnumerator ResetPos()
    {
        isFall = false;
        transform.position = new Vector2(posX, 7);
        yield return new WaitForSeconds(4f);
        isFall = true;
    }
}
