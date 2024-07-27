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

  T FindObjectOfType<T>() where T : UnityEngine.Object, IObjectAdapter;
  T FindObjectOfType<T>(bool includeInactive) where T : UnityEngine.Object, IObjectAdapter;
  IObjectAdapter FindObjectOfType(Type type);
  IObjectAdapter FIndObjectOfType(Type type, bool includeInactive);

  IObjectAdapter[] FindObjectsOfType(Type type);
  IObjectAdapter[] FindObjectsOfType(Type type, bool includeInactive);
  T[] FindObjectsOfType<T> (bool includeInactive) where T : UnityEngine.Object, IObjectAdapter;
  T[] FindObjectsOfType<T>() where T : UnityEngine.Object, IObjectAdapter;

  IObjectAdapter Instantiate(IObjectAdapter original);
  IObjectAdapter Instantiate(IObjectAdapter original, Transform parent);
  IObjectAdapter Instantiate(IObjectAdapter original, Transform parent, bool instantiateInWorldSpace);
  IObjectAdapter Instantiate(IObjectAdapter original, Vector3 position, Quaternion rotation);
  IObjectAdapter Instantiate(IObjectAdapter original, Vector3 position, Quaternion rotation, Transform parent);
}

public class ObjectAdapter : IObjectAdapter {
  private readonly UnityEngine.Object _obj;

  public ObjectAdapter(UnityEngine.Object obj) => _obj = obj;

  public string Name { get => _obj.name; set => _obj.name = value; }
  public HideFlags HideFlags { get => _obj.hideFlags; set => _obj.hideFlags = value; }

  public int GetInstanceID() => _obj.GetInstanceID();

  public override string ToString() => _obj.ToString();

  public void Destroy(IObjectAdapter objA, float f = 0) {
    UnityEngine.Object obj = (objA as ObjectAdapter)._obj;
    UnityEngine.Object.Destroy(obj, f);
  }

  public void DestroyImmediate(IObjectAdapter objA, bool allowDestroyingAssets = false) {
    UnityEngine.Object obj = (objA as ObjectAdapter)._obj;
    UnityEngine.Object.DestroyImmediate(obj, allowDestroyingAssets);
  }

  public void DontDestroyOnLoad(IObjectAdapter target) {
    UnityEngine.Object obj = (target as ObjectAdapter)._obj;
    UnityEngine.Object.DontDestroyOnLoad(obj);
  }

  public T FindObjectOfType<T>() where T : UnityEngine.Object, IObjectAdapter {
    UnityEngine.Object foundObject = UnityEngine.Object.FindObjectOfType<T>();
    return foundObject != null ? (T)Activator.CreateInstance(typeof(T), foundObject) : default;
  }

  public T FindObjectOfType<T>(bool includeInactive) where T : UnityEngine.Object, IObjectAdapter {
    UnityEngine.Object foundObject = UnityEngine.Object.FindObjectOfType<T>(includeInactive);
    return foundObject != null ? (T)Activator.CreateInstance(typeof(T), foundObject) : default;
  }

  public IObjectAdapter FindObjectOfType(Type type) {
    UnityEngine.Object foundObject = UnityEngine.Object.FindObjectOfType(type);
    return foundObject != null ? new ObjectAdapter(foundObject) : null;
  }

  public IObjectAdapter FIndObjectOfType(Type type, bool includeInactive) {
    UnityEngine.Object foundObject = UnityEngine.Object.FindObjectOfType(type, includeInactive);
    return foundObject != null ? new ObjectAdapter(foundObject) : null;
  }

  public IObjectAdapter[] FindObjectsOfType(Type type) {
    UnityEngine.Object[] foundObjects = UnityEngine.Object.FindObjectsOfType(type);

    IObjectAdapter[] adapters = new IObjectAdapter[foundObjects.Length];
    for (int i = 0; i < foundObjects.Length; i++)
      adapters[i] = new ObjectAdapter(foundObjects[i]);

    return adapters;
  }

  public IObjectAdapter[] FindObjectsOfType(Type type, bool includeInactive) {
    UnityEngine.Object[] foundObjects = UnityEngine.Object.FindObjectsOfType(type, includeInactive);

    IObjectAdapter[] adapters = new IObjectAdapter[foundObjects.Length];
    for (int i = 0; i < foundObjects.Length; i++)
      adapters[i] = new ObjectAdapter(foundObjects[i]);

    return adapters;
  }

  public T[] FindObjectsOfType<T>(bool includeInactive) where T : UnityEngine.Object, IObjectAdapter {
    UnityEngine.Object[] foundObjects = UnityEngine.Object.FindObjectsOfType<T>(includeInactive);

    T[] adapters = new T[foundObjects.Length];
    for (int i = 0; i < foundObjects.Length; i++)
      adapters[i] = (T)Activator.CreateInstance(typeof(T), foundObjects[i]);

    return adapters;
  }

  public T[] FindObjectsOfType<T>() where T : UnityEngine.Object, IObjectAdapter {
    UnityEngine.Object[] foundObjects = UnityEngine.Object.FindObjectsOfType<T>();

    T[] adapters = new T[foundObjects.Length];
    for (int i = 0; i < foundObjects.Length; i++)
      adapters[i] = (T)Activator.CreateInstance(typeof(T), foundObjects[i]);

    return adapters;
  }

  public IObjectAdapter Instantiate(IObjectAdapter original) {
    UnityEngine.Object obj = (original as ObjectAdapter)._obj;
    UnityEngine.Object instantiatedObj = UnityEngine.Object.Instantiate(obj);
    return new ObjectAdapter(instantiatedObj);
  }

  public IObjectAdapter Instantiate(IObjectAdapter original, Transform parent) {
    UnityEngine.Object obj = (original as ObjectAdapter)._obj;
    UnityEngine.Object instantiatedObj = UnityEngine.Object.Instantiate(obj, parent);
    return new ObjectAdapter(instantiatedObj);
  }

  public IObjectAdapter Instantiate(IObjectAdapter original, Transform parent, bool instantiateInWorldSpace) {
    UnityEngine.Object obj = (original as ObjectAdapter)._obj;
    UnityEngine.Object instantiatedObj = UnityEngine.Object.Instantiate(obj, parent, instantiateInWorldSpace);
    return new ObjectAdapter(instantiatedObj);
  }

  public IObjectAdapter Instantiate(IObjectAdapter original, Vector3 position, Quaternion rotation) {
    UnityEngine.Object obj = (original as ObjectAdapter)._obj;
    UnityEngine.Object instantiatedObj = UnityEngine.Object.Instantiate(obj, position, rotation);
    return new ObjectAdapter(instantiatedObj);
  }

  public IObjectAdapter Instantiate(IObjectAdapter original, Vector3 position, Quaternion rotation, Transform parent) {
    UnityEngine.Object obj = (original as ObjectAdapter)._obj;
    UnityEngine.Object instantiatedObj = UnityEngine.Object.Instantiate(obj, position, rotation, parent);
    return new ObjectAdapter(instantiatedObj);
  }
}

} // namespace