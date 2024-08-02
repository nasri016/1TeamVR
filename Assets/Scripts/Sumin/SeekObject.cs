using UnityEngine.InputSystem;
using UnityEngine;

namespace eneru7i
{
    public class SeekObject : MonoBehaviour
    {
        // 탐색 중인 오브젝트
       public GameObject seekobj;

        //탐색 여부
        public bool isSeek = false;

        // 탐색 위치 오브젝트
        public Transform seekpos;
        // 원래 위치 오브젝트
        public Transform originpos;

        private Quaternion seekObjectInitialRotation;
        //탐색 하려는 물체의 원래 위치값
        private Transform seekObjectOriginalParent;

        void Start()
        {

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
            if (isSeek && seekobj != null)
            {

                // SeekObject를 이동
                seekobj.transform.position = seekpos.position;
                seekobj.transform.SetParent(seekpos);

                // 탐색 중 마우스의 이동에 따라 아이템의 방향을 바꾸기
                seekObjectInitialRotation = seekobj.transform.rotation;
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
            if (!isSeek && seekobj != null)
            {
                // 원래 위치와 회전, 부모 객체로 되돌리기
                seekobj.transform.SetParent(seekObjectOriginalParent);
                seekobj.transform.position = originpos.transform.position;
                seekobj.transform.rotation = originpos.transform.rotation;

            }
        }
    }
}