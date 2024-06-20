using UnityEngine.InputSystem;
using UnityEngine;

namespace eneru7i
{
    public class SeekObject : MonoBehaviour
    {
        // Ž�� ���� ������Ʈ
       public GameObject seekobj;

        //Ž�� ����
        public bool isSeek = false;

        // Ž�� ��ġ ������Ʈ
        public Transform seekpos;
        // ���� ��ġ ������Ʈ
        public Transform originpos;

        private Quaternion seekObjectInitialRotation;
        //Ž�� �Ϸ��� ��ü�� ���� ��ġ��
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
        /// �������� ��� Ž���ϴ� ���
        /// </summary>
        public void Seek()
        {
            if (isSeek && seekobj != null)
            {

                // SeekObject�� �̵�
                seekobj.transform.position = seekpos.position;
                seekobj.transform.SetParent(seekpos);

                // Ž�� �� ���콺�� �̵��� ���� �������� ������ �ٲٱ�
                seekObjectInitialRotation = seekobj.transform.rotation;
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
            if (!isSeek && seekobj != null)
            {
                // ���� ��ġ�� ȸ��, �θ� ��ü�� �ǵ�����
                seekobj.transform.SetParent(seekObjectOriginalParent);
                seekobj.transform.position = originpos.transform.position;
                seekobj.transform.rotation = originpos.transform.rotation;

            }
        }
    }
}