using UnityEngine;

public class PersistentManager : MonoBehaviour
{
    public static PersistentManager instance;

    private void Awake()
    {
        // 1. Nếu là bản gốc
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

            // Tìm thằng con EventSystem và bật nó lên
            Transform eventSystem = transform.Find("EventSystem");
            if (eventSystem != null)
            {
                eventSystem.gameObject.SetActive(true);
            }
        }
        // 2. Nếu là hàng fake (nhân bản khi load lại map)
        else if (instance != this)
        {
            // Vì EventSystem trong hàng fake mặc định đã tắt (ở Bước 1), 
            // nên nó không thể gây lỗi được nữa! Cứ thế mà đem đi hủy thôi.
            Destroy(gameObject);
        }
    }
}
