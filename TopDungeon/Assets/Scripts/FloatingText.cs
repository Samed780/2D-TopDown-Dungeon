using UnityEngine;
using UnityEngine.UI;

public class FloatingText 
{
    public GameObject go;
    public Text text;
    public Vector3 motion;
    public bool active;
    public float duration;
    public float lastShown;

    public void Show()
    {
        active = true;
        lastShown = Time.time;
        go.SetActive(active);
    }

    public void Hide()
    {
        active = false;
        go.SetActive(active);
    }

    public void UpdateFloatingText()
    {
        if (!active)
            return;
        if (Time.time - lastShown > duration)
            Hide();
        go.transform.position += motion * Time.deltaTime;
    }
}
