using System.Collections.Generic;
using MainGame.Enum;
using MainGame.Vo;
using Network.Vo;
using Riptide;
using strange.extensions.command.impl;
using UnityEngine;

namespace MainGame.Processor
{
    public class HandleMapGeneratorProcessor : EventCommand
    {
        public override void Execute()
        {
            MessageReceivedVo vo = (MessageReceivedVo) evt.data;

            Message message = vo.message;
            
            Dictionary<int, CityVo> cityVos = new Dictionary<int, CityVo>();

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
            
            dispatcher.Dispatch(MainGameEvent.MapGenerator, cityVos);
            Debug.Log("Pro");
        }
    }
}