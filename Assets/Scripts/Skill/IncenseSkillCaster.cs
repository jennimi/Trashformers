using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IncenseSkillCaster : MonoBehaviour
{
    public GameObject aoePrefab;
    public float cooldown = 2f;
    private bool canCast = true;

    public void Cast()
    {
        if (!canCast) return;

        Instantiate(aoePrefab, transform.position, Quaternion.identity);
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        canCast = false;
        yield return new WaitForSeconds(cooldown);
        canCast = true;
    }
}
