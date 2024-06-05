using UnityEditor.XR.LegacyInputHelpers;
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
        public float speed = 2f;
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
        private bool left;
        private bool right;
        // 손 위치 트랜스폼
        public Transform leftHand;
        public Transform rightHand;
        // 손에 들고 있는 오브젝트
        private GameObject leftHandObject;
        private GameObject rightHandObject;

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
            Move();
            if (jump)
            {
                Jump();
            }
            HandleInteractions();
        }

        #region 플레이어 조작
        /// <summary>
        /// 마우스로 화면 조작
        /// </summary>
        public void Look()
        {
            // 마우스 입력 값을 받아옵니다.
            Vector2 mouseDelta = look * mouseSensitivity * Time.deltaTime;

            // 수평 회전
            player.transform.Rotate(Vector3.up * mouseDelta.x);

            // 수직 회전
            xRotation -= mouseDelta.y;
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);
            mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }

        /// <summary>
        /// 상호작용 처리
        /// </summary>
        private void HandleInteractions()
        {
            if (left)
            {
                if (leftHandObject == null)
                {
                    TryPickupObject(ref leftHandObject, leftHand);
                }
                else
                {
                    leftHandObject.transform.position = leftHand.position;
                }
            }
            else if (leftHandObject != null)
            {
                DropObject(ref leftHandObject);
            }

            if (right)
            {
                if (rightHandObject == null)
                {
                    TryPickupObject(ref rightHandObject, rightHand);
                }
                else
                {
                    rightHandObject.transform.position = rightHand.position;
                }
            }
            else if (rightHandObject != null)
            {
                DropObject(ref rightHandObject);
            }
        }

        /// <summary>
        /// 오브젝트를 손에 들기 시도
        /// </summary>
        /// <param name="handObject">손에 들 오브젝트</param>
        /// <param name="hand">손 위치</param>
        private void TryPickupObject(ref GameObject handObject, Transform hand)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 2.5f))
            {
                if (hit.collider.CompareTag("Interactable"))
                {
                    handObject = hit.collider.gameObject;
                    handObject.transform.SetParent(hand);
                    handObject.transform.position = hand.position;
                    handObject.transform.localPosition = Vector3.zero;
                    Rigidbody hitRb = handObject.GetComponent<Rigidbody>();
                    if (hitRb != null)
                    {
                        hitRb.isKinematic = true;
                    }
                }
            }
        }

        /// <summary>
        /// 오브젝트를 손에서 놓기
        /// </summary>
        /// <param name="handObject">손에서 놓을 오브젝트</param>
        private void DropObject(ref GameObject handObject)
        {
            // 현재 마우스 위치를 화면 좌표로 변환하여 Ray를 쏩니다.
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            // Ray가 충돌한 지점을 저장할 변수
            RaycastHit hit;

            // Ray를 쏴서 충돌한 지점이 있다면
            if (Physics.Raycast(ray, out hit, 2.5f))
            {
                // 충돌 지점의 위치를 아이템을 놓을 위치로 설정합니다.
                handObject.transform.position = hit.point;
            }
            // 손에서 들고 있던 오브젝트의 부모를 해제합니다.
            handObject.transform.SetParent(null);

            // 손에서 들고 있던 오브젝트의 Rigidbody가 존재한다면
            Rigidbody hitRb = handObject.GetComponent<Rigidbody>();
            if (!hitRb != null)
            {
                // Rigidbody의 Kinematic 속성을 해제합니다.
                hitRb.isKinematic = false;
            }

            // 손에서 들고 있던 오브젝트를 null로 초기화합니다.
            handObject = null;
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
        public void Running(bool run)
        {
            if (isGround)
            {
                isRunning = run;
            }
        }

        /// <summary>
        /// 숙이기
        /// </summary>
        public void Crouch()
        {
            if (!isCrouch && isGround)
            {
                // 플레이어를 숙였을 때의 처리
                animator.SetBool("Crouch", true);
                // 컬라이더로 앉은 키 처리
                playerCollider.height = crouchHeight;
                playerCollider.center = new Vector3(playerCollider.center.x, crouchHeight / 2f, playerCollider.center.z);
                // 카메라 위치 조정
                mainCamera.transform.localPosition = new Vector3(0f, crouchHeight, 0f);
                // 손들의 위치 변경
                leftHand.transform.localPosition = new Vector3(leftHand.transform.localPosition.x, crouchHeight / 2f, leftHand.transform.localPosition.z);
                rightHand.transform.localPosition = new Vector3(rightHand.transform.localPosition.x, crouchHeight / 2f, rightHand.transform.localPosition.z);
                isCrouch = true;
            }
            else if (isGround)
            {
                // 플레이어가 숙은 상태에서 일어났을 때의 처리
                animator.SetBool("Crouch", false);
                // 컬라이더로 일어선 키 처리
                playerCollider.height = originalHeight;
                playerCollider.center = new Vector3(playerCollider.center.x, originalHeight / 2f, playerCollider.center.z);
                // 카메라 위치 조정
                mainCamera.transform.localPosition = new Vector3(0f, originalHeight, 0f);
                // 손들의 위치 원위치
                leftHand.transform.localPosition = new Vector3(leftHand.transform.localPosition.x, originalHeight, leftHand.transform.localPosition.z);
                rightHand.transform.localPosition = new Vector3(rightHand.transform.localPosition.x, originalHeight, rightHand.transform.localPosition.z);
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
            if (context.performed)
            {
                if (isCrouch)
                {
                    Crouch();
                }
                else
                {
                    jump = true;  // 점프 상태 설정
                }
            }
        }

        /// <summary>
        /// 왼손 상호작용 연결
        /// </summary>
        /// <param name="context"></param>
        public void OnLeftInteract(InputAction.CallbackContext context)
        {
            left = context.action.ReadValue<float>() > 0.1f;
        }

        /// <summary>
        /// 오른손 상호작용 연결
        /// </summary>
        /// <param name="context"></param>
        public void OnRightInteract(InputAction.CallbackContext context)
        {
            right = context.action.ReadValue<float>() > 0.1f;
        }

        /// <summary>
        /// 달리기 연결
        /// </summary>
        /// <param name="context"></param>
        public void OnRun(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Running(true);  // 달리기 시작
            }
            else if (context.canceled)
            {
                Running(false);  // 달리기 멈춤
            }
        }

        /// <summary>
        /// 숙이기 연결
        /// </summary>
        /// <param name="context"></param>
        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Crouch();
            }
        }
        #endregion

        /// <summary>
        /// 물체 충돌 이벤트
        /// </summary>
        /// <param name="collision"></param>
        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject)
            {
                isGround = true;
            }
        }
    }
}