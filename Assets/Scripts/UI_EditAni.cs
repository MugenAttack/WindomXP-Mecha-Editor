using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class UI_EditAni : MonoBehaviour
{
    public animation cAnim;
    public RoboStructure robo;
    public UI_EditParts ep;
    public UI_InputBox inputBox;
    public UI_MsgBox msgBox;
    public MechaAnimator ma;
    [Header("Select Hod Tab")]
    public Dropdown animDD;
    public Dropdown hodDD;
    public hod2v1 cHod;


    [Header("Script Tab")]
    public Dropdown scriptDD;
    public InputField fLength;
    public InputField hDuration;
    public InputField scriptText;
    public script sData;

    [Header("Extra Tab")]
    public InputField initScript;

    [Header("Constraint Data")]
    public InputField RotC1X;
    public InputField RotC1Y;
    public InputField RotC1Z;
    public InputField RotC2X;
    public InputField RotC2Y;
    public InputField RotC2Z;
    public InputField RotC3X;
    public InputField RotC3Y;
    public InputField RotC3Z;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void populateAnimationList()
    {
        List<string> list = new List<string>();
        for (int i = 0; i < robo.ani.animations.Count; i++)
        {
            string name = robo.ani.animations[i].name;
            if (name == "")
                name = "Empty";
            list.Add((i + 1).ToString() + " - " + name);
        }
        animDD.ClearOptions();
        animDD.AddOptions(list);
        animDD.value = 0;
        AnimationSelected();
    }

    public void AnimationSelected()
    {
        cAnim = robo.ani.animations[animDD.value];
        populateHODList();
        populateScriptList();
        initScript.text = cAnim.squirrelInit;

        if (cAnim.frames.Count > 0 && cAnim.scripts.Count > 0)
            ma.run(cAnim,true);

        animation nAnim = new animation();
        if (animDD.value > 49 && animDD.value < 100)
        {
            nAnim.frames = robo.ani.animations[animDD.value].frames;
            nAnim.scripts = robo.ani.animations[animDD.value - 50].scripts;
            if (nAnim.frames.Count > 0 && nAnim.scripts.Count > 0)
                ma.run(nAnim, true);
        }

        if (animDD.value == 101 || animDD.value == 102)
        {
            nAnim.frames = robo.ani.animations[animDD.value].frames;
            nAnim.scripts = robo.ani.animations[100].scripts;
            if (nAnim.frames.Count > 0 && nAnim.scripts.Count > 0)
                ma.run(nAnim, true);
        }
    }

    public void populateHODList(bool retainPos = false)
    {
        List<string> list = new List<string>();
        for (int i = 0; i < cAnim.frames.Count; i++)
        {
            list.Add(i.ToString() + ". " + cAnim.frames[i].filename);
        }
        hodDD.ClearOptions();
        hodDD.AddOptions(list);

        if (cAnim.frames.Count > 0) { 
            hodDD.value = 0;
            selectHOD();
        }
    }

    public void selectHOD()
    {
        if (cAnim.frames.Count != 0)
        {
            robo.setPose(cAnim.frames[hodDD.value]);
        }

        ep.TransformTextUpdate();
    }

    public void populateScriptList()
    {
        scriptDD.ClearOptions();
        List<string> list = new List<string>();
        list.Clear();
        if (cAnim.scripts.Count != 0)
        {
            for (int i = 0; i < cAnim.scripts.Count; i++)
            {
                list.Add(i.ToString());
            }
            scriptDD.AddOptions(list);
            sData = cAnim.scripts[scriptDD.value];
            scriptSelected();
        }
        else
        {
            fLength.text = "";
            hDuration.text = "";
        }
    }

    public void scriptSelected()
    {
        sData = cAnim.scripts[scriptDD.value];
        fLength.text = sData.unk.ToString();
        hDuration.text = sData.time.ToString();
        scriptText.text = sData.squirrel;

    }

    public void syncScriptData()
    {
        int np = 0;
        if (int.TryParse(fLength.text, out np))
            sData.unk = np;

        float fp = 0;
        if (float.TryParse(hDuration.text, out fp))
            sData.time = fp;

        //List<byte> list = new List<byte>();
        //list.AddRange(USEncoder.ToEncoding.ToSJIS(scriptText.text));
        //for (int i = 0; i < list.Count; i++)
        //{
        //    if (list[i] == 0x0A && list[i - 1] != 0x0D)
        //        list.Insert(i, 0x0D);
        //}
        //sData.squirrel = USEncoder.ToEncoding.ToUnicode(list.ToArray());
        sData.squirrel = scriptText.text;
        cAnim.scripts[scriptDD.value] = sData;
        Debug.Log("Script Sync");
    }

    public void addScript()
    {
        cAnim.scripts.Add(sData);
        populateScriptList();
    }

    public void removeScript()
    {
        cAnim.scripts.RemoveAt(scriptDD.value);
        populateScriptList();
    }

    public void moveUpScript()
    {
        if (scriptDD.value != 0)
        {
            script s = cAnim.scripts[scriptDD.value - 1];
            cAnim.scripts[scriptDD.value - 1] = cAnim.scripts[scriptDD.value];
            cAnim.scripts[scriptDD.value] = s;
        }

        int ddPos = scriptDD.value;
        populateScriptList();
        scriptDD.value = ddPos - 1;
        scriptSelected();
    }

    public void moveDownScript()
    {
        if (scriptDD.value != cAnim.scripts.Count - 1)
        {
            script s = cAnim.scripts[scriptDD.value + 1];
            cAnim.scripts[scriptDD.value + 1] = cAnim.scripts[scriptDD.value];
            cAnim.scripts[scriptDD.value] = s;
        }

        int ddPos = scriptDD.value;
        populateScriptList();
        scriptDD.value = ddPos + 1;
        scriptSelected();
    }

    public void syncInit()
    {
        List<byte> list = new List<byte>();
        list.AddRange(USEncoder.ToEncoding.ToSJIS(initScript.text));
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == 0x0A && list[i - 1] != 0x0D)
                list.Insert(i, 0x0D);
        }
        cAnim.squirrelInit = USEncoder.ToEncoding.ToUnicode(list.ToArray());
    }

    public void updatePart(int i, Vector3 position, Quaternion rotation, Vector3 scale)
    {
        hod2v1_Part prt = cAnim.frames[hodDD.value].parts[i];
        prt.position = position;
        prt.rotation = rotation;
        prt.scale = scale;
        prt.unk1 = rotation;
        prt.unk2 = rotation;
        prt.unk3 = rotation;
        cAnim.frames[hodDD.value].parts[i] = prt;

    }

    public void addHod()
    {
        string dText = "New_Hod_File";
        if (cAnim.frames.Count > 0)
            dText = cAnim.frames[hodDD.value].filename.Replace(".hod", "");
        inputBox.openDialog("What name do you want to give this hod file(exclude the .hod extension)", dText , (string rText) =>
            {
                hod2v1 nFrame = new hod2v1(rText + ".hod");
                nFrame.parts = new List<hod2v1_Part>();
                for (int i = 0; i < robo.parts.Count; i++)
                {
                    hod2v1_Part nPart = new hod2v1_Part();
                    nPart.name = robo.hod.parts[i].name;
                    nPart.treeDepth = robo.hod.parts[i].treeDepth;
                    nPart.childCount = robo.hod.parts[i].childCount;
                    nPart.position = robo.parts[i].transform.localPosition;
                    nPart.rotation = robo.parts[i].transform.localRotation;
                    nPart.scale = robo.parts[i].transform.localScale;
                    nPart.unk1 = nPart.rotation;
                    nPart.unk2 = nPart.rotation;
                    nPart.unk3 = nPart.rotation;
                    nFrame.parts.Add(nPart);
                }
                if (cAnim.frames.Count > 0)
                    cAnim.frames.Insert(hodDD.value + 1, nFrame);
                else
                    cAnim.frames.Add(nFrame);
                populateHODList();
            });
        
        
    }

    public void removeHod()
    {
        if (cAnim.frames.Count != 0)
        {
            cAnim.frames.RemoveAt(hodDD.value);
            populateHODList();
        }
        else
            msgBox.Show("There is no Hod files in this Animation");
    }

    public void hodMoveUp()
    {
        if (hodDD.value != 0)
        {
            hod2v1 prt = cAnim.frames[hodDD.value - 1];
            cAnim.frames[hodDD.value - 1] = cAnim.frames[hodDD.value];
            cAnim.frames[hodDD.value] = prt;
        }
        int ddPos = hodDD.value;
        populateHODList();
        hodDD.value = ddPos - 1;
        selectHOD();
    }

    public void hodMoveDown()
    {
        if (hodDD.value != cAnim.frames.Count - 1)
        {
            hod2v1 prt = cAnim.frames[hodDD.value + 1];
            cAnim.frames[hodDD.value + 1] = cAnim.frames[hodDD.value];
            cAnim.frames[hodDD.value] = prt;
        }

        int ddPos = hodDD.value;
        populateHODList();
        hodDD.value = ddPos + 1;
        selectHOD();
    }

    public void hodRename()
    {
        inputBox.gameObject.SetActive(true);
        inputBox.openDialog("What name do you want to change this hod to", cAnim.frames[hodDD.value].filename, (string rText) =>
        {
            cAnim.frames[hodDD.value].filename = rText;
            int ddPos = hodDD.value;
            populateHODList();
            hodDD.value = ddPos;
            selectHOD();
        });
    }

    public void copyHod()
    {
        cHod = cAnim.frames[hodDD.value];
    }

    public void pasteHod()
    {
        for (int i = 0; i < cHod.parts.Count; i++)
            cAnim.frames[hodDD.value].parts[i] = cHod.parts[i];

        selectHOD();
    }
    public void syncPart()
    {
        hod2v1_Part prt = cAnim.frames[hodDD.value].parts[ep.index];
        int count = robo.ani.animations.Count;
        for (int i = 0; i < count; i++)
        {
            animation a = robo.ani.animations[i];
            for (int j = 0; j < a.frames.Count; j++)
            {
                a.frames[j].parts[ep.index] = prt;
            }
        }
    }

    public void syncPartSpecific()
    {
        inputBox.openDialog("Type in what animations you want the part to sync with separated by a comma. a dash between 2 animations will do a range.  example: 1,2, 6-10", "", (string rText) =>
        {
            string[] selections = rText.Split(',');
            hod2v1_Part prt = cAnim.frames[hodDD.value].parts[ep.index];
            for (int i = 0; i < selections.Length; i++)
            {
                if (selections[i].Contains("-"))
                {
                    int l = 0;
                    int t = 0;
                    string[] s = selections[i].Split("-".ToCharArray());
                    if (int.TryParse(s[0], out l) && int.TryParse(s[1], out t))
                    {
                        for (int j = l; j < t + 1; j++)
                        {
                            animation a = robo.ani.animations[j];
                            for (int k = 0; k < a.frames.Count; k++)
                            {
                                a.frames[k].parts[ep.index] = prt;
                            }
                        }
                    }
                }
                else
                {
                    int v = 0;
                    if (int.TryParse(selections[i], out v))
                    {
                        animation a = robo.ani.animations[v];
                        for (int k = 0; k < a.frames.Count; k++)
                        {
                            a.frames[k].parts[ep.index] = prt;
                        }
                    }
                }
            }
        });
    }

    public void saveAni()
    {
        robo.ani.save();
    }

    public void dumpAniList()
    {
        StreamWriter sw = new StreamWriter("AnimationList.txt");
        int aCount = robo.ani.animations.Count;
        for(int i = 0; i < aCount; i++)
        {
            string[] name = robo.ani.animations[i].name.Split((char)0x00);
            sw.WriteLine((i + 1).ToString() + "-" + name[0]);
        }
        sw.Close();
    }

    public void syncAniList()
    {
        StreamReader sr = new StreamReader("AnimationList.txt");
        while (!sr.EndOfStream)
        {
            string[] name = sr.ReadLine().Split('-');
            int id;
            if (int.TryParse(name[0],out id))
            {
                robo.ani.animations[id - 1].name = name[1];
            }
        }
        sr.Close();
        populateAnimationList();
    }

    public void setConstraintText()
    {
        hod2v1_Part p = cAnim.frames[hodDD.value].parts[ep.index];
        Vector3 e = p.unk1.eulerAngles;
        RotC1X.text = e.x.ToString();
        RotC1Y.text = e.y.ToString();
        RotC1Z.text = e.z.ToString();
        e = p.unk2.eulerAngles;
        RotC2X.text = e.x.ToString();
        RotC2Y.text = e.y.ToString();
        RotC2Z.text = e.z.ToString();
        e = p.unk3.eulerAngles;
        RotC3X.text = e.x.ToString();
        RotC3Y.text = e.y.ToString();
        RotC3Z.text = e.z.ToString();
    }

    public void updateConstraintData()
    {
        Vector3 r = new Vector3();
        float f = 0;
          
        if (float.TryParse(RotC1X.text, out f))
            r.x = f;
        if (float.TryParse(RotC1Y.text, out f))
            r.y = f;
        if (float.TryParse(RotC1Z.text, out f))
            r.z = f;
        Quaternion c1 = Quaternion.Euler(r);

        if (float.TryParse(RotC2X.text, out f))
            r.x = f;
        if (float.TryParse(RotC2Y.text, out f))
            r.y = f;
        if (float.TryParse(RotC2Z.text, out f))
            r.z = f;
        Quaternion c2 = Quaternion.Euler(r);

        if (float.TryParse(RotC3X.text, out f))
            r.x = f;
        if (float.TryParse(RotC3Y.text, out f))
            r.y = f;
        if (float.TryParse(RotC3Z.text, out f))
            r.z = f;
        Quaternion c3 = Quaternion.Euler(r);

        robo.updateConstraints(animDD.value,hodDD.value,ep.index,c1,c2,c3);
    }
}
