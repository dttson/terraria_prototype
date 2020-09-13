using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurtle : EnemyObjectBase
{
    //public override void attack()
    //{
    //    base.attack();
    //    StopAllCoroutines();
    //    StartCoroutine(coroutineWaitSpikeOut());
    //}

    //private IEnumerator coroutineWaitSpikeOut()
    //{
    //    while (true)
    //    {
    //        m_Animator.SetTrigger("SpikeIn");
    //        State = EnemyState.ATTACK;

    //        yield return new WaitForSeconds(1f);

    //        m_Animator.SetTrigger("SpikeOut");
    //        State = EnemyState.IDLE;

    //        yield return new WaitForSeconds(1f);
    //    }
    //}
}
