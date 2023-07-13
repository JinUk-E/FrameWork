using System;
using System.Collections;
using System.IO;
using Mine.Framework.Extension;
using UnityEngine;
#if SUPPORT_EXCEL
using ExcelDataReader;
using System.Data;
#endif

namespace Mine.Framework.Util
{
    static class DebugerEx
    {
        #region enum
        public enum DebugType
        {
            Log,
            LogWarning,
            LogError
        }

        #endregion

        #region public
        public static void Logger(string message,DebugType debugType)
        {
            if(!Application.isEditor) return;
            switch (debugType)
            {
                case DebugType.Log:
                    Log(message);
                    break;
                case DebugType.LogWarning:
                    LogWarning(message);
                    break;
                case DebugType.LogError:
                    LogError(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(debugType), debugType, "This DebugType is not supported.");
            }
        }
        #endregion
        
        #region private

        private static void Log(string message) => Debug.Log(message);
        private static void LogWarning(string message) => Debug.LogWarning(message);
        private static void LogError(string message) => Debug.LogError(message);

        #endregion
    }
    
    static class IOEx
    {
        #region private

        private static FileStream GetFileStream(
            string filePath, 
            FileMode mode = FileMode.Open, 
            FileAccess access = FileAccess.Read, 
            FileShare share = FileShare.Read) 
            => File.Open(filePath, mode, access, share);


        #endregion

        #region public

        public static void CreateTextFileInFolder(string path, string fileName, string text)
        {
            var filePath = $"{path}/{fileName}.txt";
            var fileStream = GetFileStream(filePath, !File.Exists(filePath) ? FileMode.CreateNew : FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            var streamWriter = new StreamWriter((Stream) fileStream);
            streamWriter.WriteLine(text);
            streamWriter.Close();
            fileStream.Close();
        }
        
#if SUPPORT_EXCEL
        /// <summary>
        /// Call Data to ExcelFile
        /// </summary>
        /// <param name="filePath"> Excel file path </param>
        /// <param name="useHeader"> how to use columns Header </param>
        /// <returns></returns>
        public static DataSet GetExcelDataSet(string filePath,bool useHeader = true)
        {
            using var fileStream = IOEx.GetFileStream(filePath);
            using var reader = ExcelReaderFactory.CreateReader(fileStream);
            return reader.AsDataSet(new ExcelDataSetConfiguration() //데이터셋 변환하기
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    //Column 자동생성을 무시하고 첫번째 행을 열로 자동 지정.
                    UseHeaderRow = useHeader,
                }
            });//데이터셋 변환하기
        }
#endif
        #endregion
       
    }
    
    static class WaitEx
    {
        #region public
        // wait for action
        public static IEnumerator WaitForAction(float waitTime, Action action)
        {
            Debug.Log($"WaitTime : {waitTime}".Color(Color.red));
            yield return new WaitForSeconds(waitTime); // wait
            Debug.Log("Start Action".Color(Color.green));
            action?.Invoke(); // null check
        }
        #endregion
    }
}