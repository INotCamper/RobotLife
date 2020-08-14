using Photon.Pun;
using System.Collections;
using UnityEngine;


namespace Com.MyComapany.MyGame
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        #region Private Fields

        [SerializeField]
        private float directionDampTime = 0.25f;
        [Tooltip("The source of the footsteps")]
        [SerializeField]
        private AudioSource audSor;

        #endregion

        #region MonoBehaviour Callbacks

        private Animator animator;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }
            audSor = GetComponent<AudioSource>();
            if (!audSor)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Audio Source Component", this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!photonView.IsMine && PhotonNetwork.IsConnected)
            {
                return;
            }
            if (!animator)
            {
                return;
            }
            //pulo
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            //apenas pode pular se estiver correndo
            if (stateInfo.IsName("Base Layer.Run"))
            {
                //usando o parametro de trigger
                if (Input.GetButtonDown("Fire2"))
                {
                    animator.SetTrigger("Jump");
                }
            }
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            if (v < 0)
            {
                v = 0;
            }
            animator.SetFloat("Speed", h * h + v * v);
            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
            if ((h * h + v * v) > 0 && !audSor.isPlaying)
            {
                audSor.pitch = Random.Range(.75f, 1.25f);
                audSor.Play();
            }
        }

        #endregion
    }
}
