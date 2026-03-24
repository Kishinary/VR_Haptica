using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class RunTIme_Debuger : MonoBehaviour
{
    public TextMeshPro debug_text;
    public List<GameObject> fingers;
    public Material default_material;
    public Material highlight_material;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnDebug(string message)
    {
        float y_offset = transform.childCount * 0.25f;
        Vector3 local_spawn_position = new Vector3(0f, y_offset, 0f);

        TextMeshPro text_debuger = Instantiate(debug_text, transform.position + transform.rotation * local_spawn_position, transform.rotation, this.transform);
        text_debuger.text = message;
    }

    public void Colorchanger(string fingername, int intensity)
    {
        for (int i = 0; i < fingers.Count; i++)
        {
            if (fingername == fingers[i].name)
            {
                if (intensity == 0)
                {
                    fingers[i].GetComponent<Renderer>().material.color = Color.white;
                }
                else
                {
                    fingers[i].GetComponent<Renderer>().material.color = Color.blue;
                }
            }
        }
    }
}
