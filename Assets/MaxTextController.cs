using UnityEngine;

public class MaxTextController : MonoBehaviour
{
    public GameObject MaxText;
    public void ShouldEnable(bool value)
    {
        if (value)
        {
            MaxText.SetActive(true);
        }
        else
        {
            MaxText.SetActive(false);
            
        }
    }
}
