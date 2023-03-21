using System;
using System.Collections.Generic;
using Ater.MetaWeBlog;
using Ater.MetaWeBlog.Options;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            // 使用cnblogs
            var option = new CnBlogsOption("blogname", "username", "pat");
            // 使用完整url
            //var option = new ClientOption("blogurl", "metaweblogurl", "blogname", "username", "pat");
            var client = new Client(option);

            // 获取你的blogId
            var blogs = client.GetUsersBlogs();
            // 获取分类
            var categories = client.GetCategories();
            // 调用添加博客方法,这里是写分类名称，而不是分类的id，具体值根据上一步获取的分类信息中提取。
            var addToCategories = new List<string> { "[网站分类].Net" };
            // 发布新的文章
            var result = client.NewPost("title", "description", addToCategories, DateTime.Now, true);

            Console.WriteLine(result);

            Console.ReadLine();
        }
    }
}
