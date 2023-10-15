using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCon : MonoBehaviour
{
    [SerializeField] float _damage = 1f;
    void OnParticleCollision(GameObject other)
    {

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerCon>().Hitted(transform.gameObject, _damage);
        }

    }
}
