/*
 * TLG Alpha
 * Copyright (C) 2017 Jan Vytrisal
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, version 3 of the License only.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>
 */

using UnityEngine;
using System.Collections.Generic;
using System.IO;

/*
 * Singleton
 * Store game data. Has methods for loading and saving game.
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
        string path = Application.persistentDataPath + "/gamedata.dat"; //require "Write Access: External", can lead to Internal_SD instead of Internal Storage

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
    }
    public static void SaveDeathCount()
    {
        string path = Application.persistentDataPath + "/gamedata.dat"; 

        FileStream fileStream = File.Open(path, FileMode.OpenOrCreate);
        BinaryWriter binaryWriter = new BinaryWriter(fileStream); 

        binaryWriter.Write(DeathCount);
        binaryWriter.Close();       
    }
    public static void SaveNewSeed()
    {
        string path = Application.persistentDataPath + "/gamedata.dat"; 

        FileStream fileStream = File.Open(path, FileMode.OpenOrCreate);
        BinaryWriter binaryWriter = new BinaryWriter(fileStream); 

        binaryWriter.Seek(8, SeekOrigin.Begin);
        binaryWriter.Write(Seeds.Count);
        binaryWriter.Seek(4 * (Seeds.Count - 1), SeekOrigin.Current); 
        binaryWriter.Write(Seeds[Seeds.Count - 1]);
        binaryWriter.Close(); 
    }
    public static void LoadAll()
    {
        string path = Application.persistentDataPath + "/gamedata.dat"; 

        if (File.Exists(path))
        {
            FileStream fileStream = File.Open(path, FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(fileStream); 

            DeathCount = binaryReader.ReadInt32();
            Highscore = binaryReader.ReadInt32();
            int seedCount = binaryReader.ReadInt32();
            for (int i = 0; i < seedCount; i++)
            {
                Seeds.Add(binaryReader.ReadInt32());
            }
            binaryReader.Close();
        }
    }
}