using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class OBJLoader
{
    public List<Vector3> vertexList = new List<Vector3>();
    public List<Vector3> normalsList = new List<Vector3>();
    public List<Vector2> uvList = new List<Vector2>();
    public List<OBJShape> shapeList = new List<OBJShape>();
    public OBJLoader(string path)
    {
        using (StreamReader sr = new StreamReader($"{Directory.GetCurrentDirectory()}\\{path}", System.Text.Encoding.Default))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = Regex.Replace(line, "  ", " ");
                string[] command = line.Split(' ');
                switch (command[0])
                {
                    case "vt":
                        uvList.Add(new Vector2(
                            (float)double.Parse(command[1]),
                            (float)double.Parse(command[2])));
                        break;
                    case "v":
                        vertexList.Add(new Vector3(
                            (float)double.Parse(command[1]), 
                            (float)double.Parse(command[2]),
                            (float)double.Parse(command[3])));
                        break;
                    case "f":
                        OBJShape shape = new OBJShape();
                        foreach (var i in command) {
                            if (i != "f" && i != "")
                            {
                                if (i.Contains("/"))
                                {
                                    string[] slash = i.Split('/');
                                    if (slash.Length == 3)
                                    {
                                        shape.IndeciesVertex.Add(Int32.Parse(slash[0]) - 1);
                                        shape.IndeciesUV.Add(Int32.Parse(slash[1]) - 1);
                                        shape.IndeciesNormal.Add(Int32.Parse(slash[2]) - 1);
                                    }
                                    else //2, so it's vert and uv
                                    {
                                        shape.IndeciesVertex.Add(Int32.Parse(slash[0]) - 1);
                                        shape.IndeciesUV.Add(Int32.Parse(slash[1]) - 1);
                                        shape.IndeciesNormal.Add(0);
                                    }

                                }
                                else
                                {
                                    shape.IndeciesVertex.Add(Int32.Parse(i) - 1);
                                    shape.IndeciesUV.Add(0);
                                    shape.IndeciesNormal.Add(0);
                                }
                            }
                        }
                        if (command.Length > 6)
                        {
                            Debug.Log(line);
                        }
                        shapeList.Add(shape);
                        break;
                    case "vn":
                        normalsList.Add(new Vector3(
                            (float)double.Parse(command[1]), 
                            (float)double.Parse(command[2]),
                            (float)double.Parse(command[3])));
                        break;
                    case "dm":
                        Debug.Log("Unfinished dm " + line);
                        break;
                    case "sm":
                        Debug.Log("Unfinished sm " + line);
                        break;
                    case "em":
                        Debug.Log("Unfinished em " + line);
                        break;
                }
            }
        }
        
    }
}
