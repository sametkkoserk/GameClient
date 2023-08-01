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
      // rotationAnimation = loadingIcon.DORotate(new Vector3(0f, 0f, -360f), 1f, RotateMode.Fast).SetLoops(-1);

      // rotationAnimation.Play();
    }

    private void OnDisable()
    {
      rotationAnimation.Kill();
    }
  }
}