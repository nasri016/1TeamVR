using UnityEngine;

namespace eneru7i
{
    /// <summary>
    /// 옷장관련 퍼즐
    /// </summary>
    public class WardrobePuzzle : MonoBehaviour
    {
        //책
        public GameObject book;
        //잃어버린 책
        public GameObject lostbook;
        //나와야할 테이프
        public GameObject videotape;

        public Animator animator;

        void Start()
        {
            animator = GetComponent<Animator>();
            book.SetActive(false);
        }


        /// <summary>
        /// 충돌 판정
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            //화살표를 가져다 놓으면 해결되도록 하기
            if (collision.gameObject == lostbook)
            {
                Solved();
            }
        }

        /// <summary>
        /// 문제 해결
        /// </summary>
        /// <returns></returns>
        void Solved()
        {
            Destroy(lostbook);
            book.SetActive(true);

            animator.SetBool("Book", true);
        }
    }
}
