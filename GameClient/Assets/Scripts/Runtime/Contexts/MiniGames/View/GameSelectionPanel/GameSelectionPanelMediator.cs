using Runtime.Contexts.MiniGames.Enum;
using Runtime.Contexts.MiniGames.Vo;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.Contexts.MiniGames.View.GameSelectionPanel
{
    public class GameSelectionPanelMediator : EventMediator
    {
        [Inject] public GameSelectionPanelView view { get; set; }
        
        public override void OnRegister()
        {
            base.OnRegister();
            dispatcher.AddListener(MiniGamesEvent.OnCreateMiniGame,OnMiniGameSelected);
        }

        private void OnMiniGameSelected(IEvent payload)
        {
            MiniGameCreatedVo vo = (MiniGameCreatedVo)payload.data;
            Destroy(gameObject);
        }

        public override void OnRemove()
        {
            base.OnRemove();
            dispatcher.RemoveListener(MiniGamesEvent.OnCreateMiniGame,OnMiniGameSelected);

        }
    }
    
}