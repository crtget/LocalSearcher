using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;


namespace UsnOperation
{

    #region UsnEntry
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class UsnEntry
    {
        public UInt32 RecordLength { get; private set; }
        public UInt64 FileReferenceNumber { get; private set; }

        /// <summary>
        /// Gets the parent file reference number.
        /// When its values is 1407374883553285(0x5000000000005L), it means this file/folder is under drive root
        /// </summary>
        /// <value>
        /// The parent file reference number.
        /// </value>
        public UInt64 ParentFileReferenceNumber { get; private set; }
        public Int64 Usn { get; private set; }
        public UInt32 Reason { get; private set; }
        public UInt32 FileAttributes { get; private set; }
        public Int32 FileNameLength { get; private set; }
        public Int32 FileNameOffset { get; private set; }
        public string FileName { get; private set; }
        public bool IsFolder
        {
            get
            {
                return (this.FileAttributes & Win32ApiConstant.FILE_ATTRIBUTE_DIRECTORY) != 0;
            }
        }

        public UsnEntry(USN_RECORD_V2 usnRecord)
        {
            this.RecordLength = usnRecord.RecordLength;
            this.FileReferenceNumber = usnRecord.FileReferenceNumber;
            this.ParentFileReferenceNumber = usnRecord.ParentFileReferenceNumber;
            this.Usn = usnRecord.Usn;
            this.Reason = usnRecord.Reason;
            this.FileAttributes = usnRecord.FileAttributes;
            this.FileNameLength = usnRecord.FileNameLength;
            this.FileNameOffset = usnRecord.FileNameOffset;
            this.FileName = usnRecord.FileName;
        }
    }
    #endregion
    #region UsnJournalData
    public class UsnJournalData
    {
        public DriveInfo Drive { get; private set; }
        public UInt64 UsnJournalID { get; private set; }
        public Int64 FirstUsn { get; private set; }
        public Int64 NextUsn { get; private set; }
        public Int64 LowestValidUsn { get; private set; }
        public Int64 MaxUsn { get; private set; }
        public UInt64 MaximumSize { get; private set; }
        public UInt64 AllocationDelta { get; private set; }

        public UsnJournalData(DriveInfo drive, USN_JOURNAL_DATA ntfsUsnJournalData)
        {
            this.Drive = drive;
            this.UsnJournalID = ntfsUsnJournalData.UsnJournalID;
            this.FirstUsn = ntfsUsnJournalData.FirstUsn;
            this.NextUsn = ntfsUsnJournalData.NextUsn;
            this.LowestValidUsn = ntfsUsnJournalData.LowestValidUsn;
            this.MaxUsn = ntfsUsnJournalData.MaxUsn;
            this.MaximumSize = ntfsUsnJournalData.MaximumSize;
            this.AllocationDelta = ntfsUsnJournalData.AllocationDelta;
        }

        // pesudo-code for checking valid USN journal
        //private bool IsUsnJournalValid()
        //{

        //    bool isValid = true;
        //    //
        //    // is the JournalID from the previous state == JournalID from current state?
        //    //
        //    if (_previousUsnState.UsnJournalID == _currentUsnState.UsnJournalID)
        //    {
        //        //
        //        // is the next usn to process still available
        //        //
        //        if (_previousUsnState.NextUsn > _currentUsnState.FirstUsn && _previousUsnState.NextUsn < _currentUsnState.NextUsn)
        //        {
        //            isValid = true;
        //        }
        //        else
        //        {
        //            isValid = false;
        //        }
        //    }
        //    else
        //    {
        //        isValid = false;
        //    }

        //    return isValid;
        //}
    }
    #endregion

    #region UsnControlCode
    public static class UsnControlCode
    {
        private const UInt32 FILE_DEVICE_FILE_SYSTEM = 0x00000009;
        private const UInt32 METHOD_NEITHER = 3;
        private const UInt32 METHOD_BUFFERED = 0;
        private const UInt32 FILE_ANY_ACCESS = 0;
        private const UInt32 FILE_SPECIAL_ACCESS = 0;
        private const UInt32 FILE_READ_ACCESS = 1;
        private const UInt32 FILE_WRITE_ACCESS = 2;

        // FSCTL_QUERY_USN_JOURNAL = CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 61, METHOD_BUFFERED, FILE_ANY_ACCESS)
        public const UInt32 FSCTL_QUERY_USN_JOURNAL = (FILE_DEVICE_FILE_SYSTEM << 16) | (FILE_ANY_ACCESS << 14) | (61 << 2) | METHOD_BUFFERED;

        // FSCTL_ENUM_USN_DATA = CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 44,  METHOD_NEITHER, FILE_ANY_ACCESS)
        public const UInt32 FSCTL_ENUM_USN_DATA = (FILE_DEVICE_FILE_SYSTEM << 16) | (FILE_ANY_ACCESS << 14) | (44 << 2) | METHOD_NEITHER;

