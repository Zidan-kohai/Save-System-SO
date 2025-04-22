using Infrastructure.Data.PersistantData.Helper.Path;
using SFB;
using Sirenix.OdinInspector;
using System;
using System.IO;
using UnityEngine;

namespace Infrastructure.PersistantData.Helper.Path
{
    [CreateAssetMenu(fileName = "LOCLA_FILE_PATH", menuName = "SO/Data/Helper/Path/Local File")]
    public class LocalFilePath : ScriptableObject, IDataPath
    {
        [field:SerializeField, ReadOnly] public string Path { get; protected set; }
        [field: SerializeField, ReadOnly] public string fileName { get; protected set; }
        [field: SerializeField, ReadOnly] public string folderPath { get; protected set; }

        [Button]
        private void ChooseFile()
        {
            var paths = StandaloneFileBrowser.OpenFolderPanel("Choose Folder", "", false);

            if (paths.Length <= 0)
                throw new Exception("You didnt choose folder");

            folderPath = paths[0];

            SetFilePath();
        }

        [Button]
        private void SetFileName(string name)
        {
            fileName = name;
            SetFilePath();
        }

        private void SetFilePath()
        {
             Path = System.IO.Path.Combine(folderPath, fileName);
        }

    }
}