using System;
using System.Collections.Generic;
using System.Linq;
using Ater.MetaWeBlog.Models;
using Ater.MetaWeBlog.XmlRPC;

namespace Ater.MetaWeBlog
{
    public class Client
    {
        public string AppKey = "0123456789ABCDEF";
        public ClientOption clientOption;

        public Client(ClientOption connectionInfo)
        {
            clientOption = connectionInfo;
        }

        public List<PostInfo> GetRecentPosts(int numposts)
        {
            Service service = new Service(clientOption.MetaWeblogURL);

            MethodCall method = new MethodCall("metaWeblog.getRecentPosts");
            method.Parameters.Add(clientOption.BlogID);
            method.Parameters.Add(clientOption.Username);
            method.Parameters.Add(clientOption.Password);
            method.Parameters.Add(numposts);

            service.Cookies = clientOption.Cookies;

            MethodResponse response = service.Execute(method);

            Value param = response.Parameters[0];
            XmlRPC.Array array = (XmlRPC.Array)param;

            List<PostInfo> items = new List<PostInfo>();
            foreach (Value value in array)
            {
                Struct struct_ = (Struct)value;

                PostInfo postinfo = new PostInfo
                {
                    Title = struct_.Get("title", StringValue.NullString).String,
                    DateCreated = struct_.Get<DateTimeValue>("dateCreated").Data,
                    Link = struct_.Get("link", StringValue.NullString).String,
                    PostID = struct_.Get("postid", StringValue.NullString).String,
                    UserID = struct_.Get("userid", StringValue.NullString).String,
                    CommentCount = struct_.Get<IntegerValue>("commentCount", 0).Integer,
                    PostStatus = struct_.Get("post_status", StringValue.NullString).String,
                    PermaLink = struct_.Get("permaLink", StringValue.NullString).String,
                    Description = struct_.Get("description", StringValue.NullString).String
                };

                items.Add(postinfo);
            }
            return items;
        }

        public MediaObjectInfo NewMediaObject(string name, string type, byte[] bits)
        {
            Service service = new Service(clientOption.MetaWeblogURL);

            Struct input_struct_ = new Struct();
            input_struct_["name"] = new StringValue(name);
            input_struct_["type"] = new StringValue(type);
            input_struct_["bits"] = new Base64Data(bits);

            MethodCall method = new MethodCall("metaWeblog.newMediaObject");
            method.Parameters.Add(clientOption.BlogID);
            method.Parameters.Add(clientOption.Username);
            method.Parameters.Add(clientOption.Password);
            method.Parameters.Add(input_struct_);

            service.Cookies = clientOption.Cookies;

            MethodResponse response = service.Execute(method);
            Value param = response.Parameters[0];
            Struct struct_ = (Struct)param;

            MediaObjectInfo mediaobject = new MediaObjectInfo
            {
                URL = struct_.Get("url", StringValue.NullString).String
            };

            return mediaobject;
        }

        public PostInfo GetPost(string postid)
        {
            Service service = new Service(clientOption.MetaWeblogURL);

            MethodCall method = new MethodCall("metaWeblog.getPost");
            method.Parameters.Add(postid);
            method.Parameters.Add(clientOption.Username);
            method.Parameters.Add(clientOption.Password);

            service.Cookies = clientOption.Cookies;

            MethodResponse response = service.Execute(method);
            Value param = response.Parameters[0];
            Struct struct_ = (Struct)param;

            PostInfo postinfo = new PostInfo
            {
                //item.Categories 
                PostID = struct_.Get<IntegerValue>("postid").ToString(),
                Description = struct_.Get<StringValue>("description").String,
                //item.Tags
                Link = struct_.Get("link", StringValue.NullString).String,
                DateCreated = struct_.Get<DateTimeValue>("dateCreated").Data,
                PermaLink = struct_.Get("permaLink", StringValue.NullString).String,
                PostStatus = struct_.Get("post_status", StringValue.NullString).String,
                Title = struct_.Get<StringValue>("title").String,
                UserID = struct_.Get("userid", StringValue.NullString).String
            };

            XmlRPC.Array rawCats = struct_.Get<XmlRPC.Array>("categories");

            rawCats.ToList().ForEach(i =>
            {
                if (i is StringValue)
                {
                    string cat = (i as StringValue).String;

                    if (cat != "" && !postinfo.Categories.Contains(cat))
                    {
                        postinfo.Categories.Add(cat);
                    }
                }
            });

            return postinfo;
        }

