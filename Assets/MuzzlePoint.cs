using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzlePoint : MonoBehaviour
{
    public float velocity;
    public float tat;
    public static float[] _50yardDrops9mm;
    public static float[] _50yardDrops40sw;
    GameObject gnwe;
    List<Projectile> activeProjectiles = new List<Projectile>();
    // Start is called before the first frame update
    void Start()
    {
        _50yardDrops9mm = new float[]
            {
                0f, 0f, 13.5f, 35f, 90f, 125f, 192f, 270f, 360f,460f,570f
            };
        for (int x = 0; x < _50yardDrops9mm.Length; x++) { _50yardDrops9mm[x] = Inches(_50yardDrops9mm[x]); }

        _50yardDrops40sw = new float[]
            {// 0 - 50 -100 -150 -200 -250 -300 - 350 - 400 - 450 - 500
                0f, 0f, 10f, 28f, 57f, 95f, 145f, 210f, 295f,400f,520f
            };
        for (int x = 0; x < _50yardDrops40sw.Length; x++) { _50yardDrops40sw[x] = Inches(_50yardDrops40sw[x]); }

        activeProjectiles.Add(new Projectile(transform.position, 343.2048f, transform.forward, ProjectileTypes._9mm));
        activeProjectiles.Add(new Projectile(transform.position, 327.3552f, transform.forward, ProjectileTypes._40sw));

    }
    public GameObject g;
    // Update is called once per frame
    void Update()
    {
        tat += Time.deltaTime;
        if (tat > 5f)
        {
            activeProjectiles.Add(new Projectile(transform.position, 343.2048f, transform.forward, ProjectileTypes._9mm));
            tat = 0f;
        }
        Debug.DrawRay(new Vector3(0f, 0f, 100f), transform.up * 5f, Color.blue, Time.deltaTime);
        Debug.DrawRay(new Vector3(0f, 0f, 200f), transform.up * 5f, Color.yellow, Time.deltaTime);
        Debug.DrawRay(new Vector3(0f, 0f, 300f), transform.up * 5f, Color.green, Time.deltaTime);
        foreach(Projectile p in activeProjectiles)
        {
            p.Tick();
            g.transform.position = p.currentPosition;
            velocity = p.velocity;
            Debug.Log(p.projectileTypes.ToString() +" | "+ p.currentPosition.ToString());
        }
    }

    public static float Inches(float m) { return m * 0.0254f; }

    
    
    public enum ProjectileTypes
    {
        _9mm,
        _40sw
    }
}

public class Projectile
{
    Vector3 startPosition;
    Vector3 direction;
    public Vector3 currentPosition;
    Vector3 lastPosition;
    public MuzzlePoint.ProjectileTypes projectileTypes;
    public float velocity;
    float dropVelocity = 0f;
    float lastDropVelocity = 0f;
    public Projectile(Vector3 startPosition, float v, Vector3 dir, MuzzlePoint.ProjectileTypes pt)
    {
        this.startPosition = startPosition;
        this.currentPosition = startPosition;
        this.velocity = v;
        this.direction = dir;
        this.projectileTypes = pt;
    }
    public void Tick()
    {
        float drop = 0f;

        lastPosition = currentPosition;
        currentPosition += (this.direction * velocity * Time.deltaTime);
        velocity -= (velocity * .085f * Time.deltaTime);

        float distance = Vector3.Distance(currentPosition, startPosition);

        if (projectileTypes.ToString() == "_9mm")
        {
            drop = MuzzlePoint._50yardDrops9mm[Mathf.Min(Mathf.RoundToInt(distance / 75f), MuzzlePoint._50yardDrops9mm.Length - 1)];
        }
        if (projectileTypes.ToString() == "_40sw")
        {
            drop = MuzzlePoint._50yardDrops40sw[Mathf.Min(Mathf.RoundToInt(distance / 75f), MuzzlePoint._50yardDrops40sw.Length - 1)];
        }
        distance = Vector3.Distance(currentPosition, lastPosition);
        currentPosition.y -= (distance / 50f) * drop;
        if (projectileTypes.ToString() == "_40sw")
        {
            Debug.DrawRay(lastPosition, currentPosition - lastPosition, Color.blue, 8f);
        }
        if (projectileTypes.ToString() == "_9mm")
        {
            Debug.DrawRay(lastPosition, currentPosition - lastPosition, Color.red, 8f);
        }
    }
}


