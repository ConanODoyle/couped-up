using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickControl : MonoBehaviour
{
	#region Editor Variables
	[SerializeField]
	[Tooltip("The distance it will stay behind its target object")]
	private float m_followDistance;

	[SerializeField]
	[Tooltip("Distance it will move per tick")]
	private float m_moveDist;
	#endregion

	//Assigned when it hatches
	private GameObject m_targetObject = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
	private void Update()
	{
		if (m_targetObject == null)
		{
//			m_targetObject = PlayerController.GetLastChick(m_parent);
			Collider[] allOverlaps = Physics.OverlapSphere(transform.position, 100f);
			Debug.Log(string.Format("Hit {0} objects", allOverlaps.Length));
			for (int i = 0; i < allOverlaps.Length; i++)
			{
				GameObject target = allOverlaps[i].gameObject;
				if (target.tag == "Player")
				{
					Debug.Log("Found player!");
					m_targetObject = target;
					break;
				}
			}
//			m_targetObject = PlayerController.GetLastChick(m_targetObject);
		}

		Transform t_transform = m_targetObject.transform;

		Vector3 lookVector = t_transform.position - transform.position;
		transform.rotation = Quaternion.LookRotation(lookVector, Vector3.up);
		float dist = (t_transform.position - transform.position).magnitude;
		if (dist > m_followDistance)
		{
			dist = Mathf.Min(dist, m_moveDist / 10);

			transform.Translate(Vector3.forward * dist);
		}	
	}

    public void setTarget(GameObject tar)
    {
        m_targetObject = tar;
    }
}
