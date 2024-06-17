using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class UI_SelectMech : MonoBehaviour
{
    public Dropdown RoboDD;
    List<string> list = new List<string>();
    public Image selectImage;
    public Material selectMaterial;
    public bool enableTool = true;
    public UI_MsgBox msgBox;
    public RoboStructure robo;
    public RoboStructure prevRobo;
    public UI_ViewControl vc;
    public GameObject saveAni;
    public GameObject saveHod;
    public UI_Tabs editTabs;
    public GameObject modeSelect;
    public UI_EditParts editParts;
    public UI_EditAni editAni;
    public string folder = "Windom_Data\\Robo";
    public GameObject prefPanel;
    // Start is called before the first frame update
    void Start()
    {
        robo.transcoder = new CypherTranscoder();
        RoboDD.ClearOptions();
        if (Directory.Exists(folder))
        {
            DirectoryInfo directory = new DirectoryInfo(folder);

            if (directory.GetDirectories().Length > 0)
            {
                List<string> options = new List<string>();
                foreach (DirectoryInfo di in directory.GetDirectories())
                {
                    options.Add(di.Name);
                    list.Add(di.Name);
                }
                RoboDD.AddOptions(options);
                selectedMech(0);

                selectImage.material = selectMaterial;
            }
            else
            {
                msgBox.Show("There is no Mechs in the Robo Folder.");
                enableTool = false;
            }
        }
        else
        {
            Directory.CreateDirectory(folder);
            msgBox.Show("There is no Mechs in the Robo Folder.");
            enableTool = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectedMech(int value)
    {
        if (File.Exists(Path.Combine(folder, list[RoboDD.value], "select.png")))
        { 
            robo.transcoder.findCypher(Path.Combine(folder, list[RoboDD.value], "select.png"));
            Texture2D tex = Helper.LoadTextureEncrypted(Path.Combine(folder, list[RoboDD.value], "select.png"), ref robo.transcoder);
            Sprite st = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));

            selectImage.sprite = st;
        }
        //selectImage.material = selectMaterial;
        //main.setPath(Path.Combine("Windom_Data\\Robo", list[RoboDD.value]));
    }

    public void loadFile(string name)
    {
        if (enableTool)
        {
            ani2 ani = new ani2();
            ani.load(Path.Combine(folder, list[RoboDD.value], name));
            robo.folder = Path.Combine(folder, list[RoboDD.value]);
            robo.buildStructure(ani.structure);
            if (name.Contains(".ani"))
            {
                robo.ani = ani;
                robo.filename = name;
                prevRobo.folder = robo.folder;
                prevRobo.buildStructure(ani.structure);
                foreach (GameObject prt in prevRobo.parts)
                {
                    prt.layer = 7;
                }
                saveAni.SetActive(true);
                saveHod.SetActive(false);
                editAni.populateAnimationList();
                if (editTabs != null)
                    editTabs.setTabActive(0, true);
                if (modeSelect != null)
                    modeSelect.SetActive(true);
                
            }
            else
            {
                robo.filename = ani._filename;
                saveAni.SetActive(false);
                saveHod.SetActive(true);
                if (editTabs != null)
                    editTabs.setTabActive(0, false);
                if (modeSelect != null)
                    modeSelect.SetActive(false);
            }

            editParts.PopulatePartsList();
            this.gameObject.SetActive(false);
            vc.Menu.SetActive(true);
            vc.EditMode(true);
            prefPanel.SetActive(false);
        }
    }

    public void setFolder(string value)
    {
        if (Directory.Exists(value))
        {
            folder = value;
            RoboDD.ClearOptions();
            list.Clear();
            DirectoryInfo directory = new DirectoryInfo(folder);

            if (directory.GetDirectories().Length > 0)
            {
                List<string> options = new List<string>();
                foreach (DirectoryInfo di in directory.GetDirectories())
                {
                    options.Add(di.Name);
                    list.Add(di.Name);
                }
                RoboDD.AddOptions(options);
                selectedMech(0);

                selectImage.material = selectMaterial;
            }
            else
            {
                msgBox.Show("There is no Mechs in the Robo Folder.");
                enableTool = false;
            }
        }
        
    }
}
