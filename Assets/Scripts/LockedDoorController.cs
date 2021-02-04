using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorController : MonoBehaviour
{
    public AudioClip doorOpen;
    GameObject doorNeighbor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player" && coll.gameObject.GetComponent<Inventory>().useKey())
        {
            AudioSource.PlayClipAtPoint(doorOpen, Camera.main.transform.position);
            if (doorNeighbor != null)
            {
                Destroy(doorNeighbor);
            }
            Destroy(this.gameObject);
        }
        else if (coll.tag == "door")
        {
            doorNeighbor = coll.gameObject;
        }
    }
}
