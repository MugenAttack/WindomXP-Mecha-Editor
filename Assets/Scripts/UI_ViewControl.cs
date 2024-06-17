using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
public class UI_ViewControl : MonoBehaviour
{
    public GameObject Menu;
    
    [Header("EditPanel")]
    public GameObject EditPanel;
    public GameObject EditMenu;
    public GameObject SelectPanel;
    public RoboStructure robo;
    public UI_EditAni ea;
    

    [Header("AnimPreview")]
    public GameObject PrevPanel;
    public GameObject PrevWindow;

    [Header("Settings")]
    public InputField folderLoc;
    
    // Start is called before the first frame update
    void Start()
    {
        Menu.SetActive(false);
        EditMode(false);
        loadSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ModeSelect(int id)
    {
        switch (id)
        {
            case 0:
                EditMode(true);
                if (PrevPanel != null)
                    PreviewMode(false);
                SelectMode(false);
                break;
            case 1:
                EditMode(false);
                if (PrevPanel != null)
                    PreviewMode(true);
                SelectMode(false);
                break;
            case 2:
                EditMode(false);
                if (PrevPanel != null)
                    PreviewMode(false);
                SelectMode(true);
                break;
        }
    }
    public void EditMode(bool enabled)
    {
        EditPanel.SetActive(enabled);
        EditMenu.SetActive(enabled);
        if (robo.ani != null)
            PrevWindow.SetActive(enabled);
        if (enabled)
        {
            if (robo.ani != null)
            { 
                
                ea.selectHOD();
            }
        }
    }

    public void PreviewMode(bool enabled)
    {
        PrevPanel.SetActive(enabled);
        if (enabled)
        {
            PrevPanel.GetComponent<AniPreview>().updateList();
            PrevPanel.GetComponent<AniPreview>().setAnimation(ea.animDD.value);
        }
        else
            PrevPanel.GetComponent<AniPreview>().animator.play = false;
    }

    public void SelectMode(bool enabled)
    {
        SelectPanel.SetActive(enabled);
    }


    public void saveSettings()
    {
        string folder = SelectPanel.GetComponent<UI_SelectMech>().folder;
        Debug.Log(folder);
        StreamWriter sw = new StreamWriter("Settings.txt");
        sw.WriteLine(folder);
        sw.Close();
    }

    public void loadSettings()
    {
        if (File.Exists("Settings.txt"))
        {
            StreamReader sr = new StreamReader("Settings.txt");
            string folder = sr.ReadLine();
            folderLoc.text = folder;
            sr.Close();
        }
    }
}
