using System.Collections.Generic;
using Riptide;
using Runtime.Contexts.MainGame.Enum;
using Runtime.Contexts.MainGame.Model;
using Runtime.Contexts.MainGame.Vo;
using Runtime.Contexts.Network.Vo;
using strange.extensions.command.impl;

namespace Runtime.Contexts.MainGame.Processor
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