// Copyright (C) 2015-2021 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the Asset Store EULA is available at http://unity3d.com/company/legal/as_terms.

using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public string scene = "<Insert scene name>";
    public float duration = 1.0f;
    public Color color = Color.black;

    public void PerformTransition()
    {
        SoundManager.instance.PlayUiButtonSound();
        //SoundManager.instance.PlayAudio(AudioType.ButtonPressed);
        Ricimi.Transition.LoadLevel(scene, duration, color);
    }
}