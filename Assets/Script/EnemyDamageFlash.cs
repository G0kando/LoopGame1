using UnityEngine;
using System.Collections;

public class EnemyDamageFlash : MonoBehaviour
{
    public Material flashMaterial;
    public float flashDuration = 0.1f;

    private Material originalMaterial;
    private Renderer enemyRenderer;

    void Start()
    {
        enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer != null)
        {
            originalMaterial = enemyRenderer.material;
        }
    }

    public void Flash()
    {
        if (enemyRenderer != null && flashMaterial != null)
        {
            StartCoroutine(FlashRoutine());
        }
    }

    IEnumerator FlashRoutine()
    {
        enemyRenderer.material = flashMaterial;
        yield return new WaitForSeconds(flashDuration);
        enemyRenderer.material = originalMaterial;
    }
}