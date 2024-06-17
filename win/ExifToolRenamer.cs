using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace PhotoRenamer
{
    class ExifToolRenamer : Renamer
    {
        public const string ExifToolFile = "exiftool.exe";
        public readonly string appDir;
        private StringBuilder logOutputBuffer;

        public ExifToolRenamer(string appDir, string dir, Log log, bool recursive)
            : base(dir, log, recursive)
        {
            this.appDir = appDir;
        }

        public override void Rename()
        {
            this.logOutputBuffer = new StringBuilder();

            Process process = new Process();
            ProcessStartInfo pInfo = process.StartInfo;

            // 不在批处理中，%不需要用%%转义
            // exiftool "-FileName<CreateDate" -d "%Y%m%d_%H%M%S%%-c.%%e" -r %1
            pInfo.FileName = ExifToolFile;

            String args = "\"-FileName<CreateDate\" -d \"%Y%m%d_%H%M%S%%-c.%%e\" {0} \"{1}\"";
            pInfo.Arguments = String.Format(args, (this.recursive ? "-r" : string.Empty), this.dir);

            pInfo.UseShellExecute = false;
            pInfo.RedirectStandardOutput = true;
            pInfo.RedirectStandardError = true;
            pInfo.WorkingDirectory = appDir;
            pInfo.ErrorDialog = true;
            pInfo.CreateNoWindow = true;
            pInfo.WindowStyle = ProcessWindowStyle.Hidden;

            /*
             * http://stackoverflow.com/questions/139593/processstartinfo-hanging-on-waitforexit-why
             * The problem is that if you redirect StandardOutput and/or StandardError the internal buffer can become full. 
             * Whatever order you use, there can be a problem:
             * 1. If you wait for the process to exit before reading StandardOutput the process can block trying to write to it, 
             * so the process never ends.
             * 2. If you read from StandardOutput using ReadToEnd then your process can block if the process never closes 
             * StandardOutput (for example if it never terminates, or if it is blocked writing to StandardError).
            */
            StringBuilder outputBuffer = new StringBuilder();
            process.OutputDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);
            process.ErrorDataReceived += new DataReceivedEventHandler(process_OutputDataReceived);

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            this.log("Finished.");

            this.log(String.Empty);

            this.log(this.logOutputBuffer.ToString());
        }

        private string TranslateResult(String result)
        {
            string chineseResult = result;
            // 1 directories scanned
            // 0 image files updated
            // 11 image files unchanged

            chineseResult = chineseResult.Replace("directories scanned", "个文件夹被扫描");
            chineseResult = chineseResult.Replace("image files updated", "张照片被改名");
            chineseResult = chineseResult.Replace("image files read", "张照片被扫描");
            chineseResult = chineseResult.Replace("image files unchanged", "张照片文件名不变" + Environment.NewLine);

            return chineseResult;
        }

        private void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                this.logOutputBuffer.AppendLine(e.Data);
            }
        }
    }
}
