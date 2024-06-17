using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UI_SPT : MonoBehaviour
{
    public InputField SPTField;
    public RoboStructure robo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadSPTField()
    {
        byte[] file = robo.transcoder.Transcode(Path.Combine(robo.folder, "Script.spt"));
        SPTField.text = USEncoder.ToEncoding.ToUnicode(file);
    }

    public void saveSPT()
    {
        List<byte> list = new List<byte>();
        list.AddRange(USEncoder.ToEncoding.ToSJIS(SPTField.text));
        for (int j = 0; j < list.Count; j++)
        {
            if (list[j] == 0x0A && list[j - 1] != 0x0D)
                list.Insert(j, 0x0D);
        }
        byte[] tFile = robo.transcoder.Transcode(list.ToArray());
        File.WriteAllBytes(Path.Combine(robo.folder, "Script.spt"), tFile);
    }
}
