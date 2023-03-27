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
  public class EnemyView : EventView
  {
    internal const string CLICK_EVENT = "CLICK_EVENT";

    //Publicly settable from Unity3D
    public float edx_WobbleForce = .4f;
    public float edx_WobbleIncrement = .1f;
    private Vector3 basePosition;

    private float theta;

    internal void init()
    {
      gameObject.AddComponent<ClickDetector>();
      var clicker = gameObject.GetComponent<ClickDetector>();
      clicker.dispatcher.AddListener(ClickDetector.CLICK, onClick);
    }

    internal void updatePosition()
    {
      wobble();
    }

    private void onClick()
    {
      dispatcher.Dispatch(CLICK_EVENT);
    }

    private void wobble()
    {
      theta += edx_WobbleIncrement;
      gameObject.transform.Rotate(Vector3.forward, edx_WobbleForce * Mathf.Sin(theta));
    }
  }
}