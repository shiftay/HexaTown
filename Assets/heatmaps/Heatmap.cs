// Alan Zucconi
// www.alanzucconi.com
using UnityEngine;
using System.Collections;

public class Heatmap : MonoBehaviour
{
    float[] odd = { -4.5f, -3.3f, -2.25f, -1.2f, -0.15f, 0.95f, 2.05f, 3.15f, 4.25f };
    float[] even = { -3.9f, -2.8f, -1.7f, -0.6f, 0.45f, 1.5f, 2.6f, 3.7f, 4.8f };
    float[] row = { 3.1f, 2.1f, 1.1f, 0.15f, -0.8f, -1.75f };

    public Vector4[] positions;
    public float[] radiuses;
    public float[] intensities;

    public Material material;
    public float intense;
    public float rad;

    public int count = 50;

    public void Setup(GridController gc) {
        positions = new Vector4[gc.INDX.Count];
        radiuses = new float[gc.INDX.Count];
        intensities = new float[gc.INDX.Count];

        for(int i = 0; i < gc.INDX.Count; i++) {
            if(gc.INDX[i] % 2 == 0) {
                positions[i] = new Vector4(odd[gc.INDY[i]], row[gc.INDX[i]], 0, 0);
            } else {
                positions[i] = new Vector4(even[gc.INDY[i]], row[gc.INDX[i]], 0, 0);
            }
            
            radiuses[i] = 3f;
            intensities[i] = 1.5f;
        } 
        

        Vector4[] properties = new Vector4[positions.Length];
        material.SetInt("_Points_Length", positions.Length);
       
        material.SetFloatArray("_Intensity", intensities);
        material.SetFloatArray("_Radius", radiuses);
        material.SetVectorArray("_Points", positions);

    }

    void Update()
    {

        // for(int i = 0; i < positions.Length; i++) {

            
        //     radiuses[i] = rad;
        //     intensities[i] = intense;
        // } 
        
        // material.SetFloatArray("_Intensity", intensities);
        // material.SetFloatArray("_Radius", radiuses);

        // Vector4[] properties = new Vector4[positions.Length];
        // material.SetInt("_Points_Length", positions.Length);

        // material.SetVectorArray("_Points", positions);
    }
}