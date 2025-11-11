using UnityEngine;

public class EndBarrier : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Zombie zombie))
        {
            Destroy(zombie.gameObject);
            GameManager.Instance.UpdateLives(-1);
        }
    }
}
