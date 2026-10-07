using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    private EnemyController enemyParent;

    private void Awake()
    {
        enemyParent = GetComponentInParent<EnemyController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            enemyParent.target = other.transform;
            enemyParent.inRange = true;
            enemyParent.hotZone.SetActive(true);
        }
    }
}
