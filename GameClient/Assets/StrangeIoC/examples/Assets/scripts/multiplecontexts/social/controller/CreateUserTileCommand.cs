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

/// CreateUserTileCommand
/// ============================
/// Creates the tile that represents the user

using StrangeIoC.examples.Assets.scripts.multiplecontexts.social.service;
using StrangeIoC.examples.Assets.scripts.multiplecontexts.social.view;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;

namespace StrangeIoC.examples.Assets.scripts.multiplecontexts.social.controller
{
  public class CreateUserTileCommand : EventCommand
  {
    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextView { get; set; }

    public override void Execute()
    {
      var vo = evt.data as UserVO;

      var go = Object.Instantiate(Resources.Load("GameTile")) as GameObject;
      go.transform.parent = contextView.transform;
      go.AddComponent<UserTileView>();

      //Here's something interesting. I'm technically bypassing the mediator here.
      //Stylistically I think this is fine during instantiation. Your team might decide differently.
      var view = go.GetComponent<UserTileView>();
      view.setUser(vo);

      var bottomLeft = new Vector3(.1f, .1f, (Camera.main.farClipPlane - Camera.main.nearClipPlane) / 2f);
      var dest = Camera.main.ViewportToWorldPoint(bottomLeft);
      view.SetTilePosition(dest);
    }
  }
}