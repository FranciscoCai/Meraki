using UnityEngine;

public class PaintBottleManager : MonoBehaviour
{
    [SerializeField] private Data_Pintor pintor;
    [SerializeField] private GameObject[] paintBottles;

    private int currentIndex = 0;

    // Niveles de transparencia
    private float solidAlpha = 1.0f;
    private float transparentAlpha = 0.3f;

    void Start()
    {
        // Inicializar objetos con transparencia y desactivados
        for (int i = 0; i < paintBottles.Length; i++)
        {
            SetAlpha(paintBottles[i], transparentAlpha);
            paintBottles[i].SetActive(false);
        }

        ActivateNextPair();
    }

    void Update()
    {
        if (currentIndex < paintBottles.Length && paintBottles[currentIndex] == null)
        {
            AdvanceProgression();
        }
    }

    void AdvanceProgression()
    {
        if (currentIndex + 1 < paintBottles.Length)
        {
            SetAlpha(paintBottles[currentIndex + 1], solidAlpha);
        }

        currentIndex++;

        if (currentIndex + 1 < paintBottles.Length)
        {
            paintBottles[currentIndex + 1].SetActive(true);
            SetAlpha(paintBottles[currentIndex + 1], transparentAlpha);
        }

        if (currentIndex < paintBottles.Length)
        {
            pintor.P_objetivo = paintBottles[currentIndex].transform;
        }
    }

    void ActivateNextPair()
    {
        if (paintBottles.Length > 0)
        {
            paintBottles[0].SetActive(true);
            SetAlpha(paintBottles[0], solidAlpha);

            if (paintBottles.Length > 1)
            {
                paintBottles[1].SetActive(true);
                SetAlpha(paintBottles[1], transparentAlpha);
            }

            pintor.P_objetivo = paintBottles[0].transform;
        }
    }

    void SetAlpha(GameObject obj, float alpha)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        if (rend != null)
        {
            Material mat = rend.material;
            Color color = mat.color;
            color.a = alpha;
            mat.color = color;

            // Asegura que el modo del shader permite transparencia
            if (alpha < 1.0f)
            {
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            }
            else
            {
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                mat.SetInt("_ZWrite", 1);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.DisableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = -1;
            }
        }
    }
}