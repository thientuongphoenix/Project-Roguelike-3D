using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    private ParticleSystem trailEffect;

    void Start()
    {
        var renderer = trailEffect.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));

        // Tạo GameObject chứa Particle System
        GameObject trailGO = new GameObject("BulletTrail");
        trailGO.transform.SetParent(transform); // Gắn hiệu ứng vào viên đạn
        trailGO.transform.localPosition = Vector3.zero;

        // Thêm Particle System Component
        trailEffect = trailGO.AddComponent<ParticleSystem>();
        var main = trailEffect.main;

        // 🔥 Cấu hình Particle System
        main.startLifetime = 0.3f; // Vệt sáng tồn tại 0.3 giây
        main.startSpeed = 0; // Hạt không di chuyển, chỉ theo viên đạn
        main.startSize = 0.15f; // Độ lớn hạt
        main.startColor = new Color(1f, 0.8f, 0f, 1f); // Vàng đậm (RGBA)

        // 🔥 Kích hoạt phát hạt theo đường đi
        var emission = trailEffect.emission;
        emission.rateOverDistance = 50; // Càng cao thì hiệu ứng càng dày

        // 🔥 Định dạng hiệu ứng theo dải (Trail)
        var shape = trailEffect.shape;
        shape.shapeType = ParticleSystemShapeType.Cone; // Dùng hình nón
        shape.angle = 0; // Tạo ra vệt dài không bị lan rộng
        shape.radius = 0.05f; // Độ rộng hiệu ứng

        // 🔥 Làm nhạt dần màu về trắng
        var colorOverLifetime = trailEffect.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(new Color(1f, 0.8f, 0f), 0.0f), // Đầu màu vàng
                                     new GradientColorKey(Color.white, 1.0f) }, // Cuối nhạt dần thành trắng
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), // Đầu trong suốt
                                     new GradientAlphaKey(0.0f, 1.0f) } // Cuối biến mất
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

        // 🔥 Làm hạt nhỏ dần
        var sizeOverLifetime = trailEffect.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = new AnimationCurve(
            new Keyframe(0.0f, 1.0f),  // Kích thước ban đầu
            new Keyframe(1.0f, 0.0f)   // Nhỏ dần về 0
        );
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1.0f, sizeCurve);

        // Chạy hiệu ứng ngay khi viên đạn xuất hiện
        trailEffect.Play();
    }

    void OnDestroy()
    {
        if (trailEffect != null)
        {
            trailEffect.transform.SetParent(null); // Không bị xóa ngay
            Destroy(trailEffect.gameObject, 1.0f); // Xóa sau 1 giây
        }
    }
}
