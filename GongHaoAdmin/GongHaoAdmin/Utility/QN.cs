using Qiniu.IO.Model;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GongHaoAdmin.Utility
{
    public class QN
    {
#if DEBUG
        public const string IMGSRC = "http://7xiyf4.com1.z0.glb.clouddn.com";
        private const string AK = "ABIJNSgcg6vGrFBUvkwnk8GMinp9vHSSaDlGMrrt";
        private const string SK = "ckurM2tEcJaUafpCrleFMNdCGRMiR8sJenCpoq9v";
        public const string BUCKET = "zm123";
#else
        public const string IMGSRC = "http://7xiyf4.com1.z0.glb.clouddn.com";
        private const string AK = "ABIJNSgcg6vGrFBUvkwnk8GMinp9vHSSaDlGMrrt";
        private const string SK = "ckurM2tEcJaUafpCrleFMNdCGRMiR8sJenCpoq9v";
        public const string BUCKET = "zm123";
#endif

        /// <summary>
        /// 获取七牛上传凭证
        /// </summary>
        /// <param name="bucket">七牛空间</param>
        /// <param name="seconds">凭证有效时长</param>
        public static string GetUploadToken(string bucket, string key, int seconds = 3600)
        {
            // 上传策略
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.SetExpires(seconds); // 上传策略的过期时间(单位:秒)
            putPolicy.MimeLimit = "image/gif;image/jpeg;image/jpg;image/png"; // 表示只允许上传图片类型
            putPolicy.Scope = bucket + ":" + key; // 设置要上传的目标空间
            putPolicy.InsertOnly = 0;

            Mac mac = new Mac(AK, SK);

            return Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
        }

        /// <summary>
        /// 公众号logo
        /// </summary>
        /// <param name="GZHId">公号ID</param>
        /// <param name="timeStamp">时间戳</param>
        public static string GZHLogo(int GZHId)
        {
            var str = Tools.DateTimeToTimeStamp(DateTime.Now);
            var timeStamp = str.Substring(0, str.IndexOf('.'));
            return "LOGO/" + GZHId + "/" + timeStamp + ".jpg";
        }

        /// <summary>
        /// 漫画logo
        /// </summary>
        /// <param name="GZHId">公号ID</param>
        /// <param name="MHId">漫画ID</param>
        /// <param name="timeStamp">时间戳</param>
        public static string MHLogo(int GZHId, int MHId)
        {
            var str = Tools.DateTimeToTimeStamp(DateTime.Now);
            var timeStamp = str.Substring(0, str.IndexOf('.'));
            return "MH/" + GZHId + "/" + MHId + "/" + timeStamp + ".jpg";
        }

        /// <summary>
        /// 漫画图片
        /// </summary>
        /// <param name="GZHId">公号ID</param>
        /// <param name="MHId">漫画ID</param>
        public static string MHimg(int GZHId, int MHId)
        {
            var str = Tools.DateTimeToTimeStamp(DateTime.Now);
            var timeStamp = str.Substring(0, str.IndexOf('.'));
            return "IMG/" + GZHId + "/" + MHId + "/" + timeStamp + ".jpg";
        }
    }
}