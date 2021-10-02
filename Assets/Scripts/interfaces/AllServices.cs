using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AllServices
{
    public static AllServices Container { get; } = new AllServices();

    public void Register<T>(T implementation) where T : IService =>
      Implementation<T>.ServiceInstance = implementation;

    public T Get<T>() where T : IService =>
      Implementation<T>.ServiceInstance;
    public void UnRegister<T>() where T : IService =>
      Implementation<T>.ServiceInstance = default;

    private static class Implementation<T> where T : IService
    {
        public static T ServiceInstance;
    }
}

public interface IService
{
}