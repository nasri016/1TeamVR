using System.Collections;
using UnityEngine;

namespace eneru7i
{
    /// <summary>
    /// 시계 관련 퍼즐을 담당
    /// </summary>
    public class ClockPuzzle : MonoBehaviour
    {
        //시침
        public GameObject arrow;
        //넣어야하는 시침
        public GameObject lostarrow;
        //나와야할 책
        public GameObject book;
        
        /// <summary>
        /// 오브젝트 세팅
        /// </summary>
        void Start()
        {
            //시계및 책 숨기기
            arrow.SetActive(false);
            book.SetActive(false);
        }

        /// <summary>
        /// 충돌 판정
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionEnter(Collision collision)
        {
            //화살표를 가져다 놓으면 해결되도록 하기
            if (collision.gameObject == lostarrow)
            {
                StartCoroutine(Solved());
            }
        }

        /// <summary>
        /// 문제 해결
        /// </summary>
        /// <returns></returns>
        IEnumerator Solved()
        {
            Destroy(lostarrow);
            arrow.SetActive(true);
            //종 치는 시간 시다리기
            yield return new WaitForSeconds(2f);

            book.SetActive(true);
        }
    }
}
