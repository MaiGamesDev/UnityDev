using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string idle;
    public string thankYou;
    public TextMeshPro lineBox;

    public 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setLine(idle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void setLine(string line)
    {
        Debug.Log(line);
        lineBox.text = line;
    }
}
