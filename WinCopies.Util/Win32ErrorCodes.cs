namespace WinCopies.Util
{
    public enum Win32ErrorCodes
    {

        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        ERROR_SUCCESS = 0x0, // 0

        /// <summary>
        /// Incorrect function.
        /// </summary>
        ERROR_INVALID_FUNCTION = 0x1, // 1

        /// <summary>
        /// The system cannot find the file specified.
        /// </summary>
        ERROR_FILE_NOT_FOUND = 0x2, // 2

        /// <summary>
        /// The system cannot find the path specified.
        /// </summary>
        ERROR_PATH_NOT_FOUND = 0x3, // 3

        /// <summary>
        /// The system cannot open the file.
        /// </summary>
        ERROR_TOO_MANY_OPEN_FILES = 0x4, // 4

        /// <summary>
        /// Access is denied.
        /// </summary>
        ERROR_ACCESS_DENIED = 0x5, // 5

        /// <summary>
        /// The handle is invalid.
        /// </summary>
        ERROR_INVALID_HANDLE = 0x6, // 6

        /// <summary>
        /// The storage control blocks were destroyed.
        /// </summary>
        ERROR_ARENA_TRASHED = 0x7, // 7

        /// <summary>
        /// Not enough storage is available to process this command.
        /// </summary>
        ERROR_NOT_ENOUGH_MEMORY = 0x8, // 8

        /// <summary>
        /// The storage control block address is invalid.
        /// </summary>
        ERROR_INVALID_BLOCK = 0x9, // 9

        /// <summary>
        /// The environment is incorrect.
        /// </summary>
        ERROR_BAD_ENVIRONMENT = 0xA, // 10

        /// <summary>
        /// An attempt was made to load a program with an incorrect format.
        /// </summary>
        ERROR_BAD_FORMAT = 0xB, // 11

        /// <summary>
        /// The access code is invalid.
        /// </summary>
        ERROR_INVALID_ACCESS = 0xC, // 12

        /// <summary>
        /// The data is invalid.
        /// </summary>
        ERROR_INVALID_DATA = 0xD, // 13

        /// <summary>
        /// Not enough storage is available to complete this operation.
        /// </summary>
        ERROR_OUTOFMEMORY = 0xE, // 14

        /// <summary>
        /// The system cannot find the drive specified.
        /// </summary>
        ERROR_INVALID_DRIVE = 0xF, // 15

        /// <summary>
        /// The directory cannot be removed.
        /// </summary>
        ERROR_CURRENT_DIRECTORY = 0x10, // 16

        /// <summary>
        /// The system cannot move the file to a different disk drive.
        /// </summary>
        ERROR_NOT_SAME_DEVICE = 0x11, // 17

        /// <summary>
        /// There are no more files.
        /// </summary>
        ERROR_NO_MORE_FILES = 0x12, // 18

        /// <summary>
        /// The media is write protected.
        /// </summary>
        ERROR_WRITE_PROTECT = 0x13, // 19

        /// <summary>
        /// The system cannot find the device specified.
        /// </summary>
        ERROR_BAD_UNIT = 0x14, // 20

        /// <summary>
        /// The device is not ready.
        /// </summary>
        ERROR_NOT_READY = 0x15, // 21

        /// <summary>
        /// The device does not recognize the command.
        /// </summary>
        ERROR_BAD_COMMAND = 0x16, // 22

        /// <summary>
        /// Data error (cyclic redundancy check).
        /// </summary>
        ERROR_CRC = 0x17, // 23

        /// <summary>
        /// The program issued a command but the command length is incorrect.
        /// </summary>
        ERROR_BAD_LENGTH = 0x18, // 24

        /// <summary>
        /// The drive cannot locate a specific area or track on the disk.
        /// </summary>
        ERROR_SEEK = 0x19, // 25

        /// <summary>
        /// The specified disk or diskette cannot be accessed.
        /// </summary>
        ERROR_NOT_DOS_DISK = 0x1A, // 26

        /// <summary>
        /// The drive cannot find the sector requested.
        /// </summary>
        ERROR_SECTOR_NOT_FOUND = 0x1B, // 27

        /// <summary>
        /// The printer is out of paper.
        /// </summary>
        ERROR_OUT_OF_PAPER = 0x1C, // 28

        /// <summary>
        /// The system cannot write to the specified device.
        /// </summary>
        ERROR_WRITE_FAULT = 0x1D, // 29

        /// <summary>
        /// The system cannot read from the specified device.
        /// </summary>
        ERROR_READ_FAULT = 0x1E, // 30

        /// <summary>
        /// A device attached to the system is not functioning.
        /// </summary>
        ERROR_GEN_FAILURE = 0x1F, // 31

        /// <summary>
        /// The process cannot access the file because it is being used by another process
        /// </summary>
        ERROR_SHARING_VIOLATION = 0x20, // 32

        /// <summary>
        /// The process cannot access the file because another process has locked a portion of the file.
        /// </summary>
        ERROR_LOCK_VIOLATION = 0x21, // 33

        /// <summary>
        /// The wrong diskette is in the drive. Insert %2 (Volume Serial Number: %3) into drive %1.
        /// </summary>
        ERROR_WRONG_DISK = 0x22, // 34

        /// <summary>
        /// Too many files opened for sharing.
        /// </summary>
        ERROR_SHARING_BUFFER_EXCEEDED = 0x24, // 36

        /// <summary>
        /// Reached the end of the file.
        /// </summary>
        ERROR_HANDLE_EOF = 0x26, // 38

        /// <summary>
        /// The disk is full.
        /// </summary>
        ERROR_HANDLE_DISK_FULL = 0x27, // 39

        /// <summary>
        /// The request is not supported.
        /// </summary>
        ERROR_NOT_SUPPORTED = 0x32, // 50

        /// <summary>
        /// Windows cannot find the network path. Verify that the network path is correct and the destination computer is not busy or turned off. If Windows still cannot find the network path, contact your network administrator.
        /// </summary>
        ERROR_REM_NOT_LIST = 0x33, // 51

        /// <summary>
        /// You were not connected because a duplicate name exists on the network. If joining a domain, go to System in Control Panel to change the computer name and try again. If joining a workgroup, choose another workgroup name.
        /// </summary>
        ERROR_DUP_NAME = 0x34, // 52

        /// <summary>
        /// The network path was not found.
        /// </summary>
        ERROR_BAD_NETPATH = 0x35, // 53

        /// <summary>
        /// The network is busy.
        /// </summary>
        ERROR_NETWORK_BUSY = 0x36, // 54

        /// <summary>
        /// The specified network resource or device is no longer available.
        /// </summary>
        ERROR_DEV_NOT_EXIST = 0x37, // 55

        /// <summary>
        /// The network BIOS command limit has been reached.
        /// </summary>
        ERROR_TOO_MANY_CMDS = 0x38, // 56

        /// <summary>
        /// A network adapter hardware error occurred.
        /// </summary>
        ERROR_ADAP_HDW_ERR = 0x39, // 57

        /// <summary>
        /// The specified server cannot perform the requested operation.
        /// </summary>
        ERROR_BAD_NET_RESP = 0x3A, // 58

        /// <summary>
        /// An unexpected network error occurred.
        /// </summary>
        ERROR_UNEXP_NET_ERR = 0x3B, // 59

        /// <summary>
        /// The remote adapter is not compatible.
        /// </summary>
        ERROR_BAD_REM_ADAP = 0x3C, // 60

        /// <summary>
        /// The printer queue is full.
        /// </summary>
        ERROR_PRINTQ_FULL = 0x3D, // 61

        /// <summary>
        /// Space to store the file waiting to be printed is not available on the server.
        /// </summary>
        ERROR_NO_SPOOL_SPACE = 0x3E, // 62

        /// <summary>
        /// Your file waiting to be printed was deleted.
        /// </summary>
        ERROR_PRINT_CANCELLED = 0x3F, // 63

        /// <summary>
        /// The specified network name is no longer available.
        /// </summary>
        ERROR_NETNAME_DELETED = 0x40, // 64

        /// <summary>
        /// Network access is denied.
        /// </summary>
        ERROR_NETWORK_ACCESS_DENIED = 0x41, // 65

        /// <summary>
        /// The network resource type is not correct.
        /// </summary>
        ERROR_BAD_DEV_TYPE = 0x42, // 66

        /// <summary>
        /// The network name cannot be found.
        /// </summary>
        ERROR_BAD_NET_NAME = 0x43, // 67

        /// <summary>
        /// The name limit for the local computer network adapter card was exceeded.
        /// </summary>
        ERROR_TOO_MANY_NAMES = 0x44, // 68

        /// <summary>
        /// The network BIOS session limit was exceeded.
        /// </summary>
        ERROR_TOO_MANY_SESS = 0x45, // 69

        /// <summary>
        /// The remote server has been paused or is in the process of being started.
        /// </summary>
        ERROR_SHARING_PAUSED = 0x46, // 70

        /// <summary>
        /// No more connections can be made to this remote computer at this time because there are already as many connections as the computer can accept.
        /// </summary>
        ERROR_REQ_NOT_ACCEP = 0x47, // 71

        /// <summary>
        /// The specified printer or disk device has been paused.
        /// </summary>
        ERROR_REDIR_PAUSED = 0x48, // 72

        /// <summary>
        /// The file exists.
        /// </summary>
        ERROR_FILE_EXISTS = 0x50, // 80

        ERROR_CANNOT_MAKE=0x52,// 82

        ERROR_FAIL_I24 = 0x53, // 83

        ERROR_OUT_OF_STRUCTURES = 0x54, // 84

        ERROR_ALREADY_ASSIGNED = 0x55, // 85

        ERROR_INVALID_PASSWORD = 0x56, // 86

        ERROR_INVALID_PARAMETER = 0x57, // 87

        ERROR_NET_WRITE_FAULT = 0x58, // 88

        ERROR_NO_PROC_SLOTS = 0x59, // 89

        ERROR_TOO_MANY_SEMAPHORES = 0x64, // 100

        ERROR_EXCL_SEM_ALREADY_OWNED = 0x65, // 101

        ERROR_SEM_IS_SET = 0x66, // 102

        ERROR_TOO_MANY_SEM_REQUESTS = 0x67, // 103

        ERROR_INVALID_AT_INTERRUPT_TIME = 0x68, // 104

        ERROR_SEM_OWNER_DIED = 0x69, // 105

        /// <summary>
        /// The file name is too long.
        /// </summary>
        ERROR_BUFFER_OVERFLOW = 0x6F, // 111

        /// <summary>
        /// There is not enough space on the disk.
        /// </summary>
        ERROR_DISK_FULL = 0x70, // 112

        /// <summary>
        /// The filename, directory name, or volume label syntax is incorrect.
        /// </summary>
        ERROR_INVALID_NAME = 0x7B, // 123

        /// <summary>
        /// Cannot create a file when that file already exists.
        /// </summary>
        ERROR_ALREADY_EXISTS = 0xB7, // 183

        /// <summary>
        /// The filename or extension is too long.
        /// </summary>
        ERROR_FILENAME_EXCED_RANGE = 0xCE, // 206

        /// <summary>
        /// This file is checked out or locked for editing by another user.
        /// </summary>
        ERROR_FILE_CHECKED_OUT = 0xDC, // 220

        /// <summary>
        /// The file size exceeds the limit allowed and cannot be saved.
        /// </summary>
        ERROR_FILE_TOO_LARGE = 0xDF, // 223

        /// <summary>
        /// The directory name is invalid.
        /// </summary>
        ERROR_DIRECTORY = 0x10B, // 267

        /// <summary>
        /// The volume is too fragmented to complete this operation.
        /// </summary>
        ERROR_DISK_TOO_FRAGMENTED = 0x12E, // 302

        /// <summary>
        /// The file cannot be opened because it is in the process of being deleted.
        /// </summary>
        ERROR_DELETE_PENDING = 0x12F, // 303

        /// <summary>
        /// Operation is not allowed on a file system public file.
        /// </summary>
        ERROR_NOT_ALLOWED_ON_SYSTEM_FILE = 0x139, // 313

        /// <summary>
        /// An attempt was made to send down the command via an invalid path to the target device.
        /// </summary>
        ERROR_BAD_DEVICE_PATH = 0x14A, // 330

        /// <summary>
        /// The request was aborted.
        /// </summary>
        ERROR_REQUEST_ABORTED = 0x4D3, // 1235

        /// <summary>
        /// The specified file could not be encrypted.
        /// </summary>
        ERROR_ENCRYPTION_FAILED = 0x1770 // 6000


    }
}
