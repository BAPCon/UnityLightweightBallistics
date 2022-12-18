using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lwballistics : MonoBehaviour
{
    public List<projectile> projectiles = new List<projectile>();
    public TextAsset jsonFile;
    public List<float[]> data_rows = new List<float[]>();

    float fire_rate_timer = 0f;
    public float fire_rate = .1f;

    public float clcrate = 0f;
    public float rate_timer = 0f;
    public int prj = 0;
    public float accuracy_offset;

    // Start is called before the first frame update
    void Start()
    {
        

        string[] lines = jsonFile.text.Split('\n');
        foreach(string line in lines)
        {
            string[] data_strings = line.Split('|');
            float[] nl = new float[]
            {
                float.Parse(data_strings[0]),
                float.Parse(data_strings[1]),
                float.Parse(data_strings[2])
            };
            data_rows.Add(nl);
        }

    }

    // Update is called once per frame
    void Update()
    {
        fire_rate_timer += Time.deltaTime;
        if (Input.GetMouseButton(0) && fire_rate_timer >= fire_rate)
        {
            fire_rate_timer = 0f;
            projectiles.Add(new projectile(transform.forward, transform.position, 2f, data_rows, accuracy_offset));
            prj += 1;
        }
        if (prj != 0 && prj <= 100)
        {
            rate_timer += Time.deltaTime;
            clcrate = rate_timer / prj;
        }

        projectiles = TickProjectiles();
        
    }

    List<projectile> TickProjectiles()
    {
        List<projectile> tempList = new List<projectile>();

        foreach(projectile p in projectiles)
        {
            p.tick();

            if (p.is_alive())
            {
                tempList.Add(p);
            }
        }

        return tempList;

    }

    
}

[System.Serializable]
public class projectile
{
    public float life_time;
    public float next_time_tick;
    public float velocity = 5f;
    Vector3 lpos;
    Vector3 novert;
    List<float[]> datarows;
    int dindex = 0;

    int j = 1;
    float life_time_length;

    bool maxed_out = false;

    public Vector3 starting_position;

    public GameObject gameObject;

    public projectile(Vector3 direction, Vector3 position, float life_time, List<float[]> drows, float accuracy_offset)
    {
        this.life_time = life_time;
        this.life_time_length = life_time;
        this.next_time_tick = drows[0][2];
        this.starting_position = position;
        gameObject = new GameObject();
        gameObject.transform.position = position;
        novert = position;
        gameObject.transform.forward = direction.normalized;
        datarows = drows;


    }

    public void tick()
    {

        this.life_time -= Time.deltaTime;
        while(dindex < datarows.Count - 1 && (1000f*life_time_length) - (1000f*life_time) >= next_time_tick)
        {
            dindex = Mathf.Min(dindex+1, datarows.Count - 1);
            if (dindex == datarows.Count + 1)
            {
                maxed_out = true;
            }
            next_time_tick = datarows[dindex][2];
        }
        changePosition();
        this.DrawDebugRay();
        CheckHits();
    }


    void changePosition()
    {
        lpos = gameObject.transform.position;
        novert += gameObject.transform.forward * Time.deltaTime * datarows[dindex][1];

        gameObject.transform.position = novert + (datarows[dindex][0] * gameObject.transform.up);
        
        if (gameObject.transform.position.z* 1.09361f >= (j*25f))
        {
            //Debug.Log((gameObject.transform.position.z * 1.09361f).ToString() + "  |  " + (gameObject.transform.position.y / 0.0254f).ToString());
            j += 1;
        }
    }

    public bool is_alive()
    {
        if (this.life_time <= 0) {
            GameObject.Destroy(gameObject);    
            return false; 
        }
        return true;
    }

    void DrawDebugRay()
    {
        Debug.DrawRay(lpos, (gameObject.transform.position - lpos), Color.red, Time.deltaTime);
    }

    void CheckHits()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(lpos, (gameObject.transform.position - lpos), out hit, datarows[dindex][1] * Time.deltaTime))
        {
            Debug.DrawRay(lpos, (gameObject.transform.position - lpos) * hit.distance, Color.yellow, 20f);
            Debug.Log("Did Hit");
        }
    }
}
