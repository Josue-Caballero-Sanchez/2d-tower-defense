using UnityEngine;

public class EnemyAnimationEvents : MonoBehaviour
{
    private void OnDefatedAnimationEnd()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
