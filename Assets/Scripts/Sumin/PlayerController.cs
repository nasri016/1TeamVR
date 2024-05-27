using UnityEngine;
using UnityEngine.InputSystem;

namespace eneru7i
{
    /// <summary>
    /// 플레이어 컨트롤러
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        //플레이어
        public GameObject player;
        //카메라
        public Camera mainCamera;
        //마우스 감도
        public float mouseSensitivity = 100f;
        //이동속도
        float speed = 3f;
        //카메라 상하 각도
        float xRotation = 0f;
        //리지드바디
        Rigidbody rb;
        //애니메이터
        Animator animator;
        //땅에 닿는지 여부
        bool isGround = true;
        //달리는지 여부
        bool isRunning = false;
        //앉아가는지 여부
        bool isCrouch = false;
        //원래 키
        public float originalHeight;
        //앉은 키
        public float crouchHeight;
        //컬라이더
        CapsuleCollider playerCollider;
        //인풋시스템 
        private Vector2 moving;
        private Vector2 look;
        private bool jump;
        private bool interact;

        void Start()
        {
            if (player == null)
            {
                player = this.gameObject;
            }

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }

            rb = player.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = player.AddComponent<Rigidbody>();
            }
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            playerCollider = player.GetComponent<CapsuleCollider>();
            if (playerCollider == null)
            {
                playerCollider = player.AddComponent<CapsuleCollider>();
            }
            originalHeight = playerCollider.height;

            animator = player.GetComponent<Animator>();
        }

        /// <summary>
        /// 플레이어 움직임 모음
        /// </summary>
        void Update()
        {
            Look();
            Interact();
            Move();
            if (jump)
            {
                Jump();
            }
        }

        #region 플레이어 조작
        /// <summary>
        /// 마우스로 화면 조작
        /// </summary>
        public void Look()
        {
            // 마우스 입력 값을 받아옵니다.
            float mouseX = look.x * mouseSensitivity * Time.deltaTime;
            float mouseY = look.y * mouseSensitivity * Time.deltaTime;

            // 플레이어를 수평 방향으로 회전합니다.
            player.transform.Rotate(Vector3.up * mouseX);

            // 카메라를 수직 방향으로 회전합니다.
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);
            mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        /// <summary>
        /// 클릭시 상호작용
        /// </summary>
        public void Interact()
        {
            if (interact)
            {
                Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out RaycastHit hit, 5f))
                {
                    if (hit.collider.CompareTag("Interactable"))
                    {
                        Debug.Log("interacting");
                    }
                }
            }
        }

        /// <summary>
        /// 플레이어 이동
        /// </summary>
        public void Move()
        {
            float moveX = moving.x;
            float moveZ = moving.y;
            //달리기 여부에 따라 이동속도 증가
            float speedGain = isRunning ? 2 : 1;
            //숙이기 여부에 따라 이동속도 감소
            float currentSpeed = isCrouch ? speed * 0.5f : speed;
            //이동속도 계산
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            transform.position += move * currentSpeed * speedGain * Time.deltaTime;
            //이동 애니메이션 사용
            animator.SetFloat("MoveX", moveX * speedGain);
            animator.SetFloat("MoveY", moveZ * speedGain);
        }

        /// <summary>
        /// 점프
        /// </summary>
        public void Jump()
        {
            if (isGround && !isCrouch)
            {
                rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
                isGround = false;
            }
            jump = false;
        }

        /// <summary>
        /// 달리기
        /// </summary>
        public void Running()
        {
            if (!isRunning && isGround)
            {
                isRunning = true;
            }
            else if (isGround)
            {
                isRunning = false;
            }
        }

        /// <summary>
        /// 숙이기
        /// </summary>
        public void Crouch()
        {
            ///숙이기
            if (!isCrouch && isGround)
            {
                animator.SetBool("Crouch",true);
                //숙일 경우의 키
                playerCollider.height = crouchHeight;
                //숙일경우 센터
                playerCollider.center = new Vector3(playerCollider.center.x, crouchHeight / 2f, playerCollider.center.z);
                isCrouch = true;
            }
            ///일어서기
            else if (isGround)
            {
                animator.SetBool("Crouch", false);
                //일어설 경우의 키
                playerCollider.height = originalHeight;
                //일어설 경우 센터
                playerCollider.center = new Vector3(playerCollider.center.x, originalHeight / 2f, playerCollider.center.z);
                isCrouch = false;
            }
        }
        #endregion

        // Unity Events 메서드
        #region unity event

        /// <summary>
        /// 이동 연결
        /// </summary>
        /// <param name="context"></param>
        public void OnMove(InputAction.CallbackContext context)
        {
            moving = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// 시선전환 연결
        /// </summary>
        /// <param name="context"></param>
        public void OnLook(InputAction.CallbackContext context)
        {
            look = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// 점프 연결
        /// </summary>
        /// <param name="context"></param>
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed && !isCrouch)
            {
                jump = true;
            }
            else if (context.performed && isCrouch)
            {
                Crouch();
            }
        }

        /// <summary>
        /// 달리기 연결
        /// </summary>
        /// <param name="context"></param>
        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                // 달리기 상태를 설정합니다.
                Running();
            }
            else if (context.canceled)
            {
                // 달리기 상태를 해제합니다.
                isRunning = false;
            }
        }

        /// <summary>
        /// 기어가기 연결
        /// </summary>
        /// <param name="context"></param>
        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Crouch();
            }
        }

        /// <summary>
        /// 상호작용 연결
        /// </summary>
        /// <param name="context"></param>
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                interact = true;
            }
            else if (context.canceled)
            {
                interact = false;
            }
        }
        #endregion

        /// <summary>
        /// 물체 충돌 이벤트
        /// </summary>
        /// <param name="collision"></param>
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Untagged"))
            {
                isGround = true;              
            }
        }
    }
}
