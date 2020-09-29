using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    ScanManager scanManager;
    public RawImage rawImage;

    private void OnEnable()
    {
        scanManager = GetComponent<ScanManager>();
        var configPath = System.IO.Path.Combine(Application.streamingAssetsPath, "config.xml").Replace('\\', '/');
        XElement root = XElement.Load(configPath);
        var folder = root.Element("AbsoluteFolder").Value;
        int Lastest = int.Parse(root.Element("Lastest").Value);
        scanManager.TextureFolder = folder;
        scanManager.LastCount = Lastest;
    }


    private void Update()
    {
        if (scanManager.Textures.Count > 0)
        {
            var tex = scanManager.Textures.Dequeue();
            rawImage.texture = tex;
            rawImage.SetNativeSize();
        }
    }
}
