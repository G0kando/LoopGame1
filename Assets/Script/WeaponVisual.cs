using UnityEngine;
using System.Collections;

public class WeaponVisual : MonoBehaviour
{
    public LineRenderer lineRenderer; // Линия (опционально)
    public Light attackLight;          // Свет от удара
    public float lightDuration = 0.1f;

    void Start()
    {
        if (attackLight != null)
            attackLight.enabled = false;
    }

    public void ShowAttack(Vector3 start, Vector3 end)
    {
        // Вспышка света
        if (attackLight != null)
        {
            StartCoroutine(FlashLight());
        }

        // Линия удара
        if (lineRenderer != null)
        {
            StartCoroutine(ShowLine(start, end));
        }
    }

    IEnumerator FlashLight()
    {
        attackLight.enabled = true;
        yield return new WaitForSeconds(lightDuration);
        attackLight.enabled = false;
    }

    IEnumerator ShowLine(Vector3 start, Vector3 end)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        yield return new WaitForSeconds(0.1f);
        lineRenderer.enabled = false;
    }
}