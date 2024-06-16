
using UnityEngine;
using UnityEngine.SceneManagement;

namespace eneru7i
{
    /// <summary>
    /// 게임 매니저 스크립트
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        /// <summary>
        /// 새 게임 시작
        /// </summary>
        public void NewGame()
        {
            SceneManager.LoadScene("MainScene");
        }

        /// <summary>
        /// 게임 이어하기
        /// </summary>
        public void ContinueGame()
        {
            SceneManager.LoadScene("MainScene");
        }

        /// <summary>
        /// 옵션 관련 창
        /// </summary>
        public void Options()
        {

        }


    }
}
