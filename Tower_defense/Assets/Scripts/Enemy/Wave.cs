using UnityEngine;

[CreateAssetMenu(fileName = "WaveScriptableObject", menuName = "ScriptableObjects/Wave")]
public class Wave : ScriptableObject
{
    public int spawnRate = 1;
    public Vector2Int[] waveComposition;
    public bool shuffleEnemies;

    public int[] GetWave()
    {
        if (!shuffleEnemies) return WaveInOrder();
        return ShuffledWave(); ;
    }

    private int[] WaveInOrder()
    {
        int enemyNumber = 0;
        int[] waveInOrder = new int[WaveSize()];
        for (int i = 0; i < waveComposition.Length; i++)
        {
            for (int j = 0; j < waveComposition[i][1]; j++)
            {
                waveInOrder[enemyNumber] = waveComposition[i][0];
                enemyNumber++;
            }
        }
        return waveInOrder;
    }
    public int WaveSize()
    {
        int size = 0;
        for (int i = 0; i < waveComposition.Length; i++)
        {
            size += waveComposition[i][1];
        }
        return size;
    }
    private int[] ShuffledWave()
    {
        int[] wave = WaveInOrder();
        Shuffle(wave);
        return wave;
    }

    private void Shuffle(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int random = Random.Range(0, array.Length);
            int temp = array[i];
            array[i] = array[random];
            array[random] = temp;
        }
    }
}
