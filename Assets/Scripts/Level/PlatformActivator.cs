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
using System.Collections;
using System.Collections.Generic;
using UnityUtilityLibrary;

/*
 * Activate all platforms in convenient time. When moving platform A needs
 * to be activated and platform B before this one is also moving platform, 
 * activator will wait until platform B is as close as possible,
 * then activate platform A.
 */
public class PlatformActivator : MonoBehaviour 
{
    private bool _activationStarted;

    /*void Update()
    {
        if (!_activationStarted)
        {
            
            _activationStarted = true;
        }
    }*/

    public void ActivatePlatforms()
    {
        StartCoroutine(ActivatePlatforms(GameManager.Platforms));
    }

    private IEnumerator ActivatePlatforms(List<GameObject> platforms)
    {
        for(int i = 1; i < platforms.Count; i++)
        {
            PlatformMotion platformMotion = platforms[i].GetComponent<PlatformMotion>();
            if (platformMotion != null)
            {
                PlatformMotion previousPlatformMotion = platforms[i - 1].GetComponent<PlatformMotion>();
                if (previousPlatformMotion != null)
                {
                    //wait until previous platform reaches its destination
                    while (true)
                    {
                        if(previousPlatformMotion.DestinationReached)
                        {
                            break;
                        }
                        yield return null;
                    }
                }
                platformMotion.enabled = true;
            }
        }
    }
}
