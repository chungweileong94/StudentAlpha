using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace StudentAlpha.Services
{
    public abstract class FileService
    {
        public static async Task WriteDataToLocalStorageAsync(string fileName, string content)
        {
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, content);
        }

        public static async Task<string> ReadDataFromLocalStorageAsync(string fileName)
        {
            string result = null;
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            StorageFile file = await folder.GetFileAsync(fileName);
            result = await FileIO.ReadTextAsync(file);
            return result;
        }
    }
}
