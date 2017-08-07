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

/*
 * Process touches when player is touching left hand panel.
 */
public class Move : MonoBehaviour
{
    private bool _touch;
    private Rect _allowedArea;

    void Awake()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 size = Vector2.Scale(rectTransform.rect.size, rectTransform.lossyScale); //world space conversion (screen size)
        _allowedArea = new Rect((Vector2)rectTransform.position - (size * 0.5f), size);
    }
    void Update()
    {
        if (_touch)
        {
            if (GameManager.Player != null)
            {
                #if UNITY_ANDROID 
                GameManager.Player.GetComponent<PlayerMotions>().ProcessTouches(_allowedArea);
                #endif
            }
        }
    }

    public void RegisterTouchOnPointerDown()
    {
        _touch = true;
    }
    public void UnregisterTouchOnPointerUp()
    {
        _touch = false;
    }
}

