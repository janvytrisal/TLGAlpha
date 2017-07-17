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

/*
 * Make sure player is not hidden behind opaque objects. Using RaycastAll from camera to player position.
 * OPTIMISE - to not remove/add same object each fixed update
 */
public class ViewOnPlayer : MonoBehaviour 
{
    private List<GameObject> _transparentObjects;
    private float _transparency = 0.15f;
    private float _refreshTime = 0.2f;
    private float _elapsedTime;

    void Start()
    {
        _transparentObjects = new List<GameObject>();
        _elapsedTime = 0;
    }
    void FixedUpdate()
    {
        if (_elapsedTime >= _refreshTime)
        {
            RevertTransparency();
            MakeViewClear();
            _elapsedTime = 0;
        }
        else
        {
            _elapsedTime += Time.deltaTime;
        }
    }

    private void RevertTransparency()
    {
        for(int i = 0; i < _transparentObjects.Count; i++)
        {
            MakeOpaque(_transparentObjects[i]);
        }
        _transparentObjects.Clear();
    }
    private void MakeViewClear()
    {
        float playerDistance = (GameManager.Player.transform.position - Camera.main.transform.position).magnitude;
        Vector3 screenPlayerPosition = Camera.main.WorldToScreenPoint(GameManager.Player.transform.position);
        Ray screenToPlayerRay = Camera.main.ScreenPointToRay(screenPlayerPosition);
        RaycastHit[] hitInfos = Physics.RaycastAll(screenToPlayerRay, playerDistance, LayerMask.GetMask(new string[2]{ "Ground", "Surroundings" }));
        //Debug.DrawRay(screenToPlayerRay.origin, screenToPlayerRay.direction * playerDistance, Color.red, 1);

        foreach (RaycastHit hitInfo in hitInfos)
        {
            GameObject hitObject = hitInfo.collider.gameObject;
            MakeTransparent(hitObject);
            _transparentObjects.Add(hitObject);
        }
    }
    private void MakeOpaque(GameObject gameObject)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material.SetOverrideTag("RenderType", "");
        renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        renderer.material.SetInt("_ZWrite", 1);
        renderer.material.DisableKeyword("_ALPHABLEND_ON");
        renderer.material.renderQueue = -1;
        //maybe also set color transparency to 1f (not necessary because RenderMode.Opaque is not using alpha channel)
    }
    private void MakeTransparent(GameObject gameObject)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        //The generated color is multiplied by the SrcFactor. 
        //The color already on screen is multiplied by DstFactor and the two are added together.
        renderer.material.SetOverrideTag("RenderType", "Transparent");
        renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha); //calculated color
        renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha); //color already on the screen
        renderer.material.SetInt("_ZWrite", 0); //pixels written to depth buffer (off, on is required for opaque objects)
        renderer.material.EnableKeyword("_ALPHABLEND_ON");
        renderer.material.renderQueue = 3000;

        Color color = renderer.material.color;
        color.a = _transparency;
        renderer.material.color = color;
    }

}
