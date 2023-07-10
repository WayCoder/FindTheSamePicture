using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public enum HitEffectKey
    {
        Planet,
        Garbage,
        End
    }

    public static ObjectManager instance { get; private set; }
    private Dictionary<int, Queue<Card>> garbageMap;
    private Dictionary<HitEffectKey, Queue<ParticleSystem>> hitEffectMap;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);

            return;
        }

        instance = this;

        garbageMap = new Dictionary<int, Queue<Card>>();

        hitEffectMap = new Dictionary<HitEffectKey, Queue<ParticleSystem>>();    
    }
    public void AllUnactivate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    public void CreateGarbage(int key, int count)
    {
        if (key >= GameManager.instance.data.objectData.garbages.Length)
        {
            return;
        }

        if (!garbageMap.ContainsKey(key))
        {
            garbageMap.Add(key, new Queue<Card>());
        }

        for (int i = 0; i < count; i++)
        {
            Instantiate(GameManager.instance.data.objectData.garbages[key], transform).gameObject.SetActive(false);
        }
    }
    public void ReturnGarbage(Card garbage, int key)
    { 
        if (!garbageMap[key].Contains(garbage))
        {
            garbageMap[key].Enqueue(garbage);
        }
    }
    public Card GetGarbage(int key)
    {
        if (!garbageMap.ContainsKey(key))
        {
            garbageMap.Add(key, new Queue<Card>());
        }

        if (garbageMap[key].Count == 0)
        {
            CreateGarbage(key, GameManager.instance.data.gamestateData.createGarbageCount);
        }

        return garbageMap[key].Dequeue();
    }
    public void CreateHitEffect(HitEffectKey key, int count)
    {
        if ((int)key >= GameManager.instance.data.objectData.hitEffect.Length)
        {
            return;
        }
     
        if (!hitEffectMap.ContainsKey(key))
        {
            hitEffectMap.Add(key, new Queue<ParticleSystem>());
        }

        for (int i = 0; i < count; i++)
        {
            ParticleSystem effect = Instantiate(GameManager.instance.data.objectData.hitEffect[(int)key], transform);

            effect.gameObject.SetActive(false);

            hitEffectMap[key].Enqueue(effect);
        }
    }
    public void OnHitEffect(HitEffectKey key, Vector2 position)
    {
        if (!hitEffectMap.ContainsKey(key))
        {
            hitEffectMap.Add(key, new Queue<ParticleSystem>());
        }

        if (hitEffectMap[key].Count == 0)
        {
            CreateHitEffect(key, GameManager.instance.data.gamestateData.createHitEffectCount);
        }

        ParticleSystem effect = hitEffectMap[key].Dequeue();

        effect.transform.position = position;

        effect.gameObject.SetActive(true);

        effect.Play();

        AudioSource source = effect.GetComponent<AudioSource>();

        if (source != null)
        {
            source.Play();
        }
           
        StartCoroutine(ReturnEffect(effect, hitEffectMap[key]));
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