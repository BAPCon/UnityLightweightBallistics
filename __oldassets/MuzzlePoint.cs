using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzlePoint : MonoBehaviour
{
    public float velocity;
    public float tat;
    public static float[] _50yardDrops9mm;
    public static float[] _50yardDrops40sw;
    public static float[] _50yardDrops22lr;
    public static float[] _50yardDrops223nato;
    public static float[] _50yardDrops762rus;
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

        _50yardDrops22lr = new float[]
            {// 0 - 50 -100 -150 -200 -250 -300 - 350 - 400 - 450 - 500
                0f, 0f, 7.4f, 22.8f, 45.9f, 86f, 126f, 224f, 325f,450f,600f
            };
        for (int x = 0; x < _50yardDrops22lr.Length; x++) { _50yardDrops22lr[x] = Inches(_50yardDrops22lr[x]); }

        _50yardDrops223nato = new float[]
            {// 0 - 50 -100 -150 -200 -250 -300 - 350 - 400 - 450 - 500 - 550 - 600 - 650f - 700f - 750f - 800f
                0f, 0f, 0f,  0f,  0f,  0f,  0f,   0f,   10f,  40f,  55f,  70f,  90f,  120f,  135f,  180f,  300f, 360f, 400f, 525f, 630f
            };
        for (int x = 0; x < _50yardDrops223nato.Length; x++) { _50yardDrops223nato[x] = Inches(_50yardDrops223nato[x]); }

        _50yardDrops762rus = new float[]
            {// 0 - 50 -100 -150 -200 -250 -300 - 350 - 400 - 450 - 500 - 550 - 600 - 650f - 700f - 750f - 800f
                0f, 0f, 0f,  0f,  6f,  13f, 30f,  40f,  60f,  80f,  115f, 155f,  215f,  292f,  380f,  480f,  590f, 720f, 840f, 1005f
            };
        for (int x = 0; x < _50yardDrops762rus.Length; x++) { _50yardDrops762rus[x] = Inches(_50yardDrops762rus[x]); }
    }
    public GameObject g;
    public ProjectileTypes projectileTypeTest;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            activeProjectiles.Add(new Projectile(transform.position, velocities[(int)projectileTypeTest], transform.forward, projectileTypeTest));
        }
        foreach(Projectile p in activeProjectiles)
        {
            p.Tick();
            p.DrawRay();
        }
    }

    public static float Inches(float m) { return m * 0.0254f; }

    
    
    public enum ProjectileTypes
    {
        _9mm,
        _40sw,
        _22lr,
        _223nato,
        _762rus
    }
    public float[] velocities = new float[] { 343.2048f, 327.3552f, 323.3928f, 959.5104f, 720.2424f };
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
    public void DrawRay()
    {
        Debug.DrawRay(currentPosition, lastPosition - currentPosition, Color.cyan, Time.deltaTime*6f);   
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
        if (projectileTypes.ToString() == "_22lr")
        {
            drop = MuzzlePoint._50yardDrops22lr[Mathf.Min(Mathf.RoundToInt(distance / 75f), MuzzlePoint._50yardDrops22lr.Length - 1)];
        }
        if (projectileTypes.ToString() == "_223nato")
        {
            drop = MuzzlePoint._50yardDrops223nato[Mathf.Min(Mathf.RoundToInt(distance / 75f), MuzzlePoint._50yardDrops223nato.Length - 1)];
        }
        if (projectileTypes.ToString() == "_762rus")
        {
            drop = MuzzlePoint._50yardDrops762rus[Mathf.Min(Mathf.RoundToInt(distance / 75f), MuzzlePoint._50yardDrops762rus.Length - 1)];
        }

        distance = Vector3.Distance(currentPosition, lastPosition);
        currentPosition.y -= (distance / 50f) * drop;
    }
}


