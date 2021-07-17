using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ranged : MonoBehaviourPun
{
    // Start is called before the first frame update
    public float speed = 6f;
    public int attackDamage = 20;
    public float lifetime;


    void Start()
    {
        lifetime = Time.time + 0.7f;
    }
    void Update()
    {
        

        transform.Translate(0, 0, speed * Time.deltaTime, gameObject.transform);
        if (photonView.IsMine)
        {
            if (Time.time > lifetime)
            {
                Debug.Log(" yok etçalıstı");
                PhotonNetwork.Destroy(this.gameObject);
            }
            Collider[] hits = (Physics.OverlapSphere(gameObject.transform.position, 0.25f));
            if (hits != null)
            {
                foreach (var hit in hits)
                {
                    if (hit.gameObject.tag == "Player")
                    {
                        Debug.Log("Hit " + hit.gameObject.GetPhotonView().Owner.NickName);
                        //  PhotonNetwork.Instantiate(playerHitImpact.name, hit.point, Quaternion.identity);
                        hit.gameObject.GetPhotonView().RPC("DealDamage", RpcTarget.All, photonView.Owner.NickName, attackDamage, PhotonNetwork.LocalPlayer.ActorNumber);
                        PhotonNetwork.Destroy(this.gameObject);
                        //Debug.Log("hasar verildi : " + enemy);
                    }
                }

            }
        }





    }



}
