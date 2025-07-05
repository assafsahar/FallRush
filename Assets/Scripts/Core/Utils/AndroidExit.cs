using UnityEngine;

namespace Core.Utils
{
    public class AndroidExit : MonoBehaviour
    {
        private void Update()
        {
            // On Android, KeyCode.Escape is mapped to the back button
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
