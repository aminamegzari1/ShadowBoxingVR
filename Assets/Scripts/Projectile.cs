using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3f;
    public float lifeTime = 5f;

    [Header("Type")]
    public bool isObstacle = false;
    private bool hasBeenHit = false;
    
    void Start()
    {
        // Auto-destruction après X secondes
        Destroy(gameObject, lifeTime);
    }
    
    void Update()
    {
        // Avance vers le joueur (direction -Z)
        transform.Translate(Vector3.back * speed * Time.deltaTime, Space.World);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        // Vérifie si c'est une main
        if (collision.gameObject.CompareTag("Hand"))
        {
            if (isObstacle)
            {
                // Obstacle touché = pénalité
                Debug.Log("❌ OBSTACLE TOUCHÉ ! -10 points");
                GameManager gm = FindFirstObjectByType<GameManager>();
                if (gm != null) gm.AddScore(-10);
            }
            else
            {
                // Cible touchée = points
                PunchDetector punch = collision.gameObject.GetComponent<PunchDetector>();
                if (punch != null && punch.speed >= 1.5f)
                {
                    Debug.Log($"🎯 CIBLE TOUCHÉE ! +10 points (vitesse: {punch.speed:F2})");
                    GameManager gm = FindFirstObjectByType<GameManager>();
                    if (gm != null) gm.AddScore(10);
                }
            }
            
            // Détruit l'objet
            Destroy(gameObject);
        }
    }
    void OnDestroy()
{
    // Si l'objet est détruit sans être touché
    if (!hasBeenHit)
    {
        GameManager gm = FindFirstObjectByType<GameManager>();
        if (isObstacle)
        {
            // OBSTACLE évité ! +5 points
            Debug.Log("🚧 OBSTACLE ÉVITÉ ! +5 points");
            if (gm != null) gm.AddScore(5);
        }
        else
        {
            // CIBLE manquée, pas de points
        }
    }
}

}