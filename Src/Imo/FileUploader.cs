// Decompiled with JetBrains decompiler
// Type: ImoSilverlightApp.FileUploader
// Assembly: ImoSilverlightApp, Version=1.2.7.0, Culture=neutral, PublicKeyToken=null
// MVID: 9B58C21E-A008-4E09-9485-1CED6EA5C267
// Assembly location: C:\Users\Admin\Desktop\RE\ImoXap\ImoSilverlightApp.dll

using ImoSilverlightApp.Helpers;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace ImoSilverlightApp
{
  internal class FileUploader
  {
    private const int MAX_FILE_SIZE = 52428800;
    private const int UPLOAD_CHUNK_SIZE = 256;
    private HashSet<string> cancelRequests = new HashSet<string>();
    private static readonly Logger log = LogManager.GetLogger(typeof (FileUploader).Name);
    private const string UPLOAD_ENDPOINT = "fu/imofileuploader";

    private bool IsCanceled(string uploadId)
    {
      lock (this.cancelRequests)
        return this.cancelRequests.Contains(uploadId);
    }

    public Action UploadFile(
      string filePath,
      Action<string, JObject> callback = null,
      Action<double> progressCallback = null)
    {
      FileUploader.log.Info("Uploading file:" + filePath);
      string uploadId = Utils.GetRandomString(8);
      string uid = IMO.User.Uid;
      string ssid = IMO.Dispatcher.GetSSID();
      if (uid == null)
      {
        callback("uid_null", (JObject) null);
        return (Action) null;
      }
      string uploadUrl = string.Format("https://{0}:{1}/{2}/{3}/{4}/", (object) IMO.Network.ConnectionData.WarpyHost, (object) IMO.Network.ConnectionData.WarpyPort, (object) "fu/imofileuploader", (object) IMO.Dispatcher.GetSSID(), (object) Utils.GetRandomString(8));
      try
      {
        FileInfo fileInfo = new FileInfo(filePath);
        if (fileInfo.Length > 52428800L)
        {
          callback("file_too_big", (JObject) null);
          return (Action) null;
        }
        if (fileInfo.Length == 0L)
        {
          callback("file_empty", (JObject) null);
          return (Action) null;
        }
        this.InternalUploadFile(fileInfo, uid, ssid, uploadUrl, (Action<JObject>) (result =>
        {
          if (result == null)
          {
            if (this.IsCanceled(uploadId))
            {
              lock (this.cancelRequests)
                this.cancelRequests.Remove(uploadId);
              callback((string) null, (JObject) null);
            }
            else
              callback("error", (JObject) null);
          }
          else
            callback((string) null, result);
        }), uploadId, progressCallback);
      }
      catch (Exception ex)
      {
        FileUploader.log.Error(ex, "Error uploading file", 120, nameof (UploadFile));
        callback("error", (JObject) null);
      }
      return (Action) (() =>
      {
        lock (this.cancelRequests)
        {
          IMO.MonitorLog.Log("send_files", "send_cancel");
          this.cancelRequests.Add(uploadId);
        }
      });
    }

    private async void InternalUploadFile(
      FileInfo fileInfo,
      string uid,
      string ssid,
      string uploadUrl,
      Action<JObject> callback,
      string uploadId,
      Action<double> progressCallback)
    {
      byte[] bytes = await FSUtils.ReadFileAsync(fileInfo.FullName);
      try
      {
        if (bytes == null)
        {
          FileUploader.log.Error("Read no bytes while uplpoading file", 142, nameof (InternalUploadFile));
          callback((JObject) null);
        }
        else
        {
          HttpWebResponse httpWebResponse = await FileUploader.FormUpload.MultipartFormDataPost(uploadUrl, new Dictionary<string, object>()
          {
            {
              "file_name",
              (object) fileInfo.Name
            },
            {
              nameof (uid),
              (object) uid
            },
            {
              "proto",
              (object) "imo"
            },
            {
              nameof (ssid),
              (object) ssid
            },
            {
              "file_data",
              (object) new FileUploader.FormUpload.FileParameter(bytes, fileInfo.Name, "application/octet-stream")
            }
          }, uploadId, progressCallback);
          if (httpWebResponse == null)
            callback((JObject) null);
          if (callback == null)
            return;
          string end = new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd();
          httpWebResponse.Close();
          if (httpWebResponse.StatusCode == HttpStatusCode.OK)
          {
            FileUploader.log.Info("Succesfully uploaded file: " + fileInfo.FullName);
            callback(JObject.Parse(end));
          }
          else
          {
            FileUploader.log.Error("Upload file unsuccessful: " + end, 177, nameof (InternalUploadFile));
            callback((JObject) null);
          }
        }
      }
      catch (Exception ex)
      {
        FileUploader.log.Error(ex, "Uploading file", 184, nameof (InternalUploadFile));
        callback((JObject) null);
      }
    }

    private static class FormUpload
    {
      private static readonly Encoding encoding = Encoding.UTF8;

      public static Task<HttpWebResponse> MultipartFormDataPost(
        string postUrl,
        Dictionary<string, object> postParameters,
        string uploadId,
        Action<double> progressCallback)
      {
        string boundary = string.Format("----------{0:N}", (object) Guid.NewGuid());
        string contentType = "multipart/form-data; boundary=" + boundary;
        byte[] multipartFormData = FileUploader.FormUpload.GetMultipartFormData(postParameters, boundary);
        return FileUploader.FormUpload.PostForm(postUrl, contentType, multipartFormData, uploadId, progressCallback);
      }

      private static async Task<HttpWebResponse> PostForm(
        string postUrl,
        string contentType,
        byte[] formData,
        string uploadId,
        Action<double> progressCallback)
      {
        if (!(WebRequest.Create(postUrl) is HttpWebRequest request))
          throw new NullReferenceException("request is not a http request");
        request.Method = "POST";
        request.ContentType = contentType;
        request.UserAgent = UserAgentHelper.GetUA();
        request.Headers[HttpRequestHeader.Cookie] = string.Format("iat={0};UDID={1}", (object) IMO.ApplicationSettings.Cookie, (object) IMO.ApplicationSettings.Udid);
        request.ContentLength = (long) formData.Length;
        using (Stream requestStreamAsync = await request.GetRequestStreamAsync())
        {
          int num = (formData.Length - 1) / 256 + 1;
          for (int index = 0; index < num; ++index)
          {
            requestStreamAsync.Write(formData, index * 256, Math.Min(formData.Length - index * 256, 256));
            if (IMO.FileUploader.IsCanceled(uploadId))
              return (HttpWebResponse) null;
            if (progressCallback != null && index < num - 1)
            {
              double progress = (double) ((index + 1) * 256) / (double) formData.Length;
              Utils.BeginInvokeOnUI((Action) (() => progressCallback(progress)));
            }
          }
        }
        return await request.GetResponseAsync() as HttpWebResponse;
      }

      private static byte[] GetMultipartFormData(
        Dictionary<string, object> postParameters,
        string boundary)
      {
        Stream stream = (Stream) new MemoryStream();
        bool flag = false;
        foreach (KeyValuePair<string, object> postParameter in postParameters)
        {
          if (flag)
            stream.Write(FileUploader.FormUpload.encoding.GetBytes("\r\n"), 0, FileUploader.FormUpload.encoding.GetByteCount("\r\n"));
          flag = true;
          if (postParameter.Value is FileUploader.FormUpload.FileParameter)
          {
            FileUploader.FormUpload.FileParameter fileParameter = (FileUploader.FormUpload.FileParameter) postParameter.Value;
            string s = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n", (object) boundary, (object) postParameter.Key, (object) (fileParameter.FileName ?? postParameter.Key), (object) (fileParameter.ContentType ?? "application/octet-stream"));
            stream.Write(FileUploader.FormUpload.encoding.GetBytes(s), 0, FileUploader.FormUpload.encoding.GetByteCount(s));
            stream.Write(fileParameter.Bytes, 0, fileParameter.Bytes.Length);
          }
          else
          {
            string s = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}", (object) boundary, (object) postParameter.Key, postParameter.Value);
            stream.Write(FileUploader.FormUpload.encoding.GetBytes(s), 0, FileUploader.FormUpload.encoding.GetByteCount(s));
          }
        }
        string s1 = "\r\n--" + boundary + "--\r\n";
        stream.Write(FileUploader.FormUpload.encoding.GetBytes(s1), 0, FileUploader.FormUpload.encoding.GetByteCount(s1));
        stream.Position = 0L;
        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer, 0, buffer.Length);
        stream.Close();
        return buffer;
      }

      public class FileParameter
      {
        public byte[] Bytes { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public FileParameter(byte[] bytes)
          : this(bytes, (string) null)
        {
        }

        public FileParameter(byte[] bytes, string fileName)
          : this(bytes, fileName, (string) null)
        {
        }

        public FileParameter(byte[] bytes, string fileName, string contenttype)
        {
          this.Bytes = bytes;
          this.FileName = fileName;
          this.ContentType = contenttype;
        }
      }
    }
  }
}
