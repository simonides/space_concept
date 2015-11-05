using UnityEngine;
using System.Collections;
using System.IO;

namespace Custom
{
	namespace Utility{
		namespace DAL{
		    public class DALSaveGame
		    {
		       
		        private static string location = Application.persistentDataPath;
		        private static string path;


		        public DALSaveGame()
		        {
		        }

				private static string CombinePath(string fileName)
		        {
					return path = System.IO.Path.Combine(location, fileName);
		        }
                private static string CombinePath(string dir, string fileName)
                {
                    return path = System.IO.Path.Combine(dir, fileName);
                }

				public static void WriteFile(string fileName, string input)
				{	
					string target;
					target = CombinePath(fileName);
		            using (StreamWriter sw = new StreamWriter(path))
		            {
		                sw.Write(input);
		            }
		        }

		        //returns the file as string
		        public static string GetFile(string fileName)
		        {
					string target;
					target = CombinePath(fileName);
		            string temp = File.ReadAllText(path);
		            return temp;
		        }
				public static string GetFilePath (string fileName){
					string target;
					target = CombinePath(fileName);
                    //Debug.Log("DAL: path: " + target);
					return target;
				}
                public static string GetFilePath(string directory, string fileName)
                {
                    string target;
                    target = CombinePath(directory);
                    try
                    {
                        if (!Directory.Exists(target))
                        {
                            DirectoryInfo di = Directory.CreateDirectory(target);
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log("The process failed: "+ e.ToString());
                    } 
                    target = CombinePath(target, fileName);
                    //Debug.Log("DAL: path: " + target);
                    return target;
                }


				public static FileStream GetFilestreamWrite(string fileName){
					string target;
					target = CombinePath(fileName);
					FileStream fs;
					fs = File.Create (target);
					
					return fs;
				}
				public static FileStream GetFilestreamRead(string fileName){
					string target;
					target = CombinePath(fileName);
					FileStream fs;
					fs= File.Open (target, FileMode.Open);

					return fs;
				}
		    }
		}
	}
}