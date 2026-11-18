using UnityEngine;

[ExecuteInEditMode]
public class MagicLightSource : MonoBehaviour
{
    public Material targetMaterial;
    private Light spotLight;

    void Start()
    {
        spotLight = GetComponent<Light>();
    }

    void Update()
    {
        if (targetMaterial == null) return;

        Vector3 lightPos = transform.position;
        Vector3 lightDir = transform.forward;
        float spotAngle = 45f;

        if (spotLight != null)
        {
            spotAngle = spotLight.spotAngle;
        }

        // Stuur de data door naar de URP shader
        targetMaterial.SetVector("_LightPosition", lightPos);
        targetMaterial.SetVector("_LightDirection", lightDir);
        targetMaterial.SetFloat("_LightAngle", spotAngle);
    }
}