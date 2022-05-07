using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace net.shutosg.UniEaseCopy
{
    public class DirectoryAnchor : ScriptableObject
    {
        private static readonly Dictionary<string, string> Cache = new Dictionary<string, string>();
        [SerializeField] private string id;

        public static string Find(string id)
        {
            if (Cache.TryGetValue(id, out var path)) return path;
            var targetAnchorInfos = AssetDatabase.FindAssets($"t:{nameof(DirectoryAnchor)}")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(p => (path: p, anchor: AssetDatabase.LoadAssetAtPath<DirectoryAnchor>(p)))
                .Where(x => x.anchor.id == id).ToList();
            if (targetAnchorInfos.Count > 1) throw new MultipleDirectoryAnchorException($"idが '{id}' の {nameof(DirectoryAnchor)} が複数存在します");
            if (targetAnchorInfos.Count == 0) throw new DirectoryAnchorNotFoundException($"idが '{id}' の {nameof(DirectoryAnchor)} が存在しません");
            var targetAnchorPath = targetAnchorInfos[0].path;
            var pathWithoutFileName = targetAnchorPath.Replace(Path.GetFileName(targetAnchorPath), "");
            Cache.Add(id, pathWithoutFileName);
            return pathWithoutFileName;
        }
    }

    public class MultipleDirectoryAnchorException : Exception
    {
        public MultipleDirectoryAnchorException(string message) : base(message) { }
    }

    public class DirectoryAnchorNotFoundException : Exception
    {
        public DirectoryAnchorNotFoundException(string message) : base(message) { }
    }
}
