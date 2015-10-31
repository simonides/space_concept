using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Custom.Utility.DAL;
//using Custom.Utility;
using System.Runtime.Serialization;
namespace Custom
{
    namespace Utility
    {
        public class SaveFileSerializer
        {


            public SaveFileSerializer() { }
            /// <summary>
            /// Pass down a DataClass of type <T> to save it.
            /// </summary>
            /// <typeparam name="T">Type of Dataclass</typeparam>
            /// <param name="dataClass">object to save</param>
            /// <param name="fileName">name of the file</param>
            /// 
            /// <returns>returns True if no exception occured</returns>
            public static bool Save<T>(T dataClass, string fileName)
            {

                try
                {

                    using (FileStream fs = File.Create(DALSaveGame.GetFilePath(fileName)))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        SurrogateSelector ss = new SurrogateSelector();
                        StreamingContext sc = new StreamingContext(StreamingContextStates.All);
                        ss.AddSurrogate(typeof(Vector3), sc, new Vector3Surrogate());
                        ss.AddSurrogate(typeof(Quaternion), sc, new QuaternionSurrogate());
                        bf.SurrogateSelector = ss;
                        bf.Serialize(fs, dataClass);
                    }
                }
                catch (IOException e)
                {
                    Debug.LogError(e.ToString());
                }
                catch (SerializationException e)
                {
                    Debug.LogError(e.ToString());

                }
                finally
                {

                }
                return true;
            }

            /// <summary>
            /// Load a file of a given Type <T>
            /// </summary>
            /// <typeparam name="T">DataClass type</typeparam>
            /// <param name="fileName">name of the File</param>
            /// <returns>returns a object of T if file existed and no errors occured.
            /// OR returns null if there was an error or file does not exist
            /// </returns>
            public static T Load<T>(string fileName)
            where T : class, new()
            {
                T myFile = null;
                try
                {
                    if (File.Exists(DALSaveGame.GetFilePath(fileName)))
                    {
                        using (FileStream fs = File.OpenRead(DALSaveGame.GetFilePath(fileName)))
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            SurrogateSelector ss = new SurrogateSelector();
                            StreamingContext sc = new StreamingContext(StreamingContextStates.All);
                            ss.AddSurrogate(typeof(Vector3), sc, new Vector3Surrogate());
                            ss.AddSurrogate(typeof(Quaternion), sc, new QuaternionSurrogate());
                            bf.SurrogateSelector = ss;
                            myFile = (T)bf.Deserialize(fs);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (FileNotFoundException e)
                {
                    return null;
                }
                catch (IOException e)
                {
                    Debug.LogError(e.ToString());
                    return null;
                }
                catch (SerializationException e)
                {
                    Debug.LogError(e.ToString());
                    return null;
                }

                return myFile;
            }
        }
    }
}