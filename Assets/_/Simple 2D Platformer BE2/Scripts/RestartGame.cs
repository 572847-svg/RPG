using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public void LoadCurretScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Practice");
        Time.timeScale = 1;
    }
}
