using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject Cylinder;
    GameObject Cyl;
    
    private Transform poolParent;
    private static int currentPoolElementID = 1;

    List<int> perfect = new List<int>();
    private static bool gameover = false;
    public float scaleSpeed = 5f;
    private int score = 0;
    public Text TextScore;

    public float minSize = 0.1f;
    public float maxSize = 1f;
    private float holdingDurationCurrent = 0f;
    private bool isHolding = false;
    private bool m_mouseWasReleased = true;


    private void Start()
    {
        poolParent = transform;
        Cyl = Cylinder;
        Instantiate(Cylinder, transform.position, transform.rotation, poolParent);        
    }
    void Update()
    {
        if (!gameover)
        {            
            if (Input.GetMouseButtonDown(0) && m_mouseWasReleased)
            {
                holdingDurationCurrent = 0f;
                isHolding = true;
                m_mouseWasReleased = false;
                Instantiate(Cylinder, transform.position, transform.rotation, poolParent);
                GameObject CylinderCopy = poolParent.GetChild(currentPoolElementID).gameObject;
                CylinderCopy.transform.position = new Vector3(0, Cyl.transform.position.y + 0.5f, 0);
                CylinderCopy.transform.localScale = new Vector3(minSize, 0.25f, minSize);
                Cyl = CylinderCopy;
            }
            else if (isHolding && (Input.GetMouseButtonUp(0)))
            {
                if (Cyl.transform.localScale.x > poolParent.GetChild(currentPoolElementID - 1).transform.localScale.x)
                {
                    GameOver();
                }
                else
                {                    
                    if (poolParent.GetChild(currentPoolElementID - 1).transform.localScale.x * 0.95 <= Cyl.transform.localScale.x)
                    {
                        PerfectMode();
                    }                    
                    currentPoolElementID++;
                    score++;
                    TextScore.text = score.ToString();
                }
                
                Reset();

            }

            if (isHolding)
            {
                holdingDurationCurrent += Time.deltaTime;
                if ((holdingDurationCurrent + minSize) < poolParent.GetChild(currentPoolElementID - 1).transform.localScale.x * 1.1f)
                    Cyl.transform.localScale = new Vector3(minSize + holdingDurationCurrent, 0.25f, minSize + holdingDurationCurrent);
                else
                    GameOver();

            }

            if (!m_mouseWasReleased)
            {
                m_mouseWasReleased = Input.GetMouseButtonUp(0);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                Restart();
        }
    }

    private void GameOver()
    {
        gameover = true;
        Cyl.GetComponent<Renderer>().material.color = Color.red;
        //Destroy(Cyl, 0.3f);        
    }

    public void Restart()
    {
        for(int i = currentPoolElementID; i >= 0; i--)           
            Destroy(poolParent.GetChild(i).gameObject);

        Cyl = Cylinder;
        Instantiate(Cylinder, transform.position, transform.rotation, poolParent);
        currentPoolElementID = 1;
        perfect.Clear();
        Reset();
        gameover = false;
        score = 0;
        TextScore.text = score.ToString();
    }

    private IEnumerator test()
    {
        yield return null;
    }

    private void PerfectMode()
    {
        perfect.Add(currentPoolElementID);
        Cyl.GetComponent<Renderer>().material.color = Color.green;
        StartCoroutine(LerpUp(scaleSpeed));
    }

    IEnumerator LerpUp(float duration)
    {
        Vector3 ScaleTo = new Vector3();



        for (int i = currentPoolElementID; i >= 0; i--)
        {
            if (i == currentPoolElementID)
            {
                if ((Cyl.transform.localScale.x + 0.2f) > 1)
                    ScaleTo = new Vector3(1f, Cyl.transform.localScale.y, 1f);
                else
                    ScaleTo = new Vector3(Cyl.transform.localScale.x + 0.2f, Cyl.transform.localScale.y, Cyl.transform.localScale.z + 0.2f);
                float progress = 0;
                Vector3 c = new Vector3(Cyl.transform.localScale.x + 0.4f, Cyl.transform.localScale.y, Cyl.transform.localScale.z + 0.4f);
                while (progress <= 1)
                {
                    Cyl.transform.localScale = Vector3.Lerp(Cyl.transform.localScale, c, progress);
                    progress += Time.deltaTime * duration;
                    yield return null;
                }
                Cyl.transform.localScale = c;
                StartCoroutine(LerpDown(Cyl.transform.localScale, ScaleTo, duration, i));
            }
            else
            {
                if (!perfect.Contains(i))
                {
                    if ((poolParent.GetChild(i).transform.localScale.x + 0.3f) * 0.8f > 1f)
                        ScaleTo = new Vector3(1f, poolParent.GetChild(i).transform.localScale.y, 1f);
                    else
                        ScaleTo = new Vector3((poolParent.GetChild(i).transform.localScale.x + 0.3f) * 0.8f, poolParent.GetChild(i).transform.localScale.y, (poolParent.GetChild(i).transform.localScale.z + 0.3f) * 0.8f);
                    float progress = 0;
                    Vector3 c = new Vector3(poolParent.GetChild(i).transform.localScale.x + 0.3f, poolParent.GetChild(i).transform.localScale.y, poolParent.GetChild(i).transform.localScale.z + 0.3f);
                    while (progress <= 1)
                    {
                        poolParent.GetChild(i).transform.localScale = Vector3.Lerp(poolParent.GetChild(i).transform.localScale, c, progress);
                        progress += Time.deltaTime * duration;
                        yield return null;
                    }
                    poolParent.GetChild(i).transform.localScale = c;
                    StartCoroutine(LerpDown(poolParent.GetChild(i).transform.localScale, ScaleTo, duration, i));
                }
            }
        }
    }

    IEnumerator LerpDown(Vector3 startValue, Vector3 endValue, float duration, int i)
    {
        float progress = 0;        
        while (progress <= 1)
        {
            poolParent.GetChild(i).transform.localScale = Vector3.Lerp(startValue, endValue, progress);
            progress += Time.deltaTime * duration;
            yield return null;
        }
        poolParent.GetChild(i).transform.localScale = endValue;
    }

    private void Reset()
    {        
        holdingDurationCurrent = 0f;
        isHolding = false;
        
    }
    
    public static int getCurrentPoolElementID()
    {
        return currentPoolElementID-1;
    }
    public static bool getGameover()
    {
        return gameover;
    }
}