using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance { get; private set; }
   
    private Queue<ParticleSystem> hitEffectMap;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);

            return;
        }

        instance = this;

        hitEffectMap = new Queue<ParticleSystem>();
    }
    
    private IEnumerator ReturnEffect(ParticleSystem effect, Queue<ParticleSystem> pool)
    { 
        while (true)
        {
            if (effect == null || pool == null)
            {
                yield break;
            }

            yield return null;

            if (!effect.IsAlive())
            {
                pool.Enqueue(effect);

                effect.gameObject.SetActive(false);

                yield break;
            }
        }
    }
}