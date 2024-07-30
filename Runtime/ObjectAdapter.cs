using System;
using UnityEngine;

namespace SUL.Adapters {

public interface IObjectAdapter {
  string Name { get; set; }
  HideFlags HideFlags { get; set; }

  int GetInstanceID();

  string ToString();

  void Destroy(IObjectAdapter objA, float f = 0);

  void DestroyImmediate(IObjectAdapter objA, bool allowDestroyingAssets = false);

  void DontDestroyOnLoad(IObjectAdapter target);

  T FindObjectOfType<T>() where T : UnityEngine.Object;
  T FindObjectOfType<T>(bool includeInactive) where T : UnityEngine.Object;

  IObjectAdapter FindObjectOfType(Type type);
  IObjectAdapter FindObjectOfType(Type type, bool includeInactive);

  IObjectAdapter[] FindObjectsOfType(Type type);
  IObjectAdapter[] FindObjectsOfType(Type type, bool includeInactive);
  T[] FindObjectsOfType<T>(bool includeInactive) where T : UnityEngine.Object;
  T[] FindObjectsOfType<T>() where T : UnityEngine.Object;

  IObjectAdapter Instantiate(IObjectAdapter original);
  IObjectAdapter Instantiate(IObjectAdapter original, Transform parent);
  IObjectAdapter Instantiate(IObjectAdapter original, Transform parent, bool instantiateInWorldSpace);
  IObjectAdapter Instantiate(IObjectAdapter original, Vector3 position, Quaternion rotation);
  IObjectAdapter Instantiate(IObjectAdapter original, Vector3 position, Quaternion rotation, Transform parent);
}

public class ObjectAdapter : UnityEngine.Object, IObjectAdapter {
  private readonly UnityEngine.Object adaptee;

  public ObjectAdapter(UnityEngine.Object obj) => this.adaptee = obj;

  public string Name { get => adaptee.name; set => adaptee.name = value; }
  public HideFlags HideFlags { get => adaptee.hideFlags; set => adaptee.hideFlags = value; }

  public override string ToString() => adaptee.ToString();

  public void Destroy(IObjectAdapter obj, float t = 0)
    => UnityEngine.Object.Destroy((obj as ObjectAdapter).adaptee, t);

  public void DestroyImmediate(IObjectAdapter obj, bool allowDestroyingAssets = false)
    => UnityEngine.Object.DestroyImmediate((obj as ObjectAdapter).adaptee, allowDestroyingAssets);

  public void DontDestroyOnLoad(IObjectAdapter target)
    => UnityEngine.Object.DontDestroyOnLoad((target as ObjectAdapter).adaptee);

  public new T FindObjectOfType<T>() where T : UnityEngine.Object
    => UnityEngine.Object.FindObjectOfType<T>();

  public new T FindObjectOfType<T>(bool includeInactive) where T : UnityEngine.Object
    => UnityEngine.Object.FindObjectOfType<T>(includeInactive);

  public new IObjectAdapter FindObjectOfType(Type type) {
    var foundObject = UnityEngine.Object.FindObjectOfType(type);
    return foundObject != null ? new ObjectAdapter(foundObject) : null;
  }

  public new IObjectAdapter FindObjectOfType(Type type, bool includeInactive) {
    var foundObject = UnityEngine.Object.FindObjectOfType(type, includeInactive);
    return foundObject != null ? new ObjectAdapter(foundObject) : null;
  }

  public new IObjectAdapter[] FindObjectsOfType(Type type) {
    var foundObjects = UnityEngine.Object.FindObjectsOfType(type);
    if (foundObjects == null) return null;

    var adapters = new IObjectAdapter[foundObjects.Length];
    for (int i = 0; i < foundObjects.Length; i++)
      adapters[i] = new ObjectAdapter(foundObjects[i]);

    return adapters;
  }

  public new IObjectAdapter[] FindObjectsOfType(Type type, bool includeInactive) {
    var foundObjects = UnityEngine.Object.FindObjectsOfType(type, includeInactive);
    if (foundObjects == null) return null;

    var adapters = new IObjectAdapter[foundObjects.Length];
    for (int i = 0; i < foundObjects.Length; i++)
      adapters[i] = new ObjectAdapter(foundObjects[i]);

    return adapters;
  }

  public new T[] FindObjectsOfType<T>(bool includeInactive) where T : UnityEngine.Object
    => UnityEngine.Object.FindObjectsOfType<T>(includeInactive);

  public new T[] FindObjectsOfType<T>() where T : UnityEngine.Object
    => UnityEngine.Object.FindObjectsOfType<T>();

  public IObjectAdapter Instantiate(IObjectAdapter original) {
    var instantiatedObj = UnityEngine.Object.Instantiate((original as ObjectAdapter).adaptee);
    return new ObjectAdapter(instantiatedObj);
  }

  public IObjectAdapter Instantiate(IObjectAdapter original, Transform parent) {
    var instantiatedObj = UnityEngine.Object.Instantiate((original as ObjectAdapter).adaptee, parent);
    return new ObjectAdapter(instantiatedObj);
  }

  public IObjectAdapter Instantiate(IObjectAdapter original, Transform parent, bool instantiateInWorldSpace) {
    var instantiatedObj = UnityEngine.Object.Instantiate((original as ObjectAdapter).adaptee, parent, instantiateInWorldSpace);
    return new ObjectAdapter(instantiatedObj);
  }

  public IObjectAdapter Instantiate(IObjectAdapter original, Vector3 position, Quaternion rotation) {
    var instantiatedObj = UnityEngine.Object.Instantiate((original as ObjectAdapter).adaptee, position, rotation);
    return new ObjectAdapter(instantiatedObj);
  }

  public IObjectAdapter Instantiate(IObjectAdapter original, Vector3 position, Quaternion rotation, Transform parent) {
    var instantiatedObj = UnityEngine.Object.Instantiate((original as ObjectAdapter).adaptee, position, rotation, parent);
    return new ObjectAdapter(instantiatedObj);
  }
}

} // namespace
