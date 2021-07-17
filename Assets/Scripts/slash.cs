using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class slash : MonoBehaviourPun
{
    [SerializeField]
    public float lifetime;
    void Start()
    {
       lifetime= Time.time + 0.1f;
    }
    private void Update() {
         if (photonView.IsMine)
        {
           if (Time.time > lifetime)
            {
                Debug.Log(" yok etçalıstı");
                PhotonNetwork.Destroy(this.gameObject);
                
                
                
            }
        }
    }
}


