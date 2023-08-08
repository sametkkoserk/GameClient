using DG.Tweening;
using UnityEngine;

namespace Runtime.Modules.Core.Icon.View.LoadingIcon
{
  [RequireComponent(typeof(RectTransform))]
  public class LoadingIconBehaviour : MonoBehaviour
  {
    [HideInInspector]
    public RectTransform loadingIcon;

    public Tween rotationAnimation;

    public void Awake()
    {
      loadingIcon = gameObject.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
      rotationAnimation = loadingIcon.DORotate(new Vector3(0f, 0f, -1440f), 8f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart);
    }

    private void OnDisable()
    {
      rotationAnimation.Kill();
    }
  }
}