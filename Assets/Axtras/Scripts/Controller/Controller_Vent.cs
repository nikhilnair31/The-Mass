using UnityEngine;

public class Controller_Vent : MonoBehaviour 
{
    #region Vars
    [Header("Vent Settings")]
    [SerializeField] private Transform ventEndTransform;
    [SerializeField] private Transform playerTransform;

    [Header("Effect Settings")]
    [SerializeField] private MeshRenderer ventMeshRenderer;
    [SerializeField] private float maxPrecision = 512f;
    [SerializeField] private float minPrecision = 16f;
    [SerializeField] private float maxDistance = 12f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private AnimationCurve precisionCurve = new AnimationCurve(
        new Keyframe(0f, 1f),
        new Keyframe(1f, 0f)
    );

    private Material ventMat;
    #endregion

    private void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        foreach (var mat in ventMeshRenderer.materials) {
            if (mat.HasProperty("Vector1_B2CC132F")) {
                ventMat = mat;
                ventMat.SetFloat("Vector1_B2CC132F", maxPrecision);
                break;
            }
        }
    }

    private void Update() {
        if (ventMat == null) return;

        float distToEnd = Vector3.Distance(ventEndTransform.position, playerTransform.position);
        float clampedDistance = Mathf.Clamp(distToEnd, minDistance, maxDistance);
        float normalizedDistance = (clampedDistance - minDistance) / (maxDistance - minDistance);

        float curveValue = precisionCurve.Evaluate(normalizedDistance);
        float currentPrecision = Mathf.Lerp(minPrecision, maxPrecision, curveValue);

        ventMat.SetFloat("Vector1_B2CC132F", currentPrecision);
    }
}