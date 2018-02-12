using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System;
using System.Xml;
using System.IO;


public class ImportStadsmodell : AssetPostprocessor
{    
    // Root origin values
    public static double xRoot = 149729.914346;
    public static double yRoot = 6579336.14918;
    public static double zRoot = 28.6472087827;

    public Material OnAssignMaterialModel(Material material, Renderer renderer)
    {
        string assetFolder = Path.GetDirectoryName(assetPath);
        string materialFolder = assetFolder + "/Materials";
        if (!Directory.Exists(materialFolder))
        {
            Directory.CreateDirectory(materialFolder);
        }

        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(assetPath);

        //Material m = new Material(Shader.Find("Unlit/Texture"));
        Material m = new Material(material);
        string textureName = assetFolder + "/" + fileNameWithoutExtension + "_0.jpg";
        Texture2D t = (Texture2D)AssetDatabase.LoadAssetAtPath(textureName, typeof(Texture2D));
        m.mainTexture = t;

        string materialName = materialFolder + "/" + fileNameWithoutExtension + ".mat";

        AssetDatabase.CreateAsset(m, materialName);

        return m;
    }

    void OnPostprocessModel(GameObject g)
    {
        string assetFolder = Path.GetDirectoryName(assetPath);

        //if (assetFolder == "Assets/scenes/IES/Stadsmodell")
        if (assetFolder.Contains( "Assets/Models/Maps") )
        {

            //Renderer rend = g.GetComponent<Renderer>();

            ////rend.receiveShadows = false;

            Debug.Log("name: " + Path.GetFileNameWithoutExtension(assetPath));
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(assetPath);

            //string textureName = assetFolder + "/" + fileNameWithoutExtension + "_0.jpg";
            //Texture2D t = (Texture2D)AssetDatabase.LoadAssetAtPath(textureName, typeof(Texture2D));
            ////rend.sharedMaterial.name = fileNameWithoutExtension;
            //rend.material.mainTexture = t;

            // XML
            string xmlFileName = assetFolder + "/" + fileNameWithoutExtension + ".xml";
            TextAsset xmlFile = (TextAsset)AssetDatabase.LoadAssetAtPath(xmlFileName, typeof(TextAsset));
            if (xmlFile == null)
            {
                Debug.Log("Couldn't find textasset : " + xmlFileName);
            }
            XmlDocument xmlDoc = loadXml(xmlFile);

            XmlNodeList originNodes = xmlDoc.GetElementsByTagName("Origin");

            float x = 0f;
            float y = 0f;
            float z = 0f;

            foreach (XmlNode originNode in originNodes)
            {
                //Debug.Log("Origin info: " + originInfo);
                XmlNodeList axisNodes = originNode.ChildNodes;
                foreach (XmlNode axis in axisNodes)
                {
                    if (axis.Name == "x")
                    {
                        //Debug.Log("X value: " + axis.InnerText);
                        double v = 0.0;
                        bool parseOk = Double.TryParse(axis.InnerText, out v);
                        if (parseOk)
                        {
                            x = (float)(v - xRoot);
                        }
                        else
                        {
                            Debug.Log("Couldn't parse value " + axis.InnerText);
                        }
                    }
                    if (axis.Name == "y")
                    {
                        //Debug.Log("X value: " + axis.InnerText);
                        double v = 0.0;
                        bool parseOk = Double.TryParse(axis.InnerText, out v);
                        if (parseOk)
                        {
                            y = (float)(v - yRoot);
                        }
                        else
                        {
                            Debug.Log("Couldn't parse value " + axis.InnerText);
                        }
                    }
                    if (axis.Name == "z")
                    {
                        //Debug.Log("X value: " + axis.InnerText);
                        double v = 0.0;
                        bool parseOk = Double.TryParse(axis.InnerText, out v);
                        if (parseOk)
                        {
                            z = (float)(zRoot - v);
                            //z = (float)(v);
                        }
                        else
                        {
                            Debug.Log("Couldn't parse value " + axis.InnerText);
                        }
                    }
                }
            }

            g.transform.localPosition = new Vector3(x, y, z);
            g.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        }
    }

    XmlDocument loadXml(TextAsset xmlFile)
    {
        MemoryStream assetStream = new MemoryStream(xmlFile.bytes);
        XmlReader reader = XmlReader.Create(assetStream);
        XmlDocument xmlDoc = new XmlDocument();
        try
        {
            xmlDoc.Load(reader);
        }
        catch (Exception ex)
        {
            Debug.Log("Error loading " + xmlFile.name + ":\n" + ex);
        }
        finally
        {
            Debug.Log(xmlFile.name + " loaded");
        }

        return xmlDoc;
    }
}
