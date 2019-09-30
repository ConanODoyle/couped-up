using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggBehavior : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("How long it takes a egg to hatch")]
    float m_hatchTime;

    [SerializeField]
    [Tooltip("Probabilty of egg hatching")]
    float m_hatchProb;

    #endregion

    #region Private Variables
    //Time elaspsed since spawn
    float p_time;

    // Egg's animator
    Animator m_Animator;
    #endregion

    #region Initialization
    private void Start()
    {
        p_time = 0;
        m_Animator = gameObject.GetComponent<Animator>();
    }
    #endregion

    #region Updates
    private void Update()
    {
        p_time += Time.deltaTime;

    }
    #endregion

    #region Hatching Methods
    public bool canHatch()
    {
        if (p_time > m_hatchTime)
        {
            if (Random.value < m_hatchProb)
            {
                return true;
            }
            else
            {
                p_time = 0;
                return false;
            }
        }
        return false;
    }

    public void changeHatchProb(float prob)
    {
        m_hatchProb = prob;
    }
    #endregion

    #region Animation methods
    private IEnumerator IEHatch()
    {
        yield return new WaitForSeconds(2f);
    }
    #endregion
}
