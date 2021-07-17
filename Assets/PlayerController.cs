
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [SerializeField]
    private float playerSpeed = 3.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;


    public Transform viewPoint;

    private PlayerInput inputActions;

    public int maxHealth = 100;
    public int currentHealth;

    //Combat Codes
    public int attackDamage = 20;
    public Transform attackPoint;
    public float attacRange = 0.8f;
    public LayerMask enemyLayers;
    [SerializeField]
    private Animator animator;
    [SerializeField]
   // public ParticleSystem slash = null;
    public GameObject attackPrefab;
    public bool slashIsPlaying = false;
    [SerializeField]
    public Slider healthSlider;
    [SerializeField]
    public TMP_Text playerNameText;
    [SerializeField]
    public Text healthValueText;
    public float nextAttackAllowed;
    //Combat Codes

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data

            stream.SendNext(currentHealth);
          //  stream.SendNext(slash.isPlaying);
        }
        else
        {

            // Network player, receive data

            this.currentHealth = (int)stream.ReceiveNext();
          //  this.slashIsPlaying = (bool)stream.ReceiveNext();
        }
    }
    void Start()
    {

        if (photonView.IsMine)
        {


            //  Billboard.instance.playerNameText.text = PhotonNetwork.NickName;
            // animator = GameObject.Find("Armature").GetComponent<Animator>();

        }
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
        healthValueText.text = maxHealth.ToString() + " / " + currentHealth.ToString();

        playerNameText.text = photonView.Owner.NickName;
    }
    void Awake()
    {
        inputActions = new PlayerInput();
        controller = GetComponent<CharacterController>();



        if (photonView.IsMine)
        {
            GameObject.Find("vcam").GetComponent<CinemachineVirtualCamera>().enabled = true;
            GameObject.Find("vcam").GetComponent<CinemachineVirtualCamera>().LookAt = transform;
            GameObject.Find("vcam").GetComponent<CinemachineVirtualCamera>().Follow = transform;
        }

    }

    private void OnEnable()
    {
        inputActions.Enable();
    }
    private void OnDisable()
    {
        inputActions.Disable();
    }





    void Update()
    {

        if (photonView.IsMine)
        {
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }
            Vector2 movementInput = inputActions.playerInput.Move.ReadValue<Vector2>();
            Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);
            controller.Move(move * Time.deltaTime * playerSpeed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
                animator.SetBool("Run", true);
            }
            else animator.SetBool("Run", false);


            // Changes the height position of the player..
            if (inputActions.playerInput.Jump.triggered && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
            
            inputActions.playerInput.Test.canceled += _ => {Attack();};

            if (inputActions.playerInput.Attack.triggered && (Time.time > nextAttackAllowed))
            {

                Debug.Log("atak tuşuna basıldı");

                Attack();

                nextAttackAllowed = Time.time + 0.8f;

            }


        }
        // if (slashIsPlaying == true)
        // {
        //     slash.Play();
        // }

        healthSlider.value = currentHealth;
        healthValueText.text = maxHealth.ToString() + " / " + currentHealth.ToString();


    }



    public void Attack()
    {

        // if(photonView.IsMine){
        animator.SetTrigger("Attack");
       // slash.Play();

        Debug.Log("atack metodu çalıştı.");

        PhotonNetwork.Instantiate(attackPrefab.name, attackPoint.transform.position,transform.rotation);
        
      

        Collider[] hits = (Physics.OverlapSphere(attackPoint.position, attacRange));




        foreach (Collider hit in hits)
        {
            if (hit.gameObject.tag == "Player")
            {
                Debug.Log("Hit " + hit.gameObject.GetPhotonView().Owner.NickName);
                //  PhotonNetwork.Instantiate(playerHitImpact.name, hit.point, Quaternion.identity);
                hit.gameObject.GetPhotonView().RPC("DealDamage", RpcTarget.All, photonView.Owner.NickName, attackDamage, PhotonNetwork.LocalPlayer.ActorNumber);

                //Debug.Log("hasar verildi : " + enemy);
            }

        }

        // }

    }


    //public void Attack()
    //{

    //    Debug.Log("atack metodu çalıştı.");
    //    Collider[] hits = (Physics.OverlapSphere(attackPoint.position, attacRange, enemyLayers));

    //    foreach (Collider hit in hits)
    //    {
    //        Debug.Log("Hit " + hit.gameObject.GetPhotonView().Owner.NickName);
    //        //  PhotonNetwork.Instantiate(playerHitImpact.name, hit.point, Quaternion.identity);
    //        hit.gameObject.GetPhotonView().RPC("DealDamage", RpcTarget.All, photonView.Owner.NickName, attackDamage, PhotonNetwork.LocalPlayer.ActorNumber);

    //        //Debug.Log("hasar verildi : " + enemy);

    //    }
    //    animator.SetTrigger("Attack");

    //}
    //    Ray ray = cam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
    //    ray.origin = cam.transform.position;

    //    if (Physics.Raycast(ray, out RaycastHit hit))
    //    {
    //        //Debug.Log("We hit " + hit.collider.gameObject.name);

    //        if (hit.collider.gameObject.tag == "Player")
    //        {
    //            Debug.Log("Hit " + hit.collider.gameObject.GetPhotonView().Owner.NickName);

    //            PhotonNetwork.Instantiate(playerHitImpact.name, hit.point, Quaternion.identity);

    //            hit.collider.gameObject.GetPhotonView().RPC("DealDamage", RpcTarget.All, photonView.Owner.NickName, allGuns[selectedGun].shotDamage, PhotonNetwork.LocalPlayer.ActorNumber);
    //        }

    //    }





    //}

    [PunRPC]
    public void DealDamage(string damager, int damageAmount, int actor)
    {
        TakeDamage(damager, damageAmount, actor);
    }

    public void TakeDamage(string damager, int damageAmount, int actor)
    {
        if (photonView.IsMine)
        {
            //Debug.Log(photonView.Owner.NickName + " has been hit by " + damager);
            //   animator.SetTrigger("hit");

            currentHealth -= damageAmount;

            if (currentHealth <= 0)
            {
                currentHealth = 0;

                PlayerSpawner.instance.Die(damager);

                MatchManager.instance.UpdateStatsSend(actor, 0, 1);
            }



        }




    }

    // [PunRPC]
    // public void SyncHealth(int health)
    // {
    //     if (photonView.IsMine)
    //     {
    //         return;
    //     }
    //     else currentHealth = health;



    // }












}
