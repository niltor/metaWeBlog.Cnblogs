using System;
using System.Collections.Generic;

namespace Ater.MetaWeBlog.Models
{
    public class PostInfo
    {
        public List<string> Categories = new List<string>();

        public string Title
        {
            get;
            set;
        }

        public string Link
        {
            get;
            set;
        }

        public DateTime? DateCreated
        {
            get;
            set;
        }

        public string PostID
        {
            get;
            set;
        }

        public string UserID
        {
            get;
            set;
        }

        public int CommentCount
        {
            get;
            set;
        }

        public string PostStatus
        {
            get;
            set;
        }

        public string PermaLink
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public static void Serialize(PostInfo[] posts, string filename)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(PostInfo[]));
            System.IO.StreamWriter textWriter = new System.IO.StreamWriter(filename);
            serializer.Serialize(textWriter, posts);
            textWriter.Close();
        }

        public static PostInfo[] Deserialize(string filename)
        {
            System.IO.StreamReader fp = System.IO.File.OpenText(filename);
            System.Xml.Serialization.XmlSerializer posts_serializer = new System.Xml.Serialization.XmlSerializer(typeof(PostInfo[]));
            PostInfo[] loaded_posts = (PostInfo[])posts_serializer.Deserialize(fp);
            fp.Close();
            return loaded_posts;
        }
    }
}