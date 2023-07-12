using System;
using Runtime.Contexts.Main.Enum;
using Runtime.Contexts.Main.Vo;
using Runtime.Modules.Core.Localization.Enum;
using Runtime.Modules.Core.Localization.Model.LocalizationModel;
using StrangeIoC.scripts.strange.extensions.dispatcher.eventdispatcher.api;
using StrangeIoC.scripts.strange.extensions.injector;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using UnityEngine;

namespace Runtime.Contexts.Main.View.Tooltip
{
  public class TooltipMediator : EventMediator
  {
    [Inject]
    public TooltipView view { get; set; }
    
    [Inject]
    public ILocalizationModel localizationModel { get; set; }

    public override void OnRegister()
    {
      dispatcher.AddListener(TooltipEvent.Show, OnShow);
      dispatcher.AddListener(TooltipEvent.Hide, OnHide);
      
      gameObject.SetActive(false);
    }

    private void OnShow(IEvent payload)
    {
      if (string.IsNullOrEmpty(view.headerField.text) || string.IsNullOrEmpty(view.contentField.text))
        return;
      
      TooltipInfoVo tooltipInfoVo = (TooltipInfoVo)payload.data;

      if (string.IsNullOrEmpty(tooltipInfoVo.headerKey.ToString()))
        view.headerField.gameObject.SetActive(false);
      if (string.IsNullOrEmpty(tooltipInfoVo.contentKey.ToString()))
        view.contentField.gameObject.SetActive(false);
      
      view.headerField.text = localizationModel.GetText(TableKey.Tooltip, tooltipInfoVo.headerKey);
      view.contentField.text = localizationModel.GetText(TableKey.Tooltip, tooltipInfoVo.contentKey);
      
      gameObject.SetActive(true);
    }

    private void OnHide()
    {
      gameObject.SetActive(false);
    }
    
    private void Update()
    {
      view.layoutElement.enabled = Math.Max(view.headerField.preferredWidth, view.contentField.preferredWidth) >= view.layoutElement.preferredWidth;

      // Vector2 position = Input.mousePosition;
      //
      // transform.position = position;
      
      Vector2 position = Input.mousePosition;
      float x = position.x / Screen.width;
      float y = position.y / Screen.height;

      if (x <= y && x <= 1 - y) //left
        view.rectTransform.pivot = new Vector2(view.v1, y);
      else if (x >= y && x <= 1 - y) //bottom
        view.rectTransform.pivot = new Vector2(x, -view.v2);
      else if (x >= y && x >= 1 - y) //right
        view.rectTransform.pivot = new Vector2(view.v3, y);
      else if (x <= y && x >= 1 - y) //top
        view.rectTransform.pivot = new Vector2(x, view.v4);
      transform.position = position;
    }

    public override void OnRemove()
    {
      dispatcher.RemoveListener(TooltipEvent.Show, OnShow);
      dispatcher.RemoveListener(TooltipEvent.Hide, OnHide);
    }
  }
}