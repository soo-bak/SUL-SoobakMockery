using System;
using System.Linq;
using UnityEngine;

namespace SUL.Adapters {

public interface IObjectAdapter {
  string Name { get; set; }
  HideFlags HideFlags { get; set; }

  int GetInstanceID();
  string ToString();
  void Destroy(IObjectAdapter objAdapter, float f = 0);
  void DestroyImmediate(IObjectAdapter objAdapter, bool allowDestroyingAssets = false);
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

public class ObjectAdapter : IObjectAdapter {
  private UnityEngine.Object adaptee;

  public ObjectAdapter(UnityEngine.Object adaptee)
    => this.adaptee = adaptee;

  public string Name {
    get => adaptee.name;
    set => adaptee.name = value;
  }

  public HideFlags HideFlags {
    get => adaptee.hideFlags;
    set => adaptee.hideFlags = value;
  }

  public int GetInstanceID() => adaptee.GetInstanceID();

  public override string ToString() => adaptee.ToString();

  public void Destroy(IObjectAdapter obj, float t = 0) {
    if (obj is ObjectAdapter adapter)
      UnityEngine.Object.Destroy(adapter.adaptee, t);
  }

  public void DestroyImmediate(IObjectAdapter obj, bool allowDestroyingAssets = false) {
    if (obj is ObjectAdapter adapter)
      UnityEngine.Object.DestroyImmediate(adapter.adaptee, allowDestroyingAssets);
  }

  public void DontDestroyOnLoad(IObjectAdapter target) {
    if (target is ObjectAdapter adapter)
      UnityEngine.Object.DontDestroyOnLoad(adapter.adaptee);
  }

  public T FindObjectOfType<T>() where T : UnityEngine.Object
    => UnityEngine.Object.FindObjectOfType<T>();

  public T FindObjectOfType<T>(bool includeInactive) where T : UnityEngine.Object
    => UnityEngine.Object.FindObjectOfType<T>(includeInactive);

  public IObjectAdapter FindObjectOfType(Type type) {
    var foundObject = UnityEngine.Object.FindObjectOfType(type);
    return foundObject != null ? new ObjectAdapter(foundObject) : null;
  }

  public IObjectAdapter FindObjectOfType(Type type, bool includeInactive) {
    var foundObject = UnityEngine.Object.FindObjectOfType(type, includeInactive);
    return foundObject != null ? new ObjectAdapter(foundObject) : null;
  }

  public IObjectAdapter[] FindObjectsOfType(Type type) {
    var foundObjects = UnityEngine.Object.FindObjectsOfType(type);
    return foundObjects?.Select(obj => new ObjectAdapter(obj)).ToArray();
  }

  public IObjectAdapter[] FindObjectsOfType(Type type, bool includeInactive) {
    var foundObjects = UnityEngine.Object.FindObjectsOfType(type, includeInactive);
    return foundObjects?.Select(obj => new ObjectAdapter(obj)).ToArray();
  }

  public T[] FindObjectsOfType<T>(bool includeInactive) where T : UnityEngine.Object
    => UnityEngine.Object.FindObjectsOfType<T>(includeInactive);

  public T[] FindObjectsOfType<T>() where T : UnityEngine.Object
    => UnityEngine.Object.FindObjectsOfType<T>();

  public IObjectAdapter Instantiate(IObjectAdapter original) {
    if (original is ObjectAdapter adapter) {
      var instantiatedObj = UnityEngine.Object.Instantiate(adapter.adaptee);
      return new ObjectAdapter(instantiatedObj);
    }
    return null;
  }

  public IObjectAdapter Instantiate(IObjectAdapter original, Transform parent) {
    if (original is ObjectAdapter adapter) {
      var instantiatedObj = UnityEngine.Object.Instantiate(adapter.adaptee, parent);
      return new ObjectAdapter(instantiatedObj);
    }
    return null;
  }

  public IObjectAdapter Instantiate(IObjectAdapter original, Transform parent, bool instantiateInWorldSpace) {
    if (original is ObjectAdapter adapter) {
      var instantiatedObj = UnityEngine.Object.Instantiate(adapter.adaptee, parent, instantiateInWorldSpace);
      return new ObjectAdapter(instantiatedObj);
    }
    return null;
  }

  public IObjectAdapter Instantiate(IObjectAdapter original, Vector3 position, Quaternion rotation) {
    if (original is ObjectAdapter adapter) {
      var instantiatedObj = UnityEngine.Object.Instantiate(adapter.adaptee, position, rotation);
      return new ObjectAdapter(instantiatedObj);
    }
    return null;
  }

  public IObjectAdapter Instantiate(IObjectAdapter original, Vector3 position, Quaternion rotation, Transform parent) {
    if (original is ObjectAdapter adapter) {
      var instantiatedObj = UnityEngine.Object.Instantiate(adapter.adaptee, position, rotation, parent);
      return new ObjectAdapter(instantiatedObj);
    }
    return null;
  }
}

} // namespace
