using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Vorlesung.Shared.Extensions;

namespace Vorlesung.Shared.Downloads
{
    public class Download
    {
        /// <summary>
        /// Id of the download db document
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; } = Guid.NewGuid().ToString().Replace("-", "");

        /// <summary>
        /// Query this Uid to boost Cosmos Db performance
        /// </summary>
        public string Uid
        {
            get
            {
                return Id;
            }
        }

        /// <summary>
        /// Hash of the Id
        /// </summary>
        public string PartitionKey
        {
            get
            {
                return GetHash(Id);
            }
        }

        /// <summary>
        /// Document created in db at this point in time. Defaults to UtcNow.
        /// </summary>
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// DisplayName of the file (not need for downloading a file
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Name of the physical file with extension.
        /// The download will be named so if the value is not null.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Extension of the physical file.
        /// Used as partition key
        /// </summary>
        public string FileExtension
        {
            get
            {
                if (string.IsNullOrEmpty(Filename))
                    return string.Empty;

                return Path.GetExtension(Filename);
            }
        }

        /// <summary>
        /// Name of the stored Blob
        /// </summary>
        public string BlobName { get; set; }
        /// <summary>
        /// Url to the Blob
        /// </summary>
        public string Url { get; set; }
        
        /// <summary>
        /// Counts how many downloads of this file happened
        /// </summary>
        public long DownloadCount { get; set; }

        public static string GetHash(string value)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(value);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
