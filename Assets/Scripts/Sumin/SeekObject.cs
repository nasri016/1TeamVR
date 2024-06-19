using UnityEngine.InputSystem;
using UnityEngine;

namespace eneru7i
{
    public class SeekObject : MonoBehaviour
    {
        // 탐색 중인 오브젝트
        private GameObject seekobj;
        private PlayerController player;

        //탐색 여부
        public bool isSeek = false;

        // 탐색 위치 오브젝트
        public Transform seekpos;

        private Quaternion seekObjectInitialRotation;
        //탐색 하려는 물체의 원래 위치값
        private Vector3 seekObjectOriginalPosition;
        private Quaternion seekObjectOriginalRotation;
        private Transform seekObjectOriginalParent;

        void Start()
        {
            player = FindObjectOfType<PlayerController>();
        }

        void Update()
        {
            if (isSeek)
            {
                Seek();
            }
        }

        /// <summary>
        /// 아이템을 들고 탐색하는 기능
        /// </summary>
        public void Seek()
        {
            if (seekobj == null)
            {
                Ray ray = player.mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
                if (Physics.Raycast(ray, out RaycastHit hit, 1f))
                {
                    if (hit.collider.CompareTag("Interactable"))
                    {
                        // 원래 위치와 회전, 부모 객체를 저장
                        seekobj = hit.collider.gameObject;
                        seekObjectOriginalPosition = seekobj.transform.position;
                        seekObjectOriginalRotation = seekobj.transform.rotation;
                        seekObjectOriginalParent = seekobj.transform.parent;

                        // SeekObject를 이동
                        seekobj.transform.position = seekpos.position;
                        seekobj.transform.SetParent(seekpos);

                        // 탐색 중 마우스의 이동에 따라 아이템의 방향을 바꾸기
                        seekObjectInitialRotation = seekobj.transform.rotation;
                    }
                }
            }

            // 아이템의 방향을 마우스 이동에 따라 조정
            if (seekobj != null)
            {
                Vector2 mouseDelta = Mouse.current.delta.ReadValue();
                Quaternion rotation = Quaternion.identity;

                // 마우스 이동으로 Y축 회전
                rotation *= Quaternion.Euler(-mouseDelta.y, 0, 0);

                // 좌클릭 중이면 Z축 회전
                if (Mouse.current.leftButton.isPressed)
                {
                    rotation *= Quaternion.Euler(0, 0, mouseDelta.x);
                }

                seekobj.transform.rotation = seekObjectInitialRotation * rotation;
            }
        }

        /// <summary>
        /// 탐색 해제 기능
        /// </summary>
        public void UnSeek()
        {
            if (isSeek && seekobj != null)
            {
                // 원래 위치와 회전, 부모 객체로 되돌리기
                seekobj.transform.SetParent(seekObjectOriginalParent);
                seekobj.transform.position = seekObjectOriginalPosition;
                seekobj.transform.rotation = seekObjectOriginalRotation;

                seekobj = null;
                isSeek = false;
            }
        }
    }
}