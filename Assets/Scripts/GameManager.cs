using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

/*
 * Singleton
 * Store game data. Have methods for loading and saving game.
 */

public enum ApplicationState { Started = 0, NewGame, ExitLevel, Continue, NextLevel, EndGame };

public class GameManager : MonoBehaviour 
{
    private static GameManager _instance;
    private GameObject _player; 
    private List<int> _seeds;
    private List<GameObject> _platforms;
    private List<GameObject[]> _placeholders;
    private ApplicationState _state;
	private int _deathCount;
    private int _highscore = int.MaxValue;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject gameManager = new GameObject();
                _instance = gameManager.AddComponent<GameManager>();
                DontDestroyOnLoad(gameManager);
            }
            return _instance;            
        }
    }
    public static GameObject Player
    {
        get
        {
            return Instance._player; //shortcut
        }
        set
        {
            Instance._player = value;
        }
    }
    public static List<int> Seeds
    {
        get
        {
            if (Instance._seeds == null)
            {
                Instance._seeds = new List<int>();
            }
            return Instance._seeds;
        }
        set
        {
            Instance._seeds = value;
        }
    }
    public static List<GameObject> Platforms
    {
        get
        {
            return Instance._platforms;
        }
        set
        {
            Instance._platforms = value;
        }
    }
    public static List<GameObject[]> Placeholders
    {
        get
        {
            return Instance._placeholders;
        }
        set
        {
            Instance._placeholders = value;
        }
    }
    public static ApplicationState State
    {
        get
        {
            return Instance._state;
        }
        set
        {
            Instance._state = value;
        }
    }
	public static int DeathCount
	{
		get
		{
            return Instance._deathCount;
		}
		set 
		{
            Instance._deathCount = value;
		}
	}
    public static int Highscore
    {
        get
        {
            return Instance._highscore;
        }
        set
        {
            Instance._highscore = value;
        }
    }

    private GameManager()
    {
    }


    /*
     * Save format:
     * DeathCount|Highscore|Seeds.Count|Seeds[0]|Seeds[1]| ... |Seeds[Seeds.Count - 1]
     * 4 byte each
     */
    public static void SaveAll()
    {
        string path = Application.persistentDataPath + "/gamedata.dat"; //require "Write Access: External", cuz can lead to Internal_SD instead of Internal Storage

        FileStream fileStream = File.Create(path);
        BinaryWriter binaryWriter = new BinaryWriter(fileStream); //UTF8 default

        binaryWriter.Write(DeathCount);
        binaryWriter.Write(Highscore);
        binaryWriter.Write(Seeds.Count); //to easier read all seeds
        foreach (int seed in Seeds)
        {
            binaryWriter.Write(seed);
        }
        binaryWriter.Close();
        Debug.Log("All saved");
    }
    public static void SaveDeathCount()
    {
        string path = Application.persistentDataPath + "/gamedata.dat"; 

        FileStream fileStream = File.Open(path, FileMode.OpenOrCreate);
        BinaryWriter binaryWriter = new BinaryWriter(fileStream); //UTF8 default

        binaryWriter.Write(DeathCount);
        binaryWriter.Close();
        Debug.Log("DeathCount saved");        
    }
    public static void SaveNewSeed()
    {
        string path = Application.persistentDataPath + "/gamedata.dat"; 

        FileStream fileStream = File.Open(path, FileMode.OpenOrCreate);
        BinaryWriter binaryWriter = new BinaryWriter(fileStream); //UTF8 default

        binaryWriter.Seek(8, SeekOrigin.Begin);
        binaryWriter.Write(Seeds.Count);
        binaryWriter.Seek(4 * (Seeds.Count - 1), SeekOrigin.Current); //mby just jump to the end of file
        binaryWriter.Write(Seeds[Seeds.Count - 1]);
        binaryWriter.Close(); 
        Debug.Log("New seed saved");
    }
    public static void LoadAll()
    {
        string path = Application.persistentDataPath + "/gamedata.dat"; 

        if (File.Exists(path))
        {
            FileStream fileStream = File.Open(path, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(fileStream); //UTF8 default

            DeathCount = binaryReader.ReadInt32();
            Highscore = binaryReader.ReadInt32();
            int seedCount = binaryReader.ReadInt32();
            for (int i = 0; i < seedCount; i++)
            {
                Seeds.Add(binaryReader.ReadInt32());
            }
            binaryReader.Close();
            Debug.Log("All loaded");
        }
    }
}