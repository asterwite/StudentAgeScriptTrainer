using Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.U2D;
using UnityEngine;
using Config;

namespace ZGScriptTrainer.Load
{
    public class LoadSprite
    {
        public static string GetFriendIcon(PersonCfg _cfg)
        {
            if (_cfg.url.IsEmpty<string>() && _cfg.url2.IsEmpty<string>())
            {
                return null;
            }
            return "role_comic/" + CfgExtension.GetRoleUrlName(_cfg, 0, 0, 0);
        }
        public static Sprite GetSprite(string _url)
        {
            if (string.IsNullOrEmpty(_url))
            {
                //Utils.Warn(new object[] { "空的图片地址" });
                return null;
            }
            string[] arr = _url.Split(new char[] { '/' });
            if (arr.Length == 0)
            {
                //Utils.Warn(new object[] { "不是有效的图片地址：" + _url });
                return null;
            }
            if (!AtlasMgr.ins.atlasDict.ContainsKey(arr[0]))
            {
                ResMgr.LoadAsync<SpriteAtlas>(ResPath.ToAtlasUrl(arr[0]), delegate (SpriteAtlas _v)
                {
                    if (!AtlasMgr.ins.atlasDict.ContainsKey(arr[0]))
                    {
                        AtlasMgr.ins.atlasDict.Add(arr[0], _v);
                    }
                   //GetSpriteFromAtlas(arr[0], arr[1]);
                }, null, false);
                return null;
            }
           return GetSpriteFromAtlas(arr[0], arr[1]);
        }
        public static Sprite GetSpriteFromAtlas(string _atlasName, string _spriteName)
        {
            SpriteAtlas spriteAtlas = AtlasMgr.ins.atlasDict[_atlasName];
            if (spriteAtlas == null)
            {
                return null;
            }
            Sprite sprite = spriteAtlas.GetSprite(_spriteName);
            if(sprite!=null)
            return sprite;

            return null;
        }
     
    }
}
