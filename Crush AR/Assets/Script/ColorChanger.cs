using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorChanger : MonoBehaviour
{
    private TextMesh textMeshPro;
    public Color myColor;
    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = gameObject.GetComponent<TextMesh>();
        textMeshPro.color = myColor;

    }

    // Update is called once per frame
    void Update()
    {
    }
}