        public string NewPost(PostInfo pi, IList<string> categories, bool publish = true)
        {
            return NewPost(pi.Title, pi.Description, categories, pi.DateCreated, publish);
        }

        public string NewPost(string title, string description, IList<string> categories, DateTime? date_created, bool publish = true)
        {
            XmlRPC.Array cats = null;

            if (categories == null)
            {
                cats = new XmlRPC.Array(0);
            }
            else
            {
                cats = new XmlRPC.Array(categories.Count);

                List<Value> ss = new List<Value>();

                categories.Select(c => new StringValue(c)).ToList().ForEach(i => ss.Add(i));

                cats.AddRange(ss);
            }

            Service service = new Service(clientOption.MetaWeblogURL);

            Struct struct_ = new Struct();
            struct_["title"] = new StringValue(title);
            struct_["description"] = new StringValue(description);
            struct_["categories"] = cats;
            if (date_created.HasValue)
            {
                struct_["dateCreated"] = new DateTimeValue(date_created.Value);
                struct_["date_created_gmt"] = new DateTimeValue(date_created.Value.ToUniversalTime());

            }
            MethodCall method = new MethodCall("metaWeblog.newPost");
            method.Parameters.Add(clientOption.BlogID);
            method.Parameters.Add(clientOption.Username);
            method.Parameters.Add(clientOption.Password);
            method.Parameters.Add(struct_);
            method.Parameters.Add(publish);

            service.Cookies = clientOption.Cookies;

            MethodResponse response = service.Execute(method);
            Value param = response.Parameters[0];
            string postid = ((StringValue)param).String;

            return postid;
        }

        public bool DeletePost(string postid)
        {
            Service service = new Service(clientOption.MetaWeblogURL);

            MethodCall method = new MethodCall("blogger.deletePost");
            method.Parameters.Add(AppKey);
            method.Parameters.Add(postid);
            method.Parameters.Add(clientOption.Username);
            method.Parameters.Add(clientOption.Password);
            method.Parameters.Add(true);

            service.Cookies = clientOption.Cookies;

            MethodResponse response = service.Execute(method);

            Value param = response.Parameters[0];
            BooleanValue success = (BooleanValue)param;

            return success.Boolean;
        }

        public List<BlogInfo> GetUsersBlogs()
        {
            Service service = new Service(clientOption.MetaWeblogURL);

            MethodCall method = new MethodCall("blogger.getUsersBlogs");
            method.Parameters.Add(AppKey);
            method.Parameters.Add(clientOption.Username);
            method.Parameters.Add(clientOption.Password);

            service.Cookies = clientOption.Cookies;

            MethodResponse response = service.Execute(method);
            XmlRPC.Array list = (XmlRPC.Array)response.Parameters[0];

            // 设置blogId
            List<BlogInfo> blogs = new List<BlogInfo>(list.Count);

            for (int i = 0; i < list.Count; i++)
            {
                Struct struct_ = (Struct)list[i];

                BlogInfo boginfo = new BlogInfo
                {
                    BlogID = struct_.Get("blogid", StringValue.NullString).String,
                    URL = struct_.Get("url", StringValue.NullString).String,
                    BlogName = struct_.Get("blogName", StringValue.NullString).String,
                    IsAdmin = struct_.Get<BooleanValue>("isAdmin", false).Boolean,
                    SiteName = struct_.Get("siteName", StringValue.NullString).String,
                    Capabilities = struct_.Get("capabilities", StringValue.NullString).String,
                    XmlRPCEndPoint = struct_.Get("xmlrpc", StringValue.NullString).String
                };

                blogs.Add(boginfo);
            }
            clientOption.BlogID = blogs?.FirstOrDefault()?.BlogID;
            return blogs;
        }

