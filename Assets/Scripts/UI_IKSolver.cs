using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_IKSolver : MonoBehaviour
{
    public RoboStructure robo;
    public UI_EditParts ep;

    [Header("IK Chain")]
    public bool isActive;
    public GameObject[] chain;
    public float distAB = 0;
    public float distBC = 0;
    public float _Angle = 0;
    public Vector3 Offset = new Vector3(0, 0, 0);
    bool _flip = false;




    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

        if (isActive && chain != null)
        {
            solve(chain[0], chain[1], chain[2], distAB, distBC, Offset, _Angle, _flip);
            robo.updateHod(ep.ea.animDD.value, ep.ea.hodDD.value);
            ep.TransformTextUpdate();
        }
    }

    public void setChain()
    {
        chain = new GameObject[3];
        chain[2] = robo.parts[ep.index];
        chain[1] = chain[2].transform.parent.gameObject;
        chain[0] = chain[1].transform.parent.gameObject;
        distAB = Vector3.Distance(chain[0].transform.position, chain[1].transform.position);
        distBC = Vector3.Distance(chain[1].transform.position, chain[2].transform.position + Offset);
    }

    public void setActive(bool value)
    {
        isActive = value;
    }

    public void setPoleAngle(float value)
    {
        _Angle = value;
    }

    public void setFlip(bool value)
    {
        _flip = value;
    }
    void solve(GameObject jointA, GameObject jointB, GameObject jointC, float distAB, float distBC, Vector3 offset, float poleAngle, bool flip = false)
    {
        float distAC = Vector3.Distance(jointA.transform.position, jointC.transform.position + offset);
        
        if (distAC < distAB + distBC)
        {
            Vector3 positionC = jointC.transform.position;
            Quaternion rotationC = jointC.transform.rotation;
            jointA.transform.LookAt(positionC);
            jointA.transform.Rotate(new Vector3(-90, 0, 0), Space.Self);
            jointB.transform.localRotation = Quaternion.Euler(0, 0, 0);
            float cos = (Mathf.Pow(distAB, 2) + Mathf.Pow(distAC, 2) - Mathf.Pow(distBC, 2)) / (2 * distAB * distAC);
            float angle = Mathf.Acos(cos) * Mathf.Rad2Deg;
            if (flip)
                jointA.transform.Rotate(new Vector3(angle, 0, 0), Space.Self);
            else
                jointA.transform.Rotate(new Vector3(-angle, 0, 0), Space.Self);
            cos = (Mathf.Pow(distAB, 2) + Mathf.Pow(distBC, 2) - Mathf.Pow(distAC, 2)) / (2 * distAB * distBC);
            angle = Mathf.Acos(cos) * Mathf.Rad2Deg;
            if (flip)
                jointB.transform.localRotation = Quaternion.Euler(new Vector3(180 + angle, 0, 0));
            else
                jointB.transform.localRotation = Quaternion.Euler(new Vector3(180 - angle, 0, 0));
            jointA.transform.RotateAround(jointA.transform.position, (positionC + offset - jointA.transform.position).normalized, poleAngle);
            jointC.transform.position = positionC;
            jointC.transform.rotation = rotationC;
        }
    }
}
