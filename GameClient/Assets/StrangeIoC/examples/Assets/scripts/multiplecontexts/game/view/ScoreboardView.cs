/*
 * Copyright 2013 ThirdMotion, Inc.
 *
 *	Licensed under the Apache License, Version 2.0 (the "License");
 *	you may not use this file except in compliance with the License.
 *	You may obtain a copy of the License at
 *
 *		http://www.apache.org/licenses/LICENSE-2.0
 *
 *		Unless required by applicable law or agreed to in writing, software
 *		distributed under the License is distributed on an "AS IS" BASIS,
 *		WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *		See the License for the specific language governing permissions and
 *		limitations under the License.
 */

using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace StrangeIoC.examples.Assets.scripts.multiplecontexts.game.view
{
  public class ScoreboardView : EventView
  {
    internal const string REPLAY = "REPLAY";
    internal const string REMOVE_CONTEXT = "REMOVE_CONTEXT";

    private Vector3 basePosition;
    private bool gameOn;
    private string livesString;
    private Rect removeContextRect;
    private Rect replayRect;
    private Rect scoreRect;
    private string scoreString;

    private void OnGUI()
    {
      GUI.TextField(scoreRect, scoreString + "\n" + livesString);

      if (!gameOn)
      {
        if (GUI.Button(replayRect, "Replay")) dispatcher.Dispatch(REPLAY);

        if (GUI.Button(removeContextRect, "Remove Social Context")) dispatcher.Dispatch(REMOVE_CONTEXT);
      }
    }

    internal void init(string score, string lives)
    {
      scoreString = score;
      scoreRect = new Rect(5f, 5f, 120f, 50f);
      replayRect = new Rect(Screen.width / 2f, Screen.height / 2f, 100f, 100f);
      removeContextRect = new Rect(Screen.width - 200, Screen.height - 20f, 200f, 20f);
      livesString = lives;
      gameOn = true;
    }

    internal void updateScore(string value)
    {
      scoreString = value;
    }

    internal void updateLives(string value)
    {
      livesString = value;
    }

    internal void gameOver()
    {
      gameOn = false;
    }
  }
}