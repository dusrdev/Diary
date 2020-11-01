using System.IO; //Make IO operation like writing, reading and creating files
using System.Runtime.Serialization.Formatters.Binary; //Saving to binary files
using System.Xml.Serialization; //Export only to xml on user request
using System.Security.Cryptography; //Encrypt streams

public class MySerializer {
    public string AesKey { get; private set; }
    private Diary.Logic.Aes AesProvider { get; set; }

    /// <summary>
    /// Create new instance of the class with an encryption key.
    /// </summary>
    /// <param name="encryptionKey"></param>
    public MySerializer(string encryptionKey) {
        AesKey = encryptionKey;
        AesProvider = new Diary.Logic.Aes(AesKey);
    }

    /// <summary>
    /// Serialize object to binary file
    /// </summary>
    /// <typeparam name="T">type of data -> must be serializable</typeparam>
    /// <param name="data">data object</param>
    /// <param name="fileName">filename -> excluding extension</param>
    /// <param name="path">default will be exe directory</param>
    public void SerializeToFile<T>(T data, string fileName, string path = "") {
        using (var stream = File.Open($"{path + fileName}.dat", FileMode.Create, FileAccess.ReadWrite)) {
            using (CryptoStream cryptoStream = AesProvider.CreateEncryptionStream(stream)) {
                var bf = new BinaryFormatter();
                bf.Serialize(cryptoStream, data);
            }
        }
    }

    /// <summary>
    /// Deserialize object from binary file
    /// </summary>
    /// <typeparam name="T">Type of data -> must be serializable</typeparam>
    /// <param name="fileName">file -> excluding path</param>
    /// <param name="path">default will be exe directory</param>
    /// <returns>the data deserialized from the file</returns>
    public T DeserializeFromFile<T>(string fileName, string path = "") {
        object data;
        using (var stream = File.Open($"{path + fileName}.dat", FileMode.Open, FileAccess.Read)) {
            using (CryptoStream cryptoStream = AesProvider.CreateDecryptionStream(stream)) {
                var bf = new BinaryFormatter();
                data = bf.Deserialize(cryptoStream);
            }
        }
        return (T)data;
    }

    /// <summary>
    /// Serializes object as xml file.
    /// Used for exporting data rather then read/write.
    /// Only activated when user needs to see the variable/database
    /// </summary>
    /// <typeparam name="T">Type of the data object -> must be serializable</typeparam>
    /// <param name="data">data object</param>
    /// <param name="fileName">filename -> excluding extension</param>
    /// <param name="path">path -> default will be in exe directory</param>
    public void SerializeToXmlFile<T>(T data, string fileName, string path = "") {
        using (TextWriter writer = new StreamWriter($"{path + fileName}.xml")) {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(writer, data);
        }
    }
}