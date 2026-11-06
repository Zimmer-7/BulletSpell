using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Enemies/Database")]
public class EnemyDatabase : ScriptableObject
{
    public List<GameObject> enemies;

    public Enemy GetEnemyByIndex(int index)
    {
        if (index >= 0 && index < enemies.Count)
        {
            return enemies[index].GetComponent<Enemy>();
        }
        else
        {
            Debug.LogError("Index out of range in EnemyDatabase");
            return null;
        }
    }

    public void Pop(int index)
    {
        if (index >= 0 && index < enemies.Count)
        {
            enemies.RemoveAt(index);
        }
        else
        {
            Debug.LogError("Index out of range in EnemyDatabase");
        }
    }

    public void Push(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public EnemyDatabase Clone()
    {
        EnemyDatabase clone = ScriptableObject.CreateInstance<EnemyDatabase>();
        clone.enemies = new List<GameObject>(this.enemies);
        return clone;
    }
}
