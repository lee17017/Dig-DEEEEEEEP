using System.Collections;
using UnityEngine;

/// <summary> This class spawns random obstacles on the lanes and displays the warning signs. </summary>
public class Spawner : MonoBehaviour
{
    #region Variables
    private Player player;
    
    [SerializeField]
    private GameObject[] obstacles;
    private Transform[] spawner = new Transform[3];
    [SerializeField]
    private SpriteRenderer warningMiddle, warningLeft, warningRight;

    private float spawnTimeCur;
    private float xStart;
    [SerializeField]
    private float spawnTime;
    #endregion

    private void Start()
    {
        player = transform.parent.GetComponent<Player>();
        spawner[0] = transform.GetChild(0);
        spawner[1] = transform.GetChild(1);
        spawner[2] = transform.GetChild(2);

        spawnTimeCur = spawnTime;
        xStart = transform.position.x;
    }

    void Update()
    {
        if (!player.obstacle && GameManager.current.run)
        {
            transform.position = new Vector3(xStart, transform.position.y, transform.position.z);

            if (spawnTimeCur < 0)
            {
                spawnTimeCur = Rand(1, 4);
                StartCoroutine(SpawnObject(Rand(spawner.Length - 1), Rand(obstacles.Length - 1)));
            }

            spawnTimeCur -= Time.deltaTime;
        }
    }

    /// <summary> Returns an int between 0 and given maximum (both inclusive). </summary>
    /// <param name="max"> The maximum </param>
    /// <returns> int between 0 and max </returns>
    private int Rand(int max)
    {
        return Random.Range(0, max + 1);
    }

    /// <summary> Returns an int between given minimum and maximum (both inclusive). </summary>
    /// <param name="min"> The minimum </param>
    /// <param name="max"> The maximum </param>
    /// <returns> int between min and max </returns>
    private int Rand(int min, int max)
    {
        return Random.Range(min, max + 1);
    }

    /// <summary> Spawns a given ostacle at a given lane. </summary>
    /// <param name="spawn"> The lane index </param>
    /// <param name="obst"> The obstacle index </param>
    IEnumerator SpawnObject(int spawn, int obst)
    {
        StartCoroutine(Warning(spawn));
        GameObject obstacle = Instantiate(obstacles[obst]);
        obstacle.transform.position = spawner[spawn].transform.position;
        yield return null;
    }

    /// <summary> Displays a warning sign on a given lane for 1 second. </summary>
    /// <param name="spawn"> The lane index </param>
    IEnumerator Warning(int spawn)
    {
        SpriteRenderer warningObj = null;

        switch (spawn)
        {
            case 0:
                warningObj = warningMiddle;
                break;
            case 1:
                warningObj = warningLeft;
                break;
            case 2:
                warningObj = warningRight;
                break;
        }
        
        warningObj.color = new Color(1, 1, 1, 1);

        yield return new WaitForSeconds(1);

        warningObj.color = new Color(1, 1, 1, 0);
    }
}