/// An example view
/// ==========================
/// 

using System.Collections;
using StrangeIoC.scripts.strange.extensions.mediation.impl;
using StrangeIoC.scripts.strange.extensions.signal.impl;
using UnityEngine;

namespace StrangeIoC.examples.Assets.scripts.signalsproject.view
{
  public class ExampleView : View
  {
    //Publicly settable from Unity3D
    public float edx_WobbleSize = 1f;
    public float edx_WobbleDampen = .9f;
    public float edx_WobbleMin = .001f;
    private Vector3 basePosition;
    public Signal clickSignal = new();

    private GameObject latestGO;

    private readonly float theta = 20f;

    internal string currentText
    {
      get
      {
        var go = latestGO;
        var textMesh = go.GetComponent<TextMesh>();
        return textMesh.text;
      }
    }

    private void Update()
    {
      transform.Rotate(Vector3.up * Time.deltaTime * theta, Space.Self);
    }

    internal void init()
    {
      latestGO = Instantiate(Resources.Load("Textfield")) as GameObject;
      var go = latestGO;
      go.name = "first";

      var textMesh = go.GetComponent<TextMesh>();
      textMesh.text = "http://www.thirdmotion.com";
      textMesh.font.material.color = Color.red;

      var localPosition = go.transform.localPosition;
      localPosition.x -= go.GetComponent<Renderer>().bounds.extents.x;
      localPosition.y += go.GetComponent<Renderer>().bounds.extents.y;
      go.transform.localPosition = localPosition;

      var extents = Vector3.zero;
      extents.x = go.GetComponent<Renderer>().bounds.size.x;
      extents.y = go.GetComponent<Renderer>().bounds.size.y;
      extents.z = go.GetComponent<Renderer>().bounds.size.z;
      (go.GetComponent<Collider>() as BoxCollider).size = extents;
      (go.GetComponent<Collider>() as BoxCollider).center = -localPosition;

      go.transform.parent = gameObject.transform;

      go.AddComponent<ClickDetector>();
      var clicker = go.GetComponent<ClickDetector>();
      clicker.clickSignal.AddListener(onClick);
    }

    internal void updateScore(string score)
    {
      latestGO = Instantiate(Resources.Load("Textfield")) as GameObject;
      var go = latestGO;
      var textMesh = go.GetComponent<TextMesh>();
      textMesh.font.material.color = Color.white;
      go.transform.parent = transform;

      textMesh.text = score;
    }

    private void onClick()
    {
      clickSignal.Dispatch();
      startWobble();
    }

    private void startWobble()
    {
      StartCoroutine(wobble(edx_WobbleSize));
      basePosition = Vector3.zero;
    }

    private IEnumerator wobble(float size)
    {
      while (size > edx_WobbleMin)
      {
        size *= edx_WobbleDampen;
        var newPosition = basePosition;
        newPosition.x += Random.Range(-size, size);
        newPosition.y += Random.Range(-size, size);
        newPosition.z += Random.Range(-size, size);
        gameObject.transform.localPosition = newPosition;
        yield return null;
      }

      gameObject.transform.localPosition = basePosition;
    }
  }
}