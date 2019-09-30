using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Editor Variables
    [SerializeField]
    [Tooltip("How far each unit moves per unit of time")]
    float m_moveSpeed;

    [SerializeField]
    [Tooltip("How long it takes to spawn an egg")]
    float m_eggSpawnTime;

    [SerializeField]
    [Tooltip("How high we can jump")]
    float m_jumpForce;

    [SerializeField]
    [Tooltip("The Prefab of the egg we are spawning")]
    GameObject m_Egg;

    [SerializeField]
    [Tooltip("The Prefab of the chick we are spawning")]
	GameObject m_Chick;

	[SerializeField]
	[Tooltip("The camera following the object, if any")]
	GameObject m_Camera;

    [SerializeField]
    [Tooltip("The text to update score")]
    Text m_Score;
    #endregion

    #region Private Variables 
    //The number of eggs currently following the chicken
    int p_numEggs;

    //The number of chicks currently following the chicken
    int p_numChicks;

    //The number of chicks and eggs currently following the chicken
    int p_numChildren;

    //The Vector which we use to translate our object by the moveDist
    Vector3 p_moveVector;

    //List of all of the children (eggs and chicks)
    List<GameObject> p_allChildren;

    //The time that has passed between egg spawns
    float p_time;

    //An bool representing if we are jumping
    bool p_jump;

    //An bool representing if we CAN jump
    bool p_canJump;

    //The last chicken spawned
    GameObject p_lastChick;

    //The chicken animator
    Animator p_chickenAnimator;
    #endregion

	//Used for record keeping, public
	public List<GameObject> p_allChicks;

    //The rigidbody of the player
    Rigidbody p_RB;

    #region Initialization
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        p_RB = GetComponent<Rigidbody>();
        p_chickenAnimator = GetComponent<Animator>();
        p_numEggs = 0;
        p_numChicks = 0;
        p_numChildren = 0;
        p_lastChick = null;
        p_allChildren = new List<GameObject>();
        p_jump = false;
        p_canJump = true;
        InvokeRepeating("Spawn", m_eggSpawnTime, m_eggSpawnTime);
    }
    #endregion

    #region Updates
    // Update is called once per frame
    void Update()
    {
        p_time += Time.deltaTime;

       
		transform.eulerAngles = new Vector3(0, m_Camera.transform.eulerAngles.y, m_Camera.transform.eulerAngles.z);     ;

        Vector3 firstPos = transform.position;

        transform.position += transform.forward * Time.deltaTime * m_moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && p_canJump)
        {
            p_chickenAnimator.SetTrigger("p_jump");
        }

        Hatch();

    }
    #endregion 

    #region Spawning Functions 
    void Spawn()
    {
        if(p_canJump)
        {
            GameObject Egg = Instantiate(m_Egg, transform.position - transform.forward * 3, Quaternion.identity);
            p_allChildren.Add(Egg);
            p_numEggs += 1;
        }
    }

    private void Hatch()
    {
        for (int i = 0; i < p_allChildren.Count; i++)
        {
            if (p_allChildren[i].gameObject.CompareTag("Egg") && p_allChildren[i].GetComponent<EggBehavior>().canHatch())
            {
                Vector3 pos = p_allChildren[i].transform.position;


                Animator egg_Animator = p_allChildren[i].gameObject.GetComponent<Animator>();
                egg_Animator.SetTrigger("egg_hatch");

                StartCoroutine(IEHatch());
				Destroy(p_allChildren[i].gameObject);

				p_allChildren[i] = Instantiate(m_Chick, pos + Vector3.up * 2, Quaternion.identity);
                if (p_lastChick != null)
                {
                    p_allChildren[i].gameObject.GetComponent<ChickControl>().setTarget(p_lastChick);
                }
                else
                {
                    p_allChildren[i].gameObject.GetComponent<ChickControl>().setTarget(gameObject);
                }
                p_lastChick = p_allChildren[i];
                p_numEggs -= 1;
                p_numChicks += 1;
                updateScore();

            }
        }
    }
    #endregion

    #region Collision Methods
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Egg") || collision.gameObject.CompareTag("Wall"))
        {
            SceneManager.LoadScene("MainMenuScene");
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            p_canJump = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            p_canJump = false;
        }
    }
    #endregion

    #region Animation methods
    private IEnumerator IEHatch()
    {
        yield return new WaitForSeconds(2f);
    }
    #endregion

    #region Score
    private void updateScore()
    {
        m_Score.text = "Chickes Hatched : " + p_numChicks.ToString();
    }
    #endregion
}





