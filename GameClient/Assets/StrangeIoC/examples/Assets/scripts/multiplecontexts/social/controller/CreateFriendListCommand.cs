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

/// CreateFriendListCommand
/// ============================
/// Creates the tile that represents the user's friends and their scores
/// Compares user score to friend's scores

using System;
using System.Collections;
using StrangeIoC.examples.Assets.scripts.multiplecontexts.social.service;
using StrangeIoC.examples.Assets.scripts.multiplecontexts.social.view;
using StrangeIoC.scripts.strange.extensions.command.impl;
using StrangeIoC.scripts.strange.extensions.context.api;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace StrangeIoC.examples.Assets.scripts.multiplecontexts.social.controller
{
  public class CreateFriendListCommand : Command
  {
    [Inject(ContextKeys.CONTEXT_VIEW)]
    public GameObject contextView { get; set; }

    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher dispatcher { get; set; }

    //Remember back in StartCommand when I said we'd need the userVO again?
    [Inject]
    public UserVO userVO { get; set; }

    public override void Execute()
    {
      var list = data as ArrayList;

      var highScore = 0;
      var aa = list.Count;
      for (var a = 0; a < aa; a++)
      {
        var vo = list[a] as UserVO;

        var go = Object.Instantiate(Resources.Load("GameTile")) as GameObject;
        go.AddComponent<UserTileView>();
        go.transform.parent = contextView.transform;
        var view = go.GetComponent<UserTileView>();
        view.setUser(vo);

        var pos = new Vector3(.2f + .1f * a, .1f, (Camera.main.farClipPlane - Camera.main.nearClipPlane) / 2f);
        var dest = Camera.main.ViewportToWorldPoint(pos);
        view.SetTilePosition(dest);

        highScore = Math.Max(highScore, vo.highScore);
      }

      string msg;
      if (userVO.currentScore > highScore)
        msg = "Score of " + userVO.currentScore + " is the new High Score!!!";
      else if (userVO.currentScore > userVO.highScore)
        msg = "Score of " + userVO.currentScore + " is a personal best!";
      else
        msg = "Score of " + userVO.currentScore + " is nothing special.";

      var award = new GameObject();
      award.transform.parent = contextView.transform;
      award.AddComponent<AwardView>();

      dispatcher.Dispatch(SocialEvent.REWARD_TEXT, msg);
    }
  }
}