        public bool EditPost(string postid, string title, string description, IList<string> categories, bool publish)
        {
            XmlRPC.Array categories_ = new XmlRPC.Array(categories == null ? 0 : categories.Count);

            if (categories != null)
            {
                List<string> sorted = categories.Distinct().ToList();

                sorted.Sort();

                List<Value> ss = new List<Value>();

                sorted.Select(c => new StringValue(c)).ToList().ForEach(i => ss.Add(i));

                categories_.AddRange(ss);
            }

            Service service = new Service(clientOption.MetaWeblogURL);
            Struct struct_ = new Struct();

            struct_["title"] = new StringValue(title);
            struct_["description"] = new StringValue(description);
            struct_["categories"] = categories_;

            MethodCall method = new MethodCall("metaWeblog.editPost");
            method.Parameters.Add(postid);
            method.Parameters.Add(clientOption.Username);
            method.Parameters.Add(clientOption.Password);
            method.Parameters.Add(struct_);
            method.Parameters.Add(publish);

            service.Cookies = clientOption.Cookies;

            MethodResponse response = service.Execute(method);
            Value param = response.Parameters[0];
            BooleanValue success = (BooleanValue)param;

            return success.Boolean;
        }

        public List<CategoryInfo> GetCategories()
        {
            Service service = new Service(clientOption.MetaWeblogURL);

            MethodCall method = new MethodCall("metaWeblog.getCategories");
            method.Parameters.Add(clientOption.BlogID);
            method.Parameters.Add(clientOption.Username);
            method.Parameters.Add(clientOption.Password);
            service.Cookies = clientOption.Cookies;
            MethodResponse response = service.Execute(method);

            Value param = response.Parameters[0];
            XmlRPC.Array array = (XmlRPC.Array)param;

            List<CategoryInfo> items = new List<CategoryInfo>();
            foreach (Value value in array)
            {
                Struct struct_ = (Struct)value;

                CategoryInfo catinfo = new CategoryInfo
                {
                    Title = struct_.Get("title", StringValue.NullString).String,
                    Description = struct_.Get("description", StringValue.NullString).String,
                    HTMLURL = struct_.Get("htmlUrl", StringValue.NullString).String,
                    RSSURL = struct_.Get("rssUrl", StringValue.NullString).String,
                    CategoryID = struct_.Get("categoryid", StringValue.NullString).String
                };

                items.Add(catinfo);
            }
            return items;
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="name"></param>
        /// <param name="parent_id"></param>
        /// <param name="description"></param>
        /// <param name="slug"></param>
        /// <returns></returns>
        public int NewCatalog(string name, int parent_id, string description = null, string slug = null)
        {
            Service service = new Service(clientOption.MetaWeblogURL);

            Struct struct_ = new Struct();
            struct_["name"] = new StringValue(name);
            struct_["parent_id"] = new IntegerValue(parent_id);

            if (slug != null)
                struct_["slug"] = new StringValue(slug);

            if (description != null)
                struct_["description"] = new StringValue(description);

            MethodCall method = new MethodCall("wp.newCategory");
            method.Parameters.Add(clientOption.BlogID);
            method.Parameters.Add(clientOption.Username);
            method.Parameters.Add(clientOption.Password);
            method.Parameters.Add(struct_);

            service.Cookies = clientOption.Cookies;

            MethodResponse response = service.Execute(method);

            Value param = response.Parameters[0];
            var success = (IntegerValue)param;
            return success.Integer;
        }

        public UserInfo GetUserInfo()
        {
            Service service = new Service(clientOption.MetaWeblogURL);

            MethodCall method = new MethodCall("blogger.getUserInfo");
            method.Parameters.Add(AppKey);
            method.Parameters.Add(clientOption.Username);
            method.Parameters.Add(clientOption.Password);

            service.Cookies = clientOption.Cookies;

            MethodResponse response = service.Execute(method);
            Value param = response.Parameters[0];
            Struct struct_ = (Struct)param;
            UserInfo item = new UserInfo
            {
                UserID = struct_.Get("userid", StringValue.NullString).String,
                Nickname = struct_.Get("nickname", StringValue.NullString).String,
                FirstName = struct_.Get("firstname", StringValue.NullString).String,
                LastName = struct_.Get("lastname", StringValue.NullString).String,
                URL = struct_.Get("url", StringValue.NullString).String
            };

            return item;
        }
    }
}