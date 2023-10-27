using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;

public static class SecureGameObject {

    public static GameObject FindObject(string objectName)
    {
        GameObject go = GameObject.Find(objectName);
        if (go == null) Debug.LogError($"{objectName} was not found!");
        return go;
    }

    public static T FindObjectOfType<T>() where T : Object {
        T result = GameObject.FindObjectOfType<T>();
        if (result == null) Debug.LogError($"object which has the type {typeof(T).Name} was not found");
        return result;
    }

    public static void CheckNull(Object obj)
    {
        if(obj == null)
        {
            string paramName = MemberInfoGetting.GetMemberName(() => obj);
            Debug.LogError($"parametr {paramName} is null!");
        }
    }


}

public static class MemberInfoGetting
{
    public static string GetMemberName<T>(Expression<System.Func<T>> memberExpression)
    {
        MemberExpression expressionBody = (MemberExpression)memberExpression.Body;
        return expressionBody.Member.Name;
    }
}
