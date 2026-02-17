using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private void OnDefatedAnimationEnd()
    {
        Destroy(gameObject);
    }
}