        // FSCTL_CREATE_USN_JOURNAL = CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 57,  METHOD_NEITHER, FILE_ANY_ACCESS)
        public const UInt32 FSCTL_CREATE_USN_JOURNAL = (FILE_DEVICE_FILE_SYSTEM << 16) | (FILE_ANY_ACCESS << 14) | (57 << 2) | METHOD_NEITHER;

        // FSCTL_READ_USN_JOURNAL = CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 46,  METHOD_NEITHER, FILE_ANY_ACCESS)
        public const UInt32 FSCTL_READ_USN_JOURNAL = (FILE_DEVICE_FILE_SYSTEM << 16) | (FILE_ANY_ACCESS << 14) | (46 << 2) | METHOD_NEITHER;

        // FSCTL_DELETE_USN_JOURNAL = CTL_CODE(FILE_DEVICE_FILE_SYSTEM, 62, METHOD_BUFFERED, FILE_ANY_ACCESS)
        public const UInt32 FSCTL_DELETE_USN_JOURNAL = (FILE_DEVICE_FILE_SYSTEM << 16) | (FILE_ANY_ACCESS << 14) | (62 << 2) | METHOD_BUFFERED;
    }

    #endregion
    #region UsnErrorCode
    [Flags]
    public enum UsnErrorCode
    {
        SUCCESS = 0,
        ERROR_INVALID_FUNCTION = 0x1,
        ERROR_INVALID_PARAMETER = 0x57,
        ERROR_JOURNAL_DELETE_IN_PROGRESS = 0x49A,
        ERROR_JOURNAL_NOT_ACTIVE = 0x49B
    }
    #endregion
    #region UsnReasonCode
    [Flags]
    public enum UsnReasonCode : uint
    {
        USN_REASON_DATA_OVERWRITE = 0x00000001,
        USN_REASON_DATA_EXTEND = 0x00000002,
        USN_REASON_DATA_TRUNCATION = 0x00000004,
        USN_REASON_NAMED_DATA_OVERWRITE = 0x00000010,
        USN_REASON_NAMED_DATA_EXTEND = 0x00000020,
        USN_REASON_NAMED_DATA_TRUNCATION = 0x00000040,
        USN_REASON_FILE_CREATE = 0x00000100,
        USN_REASON_FILE_DELETE = 0x00000200,
        USN_REASON_EA_CHANGE = 0x00000400,
        USN_REASON_SECURITY_CHANGE = 0x00000800,
        USN_REASON_RENAME_OLD_NAME = 0x00001000,
        USN_REASON_RENAME_NEW_NAME = 0x00002000,
        USN_REASON_INDEXABLE_CHANGE = 0x00004000,
        USN_REASON_BASIC_INFO_CHANGE = 0x00008000,
        USN_REASON_HARD_LINK_CHANGE = 0x00010000,
        USN_REASON_COMPRESSION_CHANGE = 0x00020000,
        USN_REASON_ENCRYPTION_CHANGE = 0x00040000,
        USN_REASON_OBJECT_ID_CHANGE = 0x00080000,
        USN_REASON_REPARSE_POINT_CHANGE = 0x00100000,
        USN_REASON_STREAM_CHANGE = 0x00200000,
        USN_REASON_CLOSE = 0x80000000
    }
    #endregion



    public class UsnOperator : IDisposable
    {
        protected USN_JOURNAL_DATA ntfsUsnJournalData;

        public DriveInfo Drive
        {
            get;
            private set;
        }

        public string DriveLetter
        {
            get
            {
                return this.Drive.Name.TrimEnd(new char[] { '\\', ':' });
            }
        }

        public IntPtr DriveRootHandle
        {
            get;
            private set;
        }

        public UsnOperator(DriveInfo drive)
        {
            if (string.Compare(drive.DriveFormat, "ntfs", true) != 0)
            {
                throw new ArgumentException("USN journal only exists in NTFS drive.");
            }

            this.Drive = drive;
            this.DriveRootHandle = this.GetRootHandle();
            this.ntfsUsnJournalData = new USN_JOURNAL_DATA();
        }

        public UsnJournalData GetUsnJournal()
        {
            UsnErrorCode usnErrorCode = this.QueryUSNJournal();

            UsnJournalData result = null;

            if (usnErrorCode == UsnErrorCode.SUCCESS)
            {
                result = new UsnJournalData(this.Drive, this.ntfsUsnJournalData);
            }

            return result;
        }

        public bool CreateUsnJournal(UInt64 maximumSize = 0x10000000, UInt64 allocationDelta = 0x100000)
        {
            uint bytesReturnedCount;

            var createUsnJournalData = new CREATE_USN_JOURNAL_DATA();
            createUsnJournalData.MaximumSize = maximumSize;
            createUsnJournalData.AllocationDelta = allocationDelta;

            int sizeCujd = Marshal.SizeOf(createUsnJournalData);

            IntPtr cujdBuffer = GetHeapGlobalPtr(sizeCujd);

            Marshal.StructureToPtr(createUsnJournalData, cujdBuffer, true);
            
            bool isSuccess = Win32Api.DeviceIoControl(
                this.DriveRootHandle,
                UsnControlCode.FSCTL_CREATE_USN_JOURNAL,
                cujdBuffer,
                sizeCujd,
                IntPtr.Zero,
                0,
                out bytesReturnedCount,
                IntPtr.Zero);

            Marshal.FreeHGlobal(cujdBuffer);

            return isSuccess;
        }

