using Runtime.Contexts.MiniGames.Enum;
using Runtime.Contexts.MiniGames.View.MiniGame;
using Runtime.Contexts.MiniGames.Vo;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.Contexts.MiniGames.View.MiniGameContainer
{
    public class MiniGameContainerMediator : EventMediator
    {
        [Inject] 
        public MiniGameContainerView view { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();
            
            dispatcher.AddListener(MiniGamesEvent.OnCreateMiniGame,OnCreateMiniGame);
        }

        private void OnCreateMiniGame(IEvent payload)
        {
            MiniGameCreatedVo vo = (MiniGameCreatedVo)payload.data;
            Addressables.InstantiateAsync(vo.miniGameKey, transform);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            
            dispatcher.RemoveListener(MiniGamesEvent.OnCreateMiniGame,OnCreateMiniGame);

        }
    }
}