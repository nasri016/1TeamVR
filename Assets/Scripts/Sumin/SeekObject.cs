using UnityEngine.InputSystem;
using UnityEngine;

namespace eneru7i
{
    public class SeekObject : MonoBehaviour
    {
        // Ž�� ���� ������Ʈ
        private GameObject seekobj;
        private PlayerController player;

        //Ž�� ����
        public bool isSeek = false;

        // Ž�� ��ġ ������Ʈ
        public Transform seekpos;

        private Quaternion seekObjectInitialRotation;
        //Ž�� �Ϸ��� ��ü�� ���� ��ġ��
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
        /// �������� ��� Ž���ϴ� ���
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
                        // ���� ��ġ�� ȸ��, �θ� ��ü�� ����
                        seekobj = hit.collider.gameObject;
                        seekObjectOriginalPosition = seekobj.transform.position;
                        seekObjectOriginalRotation = seekobj.transform.rotation;
                        seekObjectOriginalParent = seekobj.transform.parent;

                        // SeekObject�� �̵�
                        seekobj.transform.position = seekpos.position;
                        seekobj.transform.SetParent(seekpos);

                        // Ž�� �� ���콺�� �̵��� ���� �������� ������ �ٲٱ�
                        seekObjectInitialRotation = seekobj.transform.rotation;
                    }
                }
            }

            // �������� ������ ���콺 �̵��� ���� ����
            if (seekobj != null)
            {
                Vector2 mouseDelta = Mouse.current.delta.ReadValue();
                Quaternion rotation = Quaternion.identity;

                // ���콺 �̵����� Y�� ȸ��
                rotation *= Quaternion.Euler(-mouseDelta.y, 0, 0);

                // ��Ŭ�� ���̸� Z�� ȸ��
                if (Mouse.current.leftButton.isPressed)
                {
                    rotation *= Quaternion.Euler(0, 0, mouseDelta.x);
                }

                seekobj.transform.rotation = seekObjectInitialRotation * rotation;
            }
        }

        /// <summary>
        /// Ž�� ���� ���
        /// </summary>
        public void UnSeek()
        {
            if (isSeek && seekobj != null)
            {
                // ���� ��ġ�� ȸ��, �θ� ��ü�� �ǵ�����
                seekobj.transform.SetParent(seekObjectOriginalParent);
                seekobj.transform.position = seekObjectOriginalPosition;
                seekobj.transform.rotation = seekObjectOriginalRotation;

                seekobj = null;
                isSeek = false;
            }
        }
    }
}