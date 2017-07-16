using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Creates level. Depends on ApplicationState and GameManager data.
 * Nested coroutines are used to let GeneratePlatforms have more 
 * computational time.
 */
public class InitializeLevel : MonoBehaviour 
{
    private LevelSpecifications _levelSpecifications;

    void Awake()
    {
        _levelSpecifications = GetComponent<LevelSpecifications>();
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        InitializeDeathCounter();
        InitializeRandomNumberGenerator();
        yield return StartCoroutine(GeneratePlatforms()); 
        GenerateSurroundings();
        GenerateEnemies();
        DestroyPlaceholders();
        ActivatePlatforms();
        SaveGame();
        InstantiatePlayer();
        AdjustHUD();
    }
    private void InitializeDeathCounter()
    {
        if (GameManager.State == ApplicationState.NewGame)
        {
            GameManager.DeathCount = 0;
        }
    }
    private void InitializeRandomNumberGenerator()
    {
        int seed;
        if (GameManager.State == ApplicationState.NewGame)
        {
            GameManager.Seeds = new List<int>();
            seed = ChoseSeed(int.MinValue, int.MaxValue);
            GameManager.Seeds.Add(seed);
        }
        else if (GameManager.State == ApplicationState.Continue)
        {
            int totalSeeds = GameManager.Seeds.Count;
            seed = GameManager.Seeds[totalSeeds - 1]; //level to load
        }
        else //next level
        {
            seed = ChoseSeed(int.MinValue, int.MaxValue);
            GameManager.Seeds.Add(seed);
        }
        Random.InitState(seed);
    }
    private IEnumerator GeneratePlatforms()
    {
        float staticCount;
        float movingCount;
        if (GameManager.State == ApplicationState.NewGame)
        {
            _levelSpecifications.GetPlatformCounts(0, out staticCount, out movingCount);
        }
        else if (GameManager.State == ApplicationState.Continue)
        {
            int totalSeeds = GameManager.Seeds.Count;
            _levelSpecifications.GetPlatformCounts(totalSeeds - 1, out staticCount, out movingCount);
        }
        else //next level
        {
            int totalSeeds = GameManager.Seeds.Count;
            _levelSpecifications.GetPlatformCounts(totalSeeds, out staticCount, out movingCount);
        }
        yield return StartCoroutine(GetComponent<PlatformGenerator>().GeneratePlatforms((int)staticCount, (int)movingCount));
    }
    private void GenerateSurroundings()
    {
        GetComponent<SurroundingsGenerator>().GenerateSurroundings(GameManager.Placeholders);
    }
    private void GenerateEnemies()
    {
        int totalSeeds = GameManager.Seeds.Count;
        int lastSeed = GameManager.Seeds[totalSeeds - 1];

        int enemyCount = (int)_levelSpecifications.GetEnemyCount(totalSeeds - 1);
        float fireFrequency = _levelSpecifications.GetFireFrequency(totalSeeds - 1);

        GetComponent<EnemyGenerator>().GenerateEnemies(GameManager.Placeholders, enemyCount, fireFrequency);
    }
    private void DestroyPlaceholders()
    {
        GetComponent<PlatformGenerator>().DestroyAllPlaceholders();
        //GetComponent<PlatformGenerator>().DeactiveAllPlaceholders();
    }
    private void ActivatePlatforms()
    {
        GetComponent<PlatformActivator>().ActivatePlatforms();        
    }
    private void InstantiatePlayer()
    {
        GetComponent<PlayerInstantiator>().InstantiatePlayer();        
    }
    private void SaveGame()
    {
        if (GameManager.State == ApplicationState.NewGame)
        {
            GameManager.SaveAll();
        }
        else if (GameManager.State == ApplicationState.NextLevel)
        {
            GameManager.SaveNewSeed();
        }
    }
    // Returns 0 as default seed if all seeds were used.
    private int ChoseSeed(int min, int max)
    {
        List<int> seeds = GameManager.Seeds;
        Random.InitState((int) System.DateTime.Now.TimeOfDay.TotalMilliseconds); //to make sure new seed will be chosen at random
        int chosenSeed;
        int maxLevelCounter = 0;
        do
        {
            chosenSeed = Random.Range(min, max);
            maxLevelCounter++;
        }
        while(seeds.Contains(chosenSeed) && (maxLevelCounter < (-min + max)));
        return chosenSeed;
    }
    //replace later
    private void AdjustHUD()
    {
        Transform topPanelHUD = GameObject.Find("HUDCanvas/TopPanel").transform;
        topPanelHUD.Find("HealthBar").gameObject.SetActive(true);
        topPanelHUD.Find("PauseButton").gameObject.SetActive(true);
        GameObject.Find("LoadingPanel").SetActive(false);
    }
}
