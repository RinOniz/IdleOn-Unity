using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [Header("Portal Settings")]
    public string sceneToLoad;

    public string destinationSpawnName;

    public static string targetSpawnName;

    private bool isPlayerNear = false;

    private void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.W))
        {
            Teleport();
        }
    }

    private void OnMouseDown()
    {
        if (isPlayerNear)
        {
            Teleport();
        }
    }

    private void Teleport()
    {
        targetSpawnName = destinationSpawnName;

        Debug.Log("Đang dịch chuyển tới map: " + sceneToLoad);

        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
