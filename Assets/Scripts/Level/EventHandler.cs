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
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/*
 * Subscriber (Observer): handle game events
 * CHANGE -> find better approach
 */
public class EventHandler : MonoBehaviour 
{
    void OnEnable() //level start
    {
        LevelEnd.LevelEndReached += LevelEndReachedHandler;
        ProjectileHit.PlayerHit += PlayerHitHandler;
        FallChecker.PlayerFell += PlayerFellHandler;
    }
    void OnDisable() //level end
    {
        LevelEnd.LevelEndReached -= LevelEndReachedHandler;
        ProjectileHit.PlayerHit -= PlayerHitHandler;
        FallChecker.PlayerFell -= PlayerFellHandler;        
    }

    private void LevelEndReachedHandler()
    {
        int levelsTotal = (int)LevelSpecifications.levels;
        int currentLevel = GameManager.Seeds.Count;
        if (currentLevel < levelsTotal)
        {
            GameManager.State = ApplicationState.NextLevel;
            SceneManager.LoadScene("LevelInfinity", LoadSceneMode.Single); //reset everything except GameManager._instance
        }
        else
        {
            SceneManager.LoadScene("EndGame", LoadSceneMode.Single);
        }
    }
    private void PlayerHitHandler()
    {
        Immortality immortality = GameManager.Player.GetComponent<Immortality>();
        if (immortality.enabled)
        {
            return;
        }
        PlayerHealth playerHealth = GameManager.Player.GetComponent<PlayerHealth>();
        playerHealth.CurrentHealth--;
        if (playerHealth.CurrentHealth == 0)
        {
            Destroy(GameManager.Player);
            GameManager.DeathCount++;
            GameManager.SaveDeathCount();
            GetComponent<PlayerInstantiator>().InstantiatePlayer();
            immortality = GameManager.Player.GetComponent<Immortality>(); //because previous player was destroyed
        } 
        immortality.enabled = true;
    }
    private void PlayerFellHandler()
    {
        Destroy(GameManager.Player);
        GameManager.DeathCount++;
        GameManager.SaveDeathCount();
        GetComponent<PlayerInstantiator>().InstantiatePlayer();
    }
}
