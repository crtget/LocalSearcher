using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace LocalSearcher
{

	public class Everything
	{
		const int EVERYTHING_OK = 0;
		const int EVERYTHING_ERROR_MEMORY = 1;
		const int EVERYTHING_ERROR_IPC = 2;
		const int EVERYTHING_ERROR_REGISTERCLASSEX = 3;
		const int EVERYTHING_ERROR_CREATEWINDOW = 4;
		const int EVERYTHING_ERROR_CREATETHREAD = 5;
		const int EVERYTHING_ERROR_INVALIDINDEX = 6;
		const int EVERYTHING_ERROR_INVALIDCALL = 7;

		const int EVERYTHING_REQUEST_FILE_NAME = 0x00000001;
		const int EVERYTHING_REQUEST_PATH = 0x00000002;
		const int EVERYTHING_REQUEST_FULL_PATH_AND_FILE_NAME = 0x00000004;
		const int EVERYTHING_REQUEST_EXTENSION = 0x00000008;
		const int EVERYTHING_REQUEST_SIZE = 0x00000010;
		const int EVERYTHING_REQUEST_DATE_CREATED = 0x00000020;
		const int EVERYTHING_REQUEST_DATE_MODIFIED = 0x00000040;
		const int EVERYTHING_REQUEST_DATE_ACCESSED = 0x00000080;
		const int EVERYTHING_REQUEST_ATTRIBUTES = 0x00000100;
		const int EVERYTHING_REQUEST_FILE_LIST_FILE_NAME = 0x00000200;
		const int EVERYTHING_REQUEST_RUN_COUNT = 0x00000400;
		const int EVERYTHING_REQUEST_DATE_RUN = 0x00000800;
		const int EVERYTHING_REQUEST_DATE_RECENTLY_CHANGED = 0x00001000;
		const int EVERYTHING_REQUEST_HIGHLIGHTED_FILE_NAME = 0x00002000;
		const int EVERYTHING_REQUEST_HIGHLIGHTED_PATH = 0x00004000;
		const int EVERYTHING_REQUEST_HIGHLIGHTED_FULL_PATH_AND_FILE_NAME = 0x00008000;

		const int EVERYTHING_SORT_NAME_ASCENDING = 1;
		const int EVERYTHING_SORT_NAME_DESCENDING = 2;
		const int EVERYTHING_SORT_PATH_ASCENDING = 3;
		const int EVERYTHING_SORT_PATH_DESCENDING = 4;
		const int EVERYTHING_SORT_SIZE_ASCENDING = 5;
		const int EVERYTHING_SORT_SIZE_DESCENDING = 6;
		const int EVERYTHING_SORT_EXTENSION_ASCENDING = 7;
		const int EVERYTHING_SORT_EXTENSION_DESCENDING = 8;
		const int EVERYTHING_SORT_TYPE_NAME_ASCENDING = 9;
		const int EVERYTHING_SORT_TYPE_NAME_DESCENDING = 10;
		const int EVERYTHING_SORT_DATE_CREATED_ASCENDING = 11;
		const int EVERYTHING_SORT_DATE_CREATED_DESCENDING = 12;
		const int EVERYTHING_SORT_DATE_MODIFIED_ASCENDING = 13;
		const int EVERYTHING_SORT_DATE_MODIFIED_DESCENDING = 14;
		const int EVERYTHING_SORT_ATTRIBUTES_ASCENDING = 15;
		const int EVERYTHING_SORT_ATTRIBUTES_DESCENDING = 16;
		const int EVERYTHING_SORT_FILE_LIST_FILENAME_ASCENDING = 17;
		const int EVERYTHING_SORT_FILE_LIST_FILENAME_DESCENDING = 18;
		const int EVERYTHING_SORT_RUN_COUNT_ASCENDING = 19;
		const int EVERYTHING_SORT_RUN_COUNT_DESCENDING = 20;
		const int EVERYTHING_SORT_DATE_RECENTLY_CHANGED_ASCENDING = 21;
		const int EVERYTHING_SORT_DATE_RECENTLY_CHANGED_DESCENDING = 22;
		const int EVERYTHING_SORT_DATE_ACCESSED_ASCENDING = 23;
		const int EVERYTHING_SORT_DATE_ACCESSED_DESCENDING = 24;
		const int EVERYTHING_SORT_DATE_RUN_ASCENDING = 25;
		const int EVERYTHING_SORT_DATE_RUN_DESCENDING = 26;

		const int EVERYTHING_TARGET_MACHINE_X86 = 1;
		const int EVERYTHING_TARGET_MACHINE_X64 = 2;
		const int EVERYTHING_TARGET_MACHINE_ARM = 3;


#if X64
		public const string DLL_FILE_NAME = "Everything64.dll";
#else
		public const string DLL_FILE_NAME = "Everything32.dll";
#endif

		[DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode)]
		public static extern int Everything_SetSearch(string lpSearchString);
		[DllImport(DLL_FILE_NAME)]
		public static extern void Everything_SetMatchPath(bool bEnable);
		[DllImport(DLL_FILE_NAME)]
		public static extern void Everything_SetMatchCase(bool bEnable);
		[DllImport(DLL_FILE_NAME)]
		public static extern void Everything_SetMatchWholeWord(bool bEnable);
		[DllImport(DLL_FILE_NAME)]
		public static extern void Everything_SetRegex(bool bEnable);
		[DllImport(DLL_FILE_NAME)]
		public static extern void Everything_SetMax(UInt32 dwMax);
		[DllImport(DLL_FILE_NAME)]
		public static extern void Everything_SetOffset(UInt32 dwOffset);

		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_GetMatchPath();
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_GetMatchCase();
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_GetMatchWholeWord();
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_GetRegex();
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetMax();
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetOffset();
		[DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode)]
		public static extern string Everything_GetSearch();
		[DllImport(DLL_FILE_NAME)]
		public static extern int Everything_GetLastError();

		[DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode)]
		public static extern bool Everything_Query(bool bWait);

		[DllImport(DLL_FILE_NAME)]
		public static extern void Everything_SortResultsByPath();

		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetNumFileResults();
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetNumFolderResults();
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetNumResults();
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetTotFileResults();
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetTotFolderResults();
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetTotResults();
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_IsVolumeResult(UInt32 nIndex);
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_IsFolderResult(UInt32 nIndex);
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_IsFileResult(UInt32 nIndex);
		[DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode)]
		public static extern void Everything_GetResultFullPathName(UInt32 nIndex, StringBuilder lpString, UInt32 nMaxCount);
		[DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode)]
		public static extern string Everything_GetResultPath(UInt32 nIndex);
		[DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode)]
		public static extern string Everything_GetResultFileName(UInt32 nIndex);

		[DllImport(DLL_FILE_NAME)]
		public static extern void Everything_Reset();
		[DllImport(DLL_FILE_NAME)]
		public static extern void Everything_CleanUp();
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetMajorVersion();
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetMinorVersion();
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetRevision();
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetBuildNumber();
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_Exit();
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_IsDBLoaded();
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_IsAdmin();
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_IsAppData();
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_RebuildDB();
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_UpdateAllFolderIndexes();
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_SaveDB();
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_SaveRunHistory();
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_DeleteRunHistory();
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetTargetMachine();

		// Everything 1.4
		[DllImport(DLL_FILE_NAME)]
		public static extern void Everything_SetSort(UInt32 dwSortType);
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetSort();
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetResultListSort();
		[DllImport(DLL_FILE_NAME)]
		public static extern void Everything_SetRequestFlags(UInt32 dwRequestFlags);
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetRequestFlags();
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetResultListRequestFlags();
		[DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode)]
		public static extern string Everything_GetResultExtension(UInt32 nIndex);
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_GetResultSize(UInt32 nIndex, out long lpFileSize);
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_GetResultDateCreated(UInt32 nIndex, out long lpFileTime);
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_GetResultDateModified(UInt32 nIndex, out long lpFileTime);
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_GetResultDateAccessed(UInt32 nIndex, out long lpFileTime);
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetResultAttributes(UInt32 nIndex);
		[DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode)]
		public static extern string Everything_GetResultFileListFileName(UInt32 nIndex);
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetResultRunCount(UInt32 nIndex);
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_GetResultDateRun(UInt32 nIndex, out long lpFileTime);
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_GetResultDateRecentlyChanged(UInt32 nIndex, out long lpFileTime);
		[DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode)]
		public static extern string Everything_GetResultHighlightedFileName(UInt32 nIndex);
		[DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode)]
		public static extern string Everything_GetResultHighlightedPath(UInt32 nIndex);
		[DllImport(DLL_FILE_NAME, CharSet = CharSet.Unicode)]
		public static extern string Everything_GetResultHighlightedFullPathAndFileName(UInt32 nIndex);
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_GetRunCountFromFileName(string lpFileName);
		[DllImport(DLL_FILE_NAME)]
		public static extern bool Everything_SetRunCountFromFileName(string lpFileName, UInt32 dwRunCount);
		[DllImport(DLL_FILE_NAME)]
		public static extern UInt32 Everything_IncRunCountFromFileName(string lpFileName);


		public class FileInfo
		{
			public string FileName;
			public string FullPathName;
			public long FileSize;
			public FileInfo(string fullpathname, long filesize)
			{
				this.FileName = Path.GetFileName(fullpathname);
				this.FullPathName = fullpathname;
				this.FileSize = filesize;
			}
		}


		public static FileInfo[] GetFiles(string word)
		{
			UInt32 index;
			const int bufsize = 260;
			StringBuilder buf = new StringBuilder(bufsize);

			Everything_SetSearch(word);
			Everything_SetRequestFlags(EVERYTHING_REQUEST_FILE_NAME | EVERYTHING_REQUEST_PATH | EVERYTHING_REQUEST_SIZE);
			Everything_SetSort(EVERYTHING_SORT_NAME_ASCENDING);
			Everything_Query(true);

			List<FileInfo> result = new List<FileInfo>();

			for (index = 0; index < Everything_GetNumResults(); index++)
			{
				long filesize;
				Everything_GetResultFullPathName(index, buf, bufsize);
				Everything_GetResultSize(index, out filesize);
				result.Add(new FileInfo(buf.ToString(), filesize));
			}


			return result.ToArray();
		}



	}
}
