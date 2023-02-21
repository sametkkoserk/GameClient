using System.Collections.Generic;
using Riptide;
using Runtime.MainGame.Enum;
using Runtime.MainGame.Model;
using Runtime.MainGame.Vo;
using Runtime.Network.Services.NetworkManager;
using Runtime.Network.Vo;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.MainGame.Processor
{
    public class HandleMapGeneratorProcessor : EventCommand
    {
        [Inject]
        public IMainGameModel mainGameModel { get; set; }
        
        [Inject] 
        public INetworkManagerService networkManager { get; set; }
        public override void Execute()
        {
            MessageReceivedVo vo = (MessageReceivedVo) evt.data;
            string message = vo.message;
            MapGeneratorVo mapGeneratorVo = networkManager.GetData<MapGeneratorVo>(message);

            mainGameModel.cities = mapGeneratorVo.cityVos;
            
            dispatcher.Dispatch(MainGameEvent.StartGame);
        }
    }
}