        public List<UsnEntry> GetEntries()
        {
            var result = new List<UsnEntry>();

            UsnErrorCode usnErrorCode = this.QueryUSNJournal();

            if (usnErrorCode == UsnErrorCode.SUCCESS)
            {
                MFT_ENUM_DATA mftEnumData = new MFT_ENUM_DATA();
                mftEnumData.StartFileReferenceNumber = 0;
                mftEnumData.LowUsn = 0;
                mftEnumData.HighUsn = this.ntfsUsnJournalData.NextUsn;

                int sizeMftEnumData = Marshal.SizeOf(mftEnumData);

                IntPtr ptrMftEnumData = GetHeapGlobalPtr(sizeMftEnumData);

                Marshal.StructureToPtr(mftEnumData, ptrMftEnumData, true);

                int ptrDataSize = sizeof(UInt64) + 10000;

                IntPtr ptrData = GetHeapGlobalPtr(ptrDataSize);

                uint outBytesCount;

                while (false != Win32Api.DeviceIoControl(
                    this.DriveRootHandle,
                    UsnControlCode.FSCTL_ENUM_USN_DATA,
                    ptrMftEnumData,
                    sizeMftEnumData,
                    ptrData,
                    ptrDataSize,
                    out outBytesCount,
                    IntPtr.Zero))
                {
                    // ptrData includes following struct:
                    //typedef struct
                    //{
                    //    USN             LastFileReferenceNumber;
                    //    USN_RECORD_V2   Record[1];
                    //} *PENUM_USN_DATA;
                    
                    IntPtr ptrUsnRecord = new IntPtr(ptrData.ToInt32() + sizeof(Int64));

                    while (outBytesCount > 60)
                    {
                        var usnRecord = new USN_RECORD_V2(ptrUsnRecord);
                        
               
                        result.Add(new UsnEntry(usnRecord));
       
                        ptrUsnRecord = new IntPtr(ptrUsnRecord.ToInt32() + usnRecord.RecordLength);

                        outBytesCount -= usnRecord.RecordLength;
                    }

                    Marshal.WriteInt64(ptrMftEnumData, Marshal.ReadInt64(ptrData, 0));
                }

                Marshal.FreeHGlobal(ptrData);
                Marshal.FreeHGlobal(ptrMftEnumData);
            }

            return result;
        }

        private static IntPtr GetHeapGlobalPtr(int size)
        {
            IntPtr buffer = Marshal.AllocHGlobal(size);
            Win32Api.ZeroMemory(buffer, size);

            return buffer;
        }

        private UsnErrorCode QueryUSNJournal()
        {
            int sizeUsnJournalData = Marshal.SizeOf(this.ntfsUsnJournalData);

            USN_JOURNAL_DATA tempUsnJournalData;

            uint bytesReturnedCount;

            bool isSuccess = Win32Api.DeviceIoControl(
                this.DriveRootHandle,
                UsnControlCode.FSCTL_QUERY_USN_JOURNAL,
                IntPtr.Zero,
                0,
                out tempUsnJournalData,
                sizeUsnJournalData,
                out bytesReturnedCount,
                IntPtr.Zero);

            this.ntfsUsnJournalData = tempUsnJournalData;

            //if (isSuccess)
            //{
            //    return tempUsnJournalData;
            //}
            //else
            //{
            //int win32ErrorCode = Marshal.GetLastWin32Error();
            //if (Enum.IsDefined(typeof(UsnErrorCode), win32ErrorCode))
            //{
            //    var usnErrorCode = (UsnErrorCode)win32ErrorCode;
            //}

            //    throw new IOException("Drive returned false for Query Usn Journal", new Win32Exception(win32ErrorCode));
            //}

            return (UsnErrorCode)Marshal.GetLastWin32Error();
        }

        private IntPtr GetRootHandle()
        {
            string volume = string.Format("\\\\.\\{0}:", this.DriveLetter);

            var result = Win32Api.CreateFile(
                volume,
                Win32ApiConstant.GENERIC_READ,
                Win32ApiConstant.FILE_SHARE_READ | Win32ApiConstant.FILE_SHARE_WRITE,
                IntPtr.Zero,
                Win32ApiConstant.OPEN_EXISTING,
                0,
                IntPtr.Zero);

            if (result.ToInt32() == Win32ApiConstant.INVALID_HANDLE_VALUE)
            {
                throw new IOException("Drive returned invalid root handle", new Win32Exception(Marshal.GetLastWin32Error()));
            }

            return result;
        }

        public void Dispose()
        {
            if (this.DriveRootHandle != null && this.DriveRootHandle.ToInt32() != Win32ApiConstant.INVALID_HANDLE_VALUE)
            {
                Win32Api.CloseHandle(this.DriveRootHandle);
            }
        }
    }
}
