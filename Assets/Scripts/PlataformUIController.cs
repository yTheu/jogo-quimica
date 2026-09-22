using UnityEngine;

public class PlatformUIController : MonoBehaviour
{
    [SerializeField] private GameObject mobileControls;

    private void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        mobileControls.SetActive(true);
#else
        mobileControls.SetActive(false);
#endif
    }
}