using UnityEngine;

namespace eneru7i
{
    /// <summary>
    /// 플레이어를 컨트롤하는 코드
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public GameObject player;
        public Camera mainCamera;
        // 마우스 감도
        public float mouseSensitivity = 100f;
        //이동속도
        float speed = 3f;
        // 카메라 회전 제한 각도
        float xRotation = 0f;
        //Rigidbody 컴포넌트
        Rigidbody rb;
        //땅에 닿았나 여부
        bool isGround = true;
        //땅을 기는가의 여부
        bool isCrouch = false;
        // 플레이어 원래 높이
        float originalHeight;
        // 플레이어 앉았을 때 높이
        public float crouchHeight = 0.8f;
        // 플레이어의 콜라이더
        CapsuleCollider playerCollider;

        void Start()
        {
            // 플레이어와 카메라를 설정
            if (player == null)
            {
                player = this.gameObject;
            }

            if (mainCamera == null)
            {
                mainCamera = Camera.main;
            }
            // Rigidbody 컴포넌트 할당
            rb = player.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = player.AddComponent<Rigidbody>();
            }
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            // 콜라이더 할당 및 높이 설정
            playerCollider = player.GetComponent<CapsuleCollider>();
            if (playerCollider == null)
            {
                playerCollider = player.AddComponent<CapsuleCollider>();
            }
            originalHeight = playerCollider.height;
        }

        /// <summary>
        /// 플레이어 움직임 종합
        /// </summary>
        void Update()
        {
            Rotate();
            Move();
            Jump();
            Crouch();
            Interact();
        }

        #region 마우스 사용
        /// <summary>
        /// 플레이어 회전하는 함수
        /// </summary>
        void Rotate()
        {
            // 마우스 입력 받기
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // 몸체 좌우 회전
            player.transform.Rotate(Vector3.up * mouseX);

            // 카메라 상하 회전
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);
            mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

    /// <summary>
    /// 클릭시 상호작용
    /// </summary>
    void Interact()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                //5f 거리 내 상호작용이 가능한 물체가 있을 경우
                if (Physics.Raycast(ray, out RaycastHit hit, 5f))
                {
                    int hitLayer = hit.collider.gameObject.layer;
                    if (hit.collider.CompareTag("Object"))
                    {
                        Debug.Log("interacting");
                    }
                }
            }
        }
        #endregion

        #region 키보드 사용
        /// <summary>
        /// 플레이어가 움직이는 함수
        /// </summary>
        void Move()
        {
            {
                // 좌우 입력에 따라 이동
                float moveX = Input.GetAxis("Horizontal");
                float moveZ = Input.GetAxis("Vertical");
                //쉬프트 누르면 이동속도 2배
                float speedGain = Input.GetKey(KeyCode.LeftShift)? 2 : 1;
                // 앉으면 이동속도 감소
                float currentSpeed = isCrouch ? speed * 0.5f : speed;

                // 이동 벡터를 로컬 좌표계로 변환
                Vector3 move = transform.right * moveX + transform.forward * moveZ;

                // 이동 속도에 따라 변화             
                transform.position += move * currentSpeed * speedGain * Time.deltaTime;
            }
        }

        /// <summary>
        /// 플레이어 점프하는 스크립트
        /// </summary>
        void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && isGround && !isCrouch)
            {
                // 플레이어에 위쪽 방향으로 힘을 가해 점프
                rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
                isGround = false;
            }
        }

        /// <summary>
        /// 땅을 기는 스크립트
        /// </summary>
        void Crouch()
        {
            //땅을 안길경우
            if (!isCrouch)              
            {
                if (Input.GetKeyDown(KeyCode.C) && isGround)
                {
                    // 앉는 상태로 변경
                    playerCollider.height = crouchHeight;
                    isCrouch = true;
                }              
            }
            //땅을 길 경우
            else
            {
                if ((Input.GetKeyDown(KeyCode.C)|| Input.GetKeyDown(KeyCode.Space)) && isGround)
                {
                    // 앉지 않는 상태로 변경
                    playerCollider.height = originalHeight;
                    isCrouch = false;
                }
            }
        }
        #endregion

        /// <summary>
        /// 충돌 관련 스크립트들
        /// </summary>
        /// <param name="collision"></param>
        void OnCollisionEnter(Collision collision)
        {
            // 충돌한 객체가 땅이면 isGround를 true로 설정
            if (collision.gameObject.CompareTag("Untagged"))
            {
                isGround = true;
            }
        }
    }
}
