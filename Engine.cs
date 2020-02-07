using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UsnOperation;

namespace QueryEngine
{
    #region FrnFilePath

    internal class FrnFilePath
    {
        private UInt64 fileReferenceNumber;

        private UInt64? parentFileReferenceNumber;

        private string fileName;

        private string path;

        public UInt64 FileReferenceNumber { get { return this.fileReferenceNumber; } }

        public UInt64? ParentFileReferenceNumber { get { return this.parentFileReferenceNumber; } }

        public string FileName { get { return this.fileName; } }

        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                this.path = value;
            }
        }

        public FrnFilePath(UInt64 fileReferenceNumber, UInt64? parentFileReferenceNumber, string fileName, string path = null)
        {
            this.fileReferenceNumber = fileReferenceNumber;
            this.parentFileReferenceNumber = parentFileReferenceNumber;
            this.fileName = fileName;
            this.path = path;
        }
    }
    #endregion
    #region FileAndDirectoryEntry
    public class FileAndDirectoryEntry
    {
        protected UInt64 fileReferenceNumber;

        protected UInt64 parentFileReferenceNumber;

        protected string fileName;

        protected bool isFolder;

        protected string path;

        public UInt64 FileReferenceNumber
        {
            get
            {
                return this.fileReferenceNumber;
            }
        }

        public UInt64 ParentFileReferenceNumber
        {
            get
            {
                return this.parentFileReferenceNumber;
            }
        }

        public string FileName
        {
            get
            {
                return this.fileName;
            }
        }

        public string Path
        {
            get
            {
                return this.path;
            }
        }

        public string FullFileName
        {
            get
            {
                return string.Concat(this.path, "\\", this.fileName);
            }
        }

        public bool IsFolder
        {
            get
            {
                return this.isFolder;
            }
        }

        public FileAndDirectoryEntry(UsnEntry usnEntry, string path)
        {
            this.fileReferenceNumber = usnEntry.FileReferenceNumber;
            this.parentFileReferenceNumber = usnEntry.ParentFileReferenceNumber;
            this.fileName = usnEntry.FileName;
            this.isFolder = usnEntry.IsFolder;
            this.path = path;
        }
    }
    #endregion
    public class Engine
    {
        /// <summary>
        /// When its values is 1407374883553285(0x5000000000005L), it means this file/folder is under drive root
        /// </summary>
        protected const UInt64 ROOT_FILE_REFERENCE_NUMBER = 0x5000000000005L;

        protected static readonly string excludeFolders = string.Join("|", 
            new string[]
            {
                "$RECYCLE.BIN",
                "System Volume Information",
                "$AttrDef",
                "$BadClus",
                "$BitMap",
                "$Boot",
                "$LogFile",
                "$Mft",
                "$MftMirr",
                "$Secure",
                "$TxfLog",
                "$UpCase",
                "$Volume",
                "$Extend"
            }).ToUpper();

        

        public static IEnumerable<DriveInfo> GetAllFixedNtfsDrives()
        {
            return DriveInfo.GetDrives()
                .Where(d => d.DriveType == DriveType.Fixed && d.DriveFormat.ToUpper() == "NTFS");
        }

        public static List<FileAndDirectoryEntry> GetAllFilesAndDirectories()
        {
            List<FileAndDirectoryEntry> result = new List<FileAndDirectoryEntry>();

            IEnumerable<DriveInfo> fixedNtfsDrives = GetAllFixedNtfsDrives();

            foreach (var drive in fixedNtfsDrives)
            {
                var usnOperator = new UsnOperator(drive);
                var usnEntries = usnOperator.GetEntries().Where(e => !excludeFolders.Contains(e.FileName.ToUpper()));

                var folders = usnEntries.Where(e => e.IsFolder).ToArray();
                List<FrnFilePath> paths = GetFolderPath(folders, drive);

                result.AddRange(usnEntries.Join(
                    paths,
                    usn => usn.ParentFileReferenceNumber,
                    path => path.FileReferenceNumber,
                    (usn, path) => new FileAndDirectoryEntry(usn, path.Path)));
            }

            return result;
        }

        private static List<FrnFilePath> GetFolderPath(UsnEntry[] folders, DriveInfo drive)
        {
            Dictionary<UInt64, FrnFilePath> pathDic = new Dictionary<ulong, FrnFilePath>();
            pathDic.Add(ROOT_FILE_REFERENCE_NUMBER,
                new FrnFilePath(ROOT_FILE_REFERENCE_NUMBER, null, string.Empty, drive.Name.TrimEnd('\\')));

            foreach (var folder in folders)
            {
                pathDic.Add(folder.FileReferenceNumber,
                    new FrnFilePath(folder.FileReferenceNumber, folder.ParentFileReferenceNumber, folder.FileName));
            }

            Stack<UInt64> treeWalkStack = new Stack<ulong>();

            foreach (var key in pathDic.Keys)
            {
                treeWalkStack.Clear();

                FrnFilePath currentValue = pathDic[key];

                if (string.IsNullOrWhiteSpace(currentValue.Path)
                    && currentValue.ParentFileReferenceNumber.HasValue
                    && pathDic.ContainsKey(currentValue.ParentFileReferenceNumber.Value))
                {
                    FrnFilePath parentValue = pathDic[currentValue.ParentFileReferenceNumber.Value];

                    while (string.IsNullOrWhiteSpace(parentValue.Path)
                        && parentValue.ParentFileReferenceNumber.HasValue
                        && pathDic.ContainsKey(parentValue.ParentFileReferenceNumber.Value))
                    {
                        currentValue = parentValue;

                        if (currentValue.ParentFileReferenceNumber.HasValue
                            && pathDic.ContainsKey(currentValue.ParentFileReferenceNumber.Value))
                        {
                            treeWalkStack.Push(key);
                            parentValue = pathDic[currentValue.ParentFileReferenceNumber.Value];
                        }
                        else
                        {
                            parentValue = null;
                            break;
                        }
                    }

                    if (parentValue != null)
                    {
                        currentValue.Path = BuildPath(currentValue, parentValue);

                        while (treeWalkStack.Count() > 0)
                        {
                            UInt64 walkedKey = treeWalkStack.Pop();

                            FrnFilePath walkedNode = pathDic[walkedKey];
                            FrnFilePath parentNode = pathDic[walkedNode.ParentFileReferenceNumber.Value];

                            walkedNode.Path = BuildPath(walkedNode, parentNode);
                        }
                    }
                }
            }

            var result = pathDic.Values.Where(p => !string.IsNullOrWhiteSpace(p.Path) && p.Path.StartsWith(drive.Name)).ToList();

            return result;
        }

        private static string BuildPath(FrnFilePath currentNode, FrnFilePath parentNode)
        {
            return string.Concat(new string[] { parentNode.Path, "\\", currentNode.FileName });
        }
    }
}
