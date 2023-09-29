using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;



public class Log : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            //Open the File
            StreamWriter sw = new StreamWriter("C:\\Test1.txt", true, Encoding.ASCII);

            

            //close the file
            sw.Close();
        }
        catch(Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        finally
        {
            Console.WriteLine("Executing finally block.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

