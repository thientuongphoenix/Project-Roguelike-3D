using UnityEngine;

public class PlayerBehindObstacal_Handler : MonoBehaviour
{
    public Material defaultMaterial;  // Material bình thường của nhân vật
    public Material seeThroughMaterial; // Material bóng khi bị che
    private Renderer characterRenderer;

    void Start()
    {
        characterRenderer = GetComponentInChildren<Renderer>();
    }

    void Update()
    {
        CheckIfObstructed();
    }

    void CheckIfObstructed()
    {
        Vector3 cameraToCharacter = transform.position - Camera.main.transform.position;
        Ray ray = new Ray(Camera.main.transform.position, cameraToCharacter.normalized);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, cameraToCharacter.magnitude))
        {
            if (hit.collider.gameObject != gameObject) // Nếu có vật khác che
            {
                characterRenderer.material = seeThroughMaterial;
            }
            else
            {
                characterRenderer.material = defaultMaterial;
            }
        }
        else
        {
            characterRenderer.material = defaultMaterial;
        }
    }
}
