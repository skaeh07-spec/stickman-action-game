using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("스테이지 클리어!");
            // 나중에 여기서 클리어 화면 UI 띄우기
        }
    }
}