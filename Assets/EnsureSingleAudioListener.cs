using UnityEngine;

public class EnsureSingleAudioListener : MonoBehaviour
{
    void Start()
    {
        // Find all active AudioListener components in the scene.
        AudioListener[] listeners = FindObjectsOfType<AudioListener>();

        // If there are more than one AudioListener, disable the extras.
        if (listeners.Length > 1)
        {
            for (int i = 1; i < listeners.Length; i++)
            {
                listeners[i].enabled = false;
            }
        }
    }
}
