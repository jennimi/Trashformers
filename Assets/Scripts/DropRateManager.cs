using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DropRateManager : MonoBehaviour
{
    [System.Serializable]
    public class Drops
    {
        public string name;
        public GameObject item;
        public float dropRate;
    }

    public List<Drops> drops;
    public int xpAmount = 10; // default XP drop for this enemy

    public void DropItems()
    {
        float randomNumber = Random.Range(0f, 100f);

        foreach (Drops rate in drops)
        {
            if (randomNumber <= rate.dropRate)
            {
                GameObject drop = Instantiate(rate.item, transform.position, Quaternion.identity);

                var xp = drop.GetComponent<XPItem>();
                if (xp != null)
                    xp.amount = xpAmount;
            }
        }
    }
}

