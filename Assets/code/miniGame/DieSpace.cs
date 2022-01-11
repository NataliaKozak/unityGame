using UnityEngine;

public class DieSpace : MonoBehaviour
{
    public GameObject rest;
    private void OnTriggerEnter2D(Collider2D other)
    {
        print("in triger");
        if(other.tag == "Player")
        {
            other.transform.position = rest.transform.position;
        }
    }
}
