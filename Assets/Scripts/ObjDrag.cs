using UnityEngine;
using System.Collections;

public class ObjDrag : MonoBehaviour
{
    float distance;
    bool materialChanged;
    public Material selectedMaterial;
    public Material unSelectedMaterial;
    void Awake()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
    }
    void Update()
    {
        if(Input.GetMouseButtonUp(0) && materialChanged)
        {
            GetComponent<Renderer>().material = unSelectedMaterial;
            materialChanged = false;
        }
    }
    void OnMouseDrag()
    {
        if(!materialChanged)
        {
            GetComponent<Renderer>().material = selectedMaterial;
            materialChanged = true;
        }
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPosition = new Vector3(Camera.main.ScreenToWorldPoint(mousePosition).x, 0, Camera.main.ScreenToWorldPoint(mousePosition).z);
        transform.position = objPosition;
    }
}
