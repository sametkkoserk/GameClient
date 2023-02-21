using System.Collections.Generic;
using Riptide;
using Runtime.MainGame.Enum;
using Runtime.MainGame.Model;
using Runtime.MainGame.Vo;
using Runtime.Network.Vo;
using strange.extensions.command.impl;
using UnityEngine;

namespace Runtime.MainGame.Processor
{
    public class HandleMapGeneratorProcessor : EventCommand
    {
        [Inject]
        public IMainGameModel mainGameModel { get; set; }
        public override void Execute()
        {
            MessageReceivedVo vo = (MessageReceivedVo) evt.data;

            Message message = vo.message;
            
            Dictionary<int, CityVo> cityVos = new();

            int cityCount = message.GetInt();

            for (int i = 0; i < cityCount; i++)
            {
                CityVo cityVo = new()
                {
                    ID = message.GetInt(),
                    isPlayable = message.GetBool(),
                    soldierCount = message.GetInt(),
                    position = message.GetVector3(),
                    ownerID = message.GetInt()
                };
                
                cityVos.Add(cityVo.ID, cityVo);
            }
            
            mainGameModel.cities = cityVos;
            
            dispatcher.Dispatch(MainGameEvent.StartGame);
        }
    }
}