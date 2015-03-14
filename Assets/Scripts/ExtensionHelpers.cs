using UnityEngine;
using System.Collections;


public static class ExtensionHelpers
{
    public static Vector2 toVec2(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

    public static Vector3 toVec3(this Vector2 vec, float z = 0.0f)
    {
        return new Vector3(vec.x, vec.y, z);
    }

    public static void DebugPrint(this Vector2[] array, string str)
    {
        string strToPrint = str + ": ";
        foreach(Vector2 vec in array)
        {
            strToPrint += "[" + vec.x + ", " + vec.y + "]  ";
        }
        Debug.Log(strToPrint);
    }


    public static void DebugPrint(this Vector3[] array, string str, bool is3D=true)
    {
        string strToPrint = str + ": ";
        if (is3D)
        {
            foreach(Vector3 vec in array)
            {
                strToPrint += "[" + vec.x + ", " + vec.y + ", " + vec.z + "]  ";
            }
        }
        else
        {
            foreach(Vector3 vec in array)
            {
                strToPrint += "[" + vec.x + ", " + vec.y + "]  ";
            }    
        }
        Debug.Log(strToPrint);
    }


//    public static void DebugPrint(this Camera cam )
//    {
//        Camera cam = Camera.main;
//        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight, transform.position.z));
//        Vector3 botLeft  = cam.ScreenToWorldPoint(new Vector3(0, 0, transform.position.z));
//        Debug.Log("topRight: " + topRight);
//        Debug.Log("botLeft: " + botLeft);
//        collider.size  = botLeft - topRight;
//    }
}
