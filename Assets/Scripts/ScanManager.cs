using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using UnityEngine;
using System.Linq;
[DisallowMultipleComponent]
public class ScanManager : MonoBehaviour
{
    private Queue<BitmapMessage> ptrs = new Queue<BitmapMessage>();
    private List<string> texNames = new List<string>();
    private Queue<Texture2D> tex2ds = new Queue<Texture2D>();

    private int count = 20;
    public int LastCount 
    {
        get => count;
        set => count = value;
    }

    private string texFolder = string.Empty;
    public string TextureFolder
    {
        get => texFolder;
        set => texFolder = value;
    }

    private bool searching = true;
    Thread thread = null;
    public Queue<Texture2D> Textures => tex2ds;
    
    private void Start()
    {
        if (!Directory.Exists(texFolder))
        {
            Debug.LogError("Texture folder is not found");
            return;
        }
        thread = new Thread(ScanFolder);
        thread.Start();
    }


    private void ScanFolder()
    {
        while (searching)
        {
            DirectoryInfo folder = new DirectoryInfo(texFolder);
            var fileInfos = folder.GetFiles("*.png", SearchOption.TopDirectoryOnly);
            var infos = fileInfos.OrderByDescending(file => file.CreationTime).Take(LastCount);

            foreach (var info in infos)
            {
                LoadBitMap2Intptr(info.Name, info.FullName);
            }
            Thread.Sleep(5000);
        }
    }

    private void Update()
    {
        if (ptrs.Count > 0)
        {
            var bitmapMessage = ptrs.Dequeue();
            tex2ds.Enqueue(BitmapMessage2Texture2d(bitmapMessage));
        }
    }

    private Texture2D BitmapMessage2Texture2d(BitmapMessage message)
    {
        var bitmap = message.bitMap;
        var intPtr = message.intPtr;
        Texture2D tex = new Texture2D(bitmap.Width, bitmap.Height, TextureFormat.BGRA32, false);
        tex.LoadRawTextureData(intPtr, bitmap.Width * bitmap.Height * 4);
        tex.Apply();
        bitmap.Dispose();
        return tex;
    }

    private void LoadBitMap2Intptr(string fileName, string fullName)
    {
        if (texNames.Contains(fileName))
            return;
        texNames.Add(fileName);
        Bitmap bitmap = (Bitmap)Bitmap.FromFile(@fullName);
        IntPtr intPtr = bitmap.BitMap2IntPtr();
        ptrs.Enqueue(new BitmapMessage(bitmap, intPtr));
    }

    private void OnDisable()
    {
        searching = false;
        thread.Abort();
    }
}
