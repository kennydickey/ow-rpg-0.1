using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField] float healthPoints = 100f;
    public bool isDead = false;
    public bool IsDead()
    {
        return isDead;
    }

    public void TakeDamage(float damage)
    {
        healthPoints = Mathf.Max(healthPoints - damage, 0);
        if(healthPoints == 0 && isDead == false)
        {
            Die();
        }
    }

    private void Die()
    {       
        GetComponent<Animator>().SetTrigger("death");
        isDead = true;
    }
}
