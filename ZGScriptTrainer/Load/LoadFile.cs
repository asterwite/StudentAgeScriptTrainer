using Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine.AddressableAssets;
using MessagePack;
using MessagePack.Resolvers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ZGScriptTrainer.Load
{
    internal class LoadFile
    {
        public static Dictionary<int, T> Load<T>(string _path)
        {
            CfgMgr.Init();
            Dictionary<int, T> dictionary = new Dictionary<int, T>();
            TextAsset textAsset = Resources.Load<TextAsset>(_path);
            if (textAsset != null)
            {
                using (Dictionary<string, T>.Enumerator enumerator = MessagePackSerializer.Deserialize<Dictionary<string, T>>(MessagePackSerializer.ConvertFromJson(textAsset.text, null, default(CancellationToken)), null, default(CancellationToken)).GetEnumerator())
                {
                    //while (enumerator.MoveNext())
                    //{
                    //    KeyValuePair<string, T> keyValuePair = enumerator.Current;
                    //    dictionary.Add(int.Parse(keyValuePair.Key), keyValuePair.Value);
                    //}
                    return dictionary;
                }
            }
            Debug.LogWarning("配置表不存在" + _path);
            return dictionary;
        }
        //加载资源文件
        public static Dictionary<int, T> LoadAsync<T>(string _path, Action<Dictionary<int, T>> _callback)
        {
            Dictionary<int, T> tar;
            if (CfgMgr.enableAsync)
            {
                CfgMgr.Init();
                CfgMgr.restLoadCnt++;
                CfgMgr.maxLoadCnt++;
                Addressables.LoadAssetAsync<TextAsset>(ResPath.ToAAUrl(_path)).Completed += delegate (AsyncOperationHandle<TextAsset> obj)
                {
                    CfgMgr.restLoadCnt--;
                    if (obj.Status == AsyncOperationStatus.Succeeded)
                    {
                        TextAsset result = obj.Result;
                        tar = new Dictionary<int, T>();
                        try
                        {
                            foreach (KeyValuePair<string, T> keyValuePair in MessagePackSerializer.Deserialize<Dictionary<string, T>>(MessagePackSerializer.ConvertFromJson(result.text, null, default(CancellationToken)), null, default(CancellationToken)))
                            {
                                tar.Add(int.Parse(keyValuePair.Key), keyValuePair.Value);
                            }
                        }
                        catch (Exception ex)
                        {
                            string text = "Deserialize Failed：";
                            string path = _path;
                            string text2 = "\n";
                            Exception ex2 = ex;
                            Debug.LogError(text + path + text2 + ((ex2 != null) ? ex2.ToString() : null));
                        }
                        
                        _callback.DynamicInvoke(new object[] { tar });
                    }
                    else
                    {
                        Debug.LogWarning("配置表不存在" + _path);
                    }
                    //多余部分，调用会导致跳转主菜单，并且造成严重卡顿
                    //Action<int, int> action = CfgMgr.compCallback;
                    //if (action == null)
                    //{
                    //    return;
                    //}
                    //action(CfgMgr.restLoadCnt, CfgMgr.maxLoadCnt);
                };
                return null;
            }
            tar = Load<T>(_path);
            //_callback.DynamicInvoke(new object[] { tar });
            return tar;
        }
    }
}
