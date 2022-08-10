using System.Runtime.Serialization.Json;

using static System.Console;
using static System.Environment;

namespace zsh_ultra
{
    public static class DirectoryConstant
    {
        public static readonly string ProgramFolderName = ".zshultra";
        public static readonly DirectoryInfo AppPath = new DirectoryInfo(Path.Combine(GetFolderPath(SpecialFolder.ApplicationData), ProgramFolderName));
    }

    public static class IOMRSys
    {
        public static readonly DirectoryInfo AppDirectory;

        static IOMRSys()
        {
            AppDirectory = Directory.CreateDirectory(DirectoryConstant.AppPath.FullName);
        }

        public static StreamWriter? CreateFile(string fileName, Predicate<DirectoryInfo> where)
        {
            foreach (string dir in Directory.GetDirectories(AppDirectory.FullName))
            {
                if (where(new DirectoryInfo(dir)))
                {
                    return File.CreateText(Path.Combine(dir, fileName));
                }
            }

            return null;
        }

        public static bool SearchFileAndRemove(Predicate<FileInfo> where)
        {
            foreach (string fileDir in Directory.GetDirectories(AppDirectory.FullName))
            {
                foreach (string filePath in Directory.GetFiles(fileDir))
                {
                    if (where(new FileInfo(filePath)))
                    {
                        File.Delete(filePath);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool SerializeObjectToFile<T>(T serializableObject, string fileName, Predicate<DirectoryInfo> where)
        {
            var returnValue = false;
            using (var sw = CreateFile(fileName, where))
            {
                if (sw == null) WriteLine("Stream for the specified file was null. File creating was cancelled.");
                else
                {
                    var temp = new DataContractJsonSerializer(typeof(T));

                    if (temp == null) WriteLine("Can't generate serializer.");
                    else
                    {
                        temp.WriteObject(sw.BaseStream, serializableObject);
                        returnValue = true;
                    }
                }
            }

            return returnValue;
        }

        public static T? SearchAndDeserializeFileToObject<T>(Predicate<FileInfo> where)
        {
            var returnValue = default(T?);
            foreach (string fileDir in Directory.GetDirectories(AppDirectory.FullName))
            {
                foreach (string filePath in Directory.GetFiles(fileDir))
                {
                    if (where(new FileInfo(filePath)))
                    {
                        returnValue = DeserializeFileToObject(returnValue, filePath);
                    }
                }
            }

            return returnValue;

            static T? DeserializeFileToObject<T>(T? returnValue, string filePath)
            {
                using (var sr = File.OpenText(filePath))
                {
                    if (sr == null) WriteLine("Stream for the specified file was null. File opening was cancelled.");
                    else
                    {
                        var temp = new DataContractJsonSerializer(typeof(T));

                        if (temp == null) WriteLine("Can't generate serializer.");
                        else
                        {
                            var tempObject = temp.ReadObject(sr.BaseStream);

                            if (tempObject == null) WriteLine("Deserialized object for the specified file was null. File deserialization was cancelled.");
                            else
                            {
                                if (tempObject is T parsed)
                                {
                                    returnValue = parsed;
                                }
                            }
                        }
                    }
                }

                return returnValue;
            }
        }
    }
}
