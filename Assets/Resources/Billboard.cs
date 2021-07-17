
using UnityEngine;


public class Billboard : MonoBehaviour
{

    public Transform cam;
    
    

    private void Awake()
    {
        cam = Camera.main.transform;
    }
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
