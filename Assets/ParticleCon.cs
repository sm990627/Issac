using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCon : MonoBehaviour
{
    [SerializeField] float _damage = 0.5f;
    void OnParticleCollision(GameObject other)
    {

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerCon>().Hitted(transform.gameObject, _damage);
        }

    }
}
