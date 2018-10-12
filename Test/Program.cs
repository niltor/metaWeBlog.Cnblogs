using System;
using System.Collections.Generic;
using MSDev.MetaWeblog;
using MSDev.MetaWeBlog.Options;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            var option = new CnBlogsOption("blogname", "username", "password");
            var client = new Client(option);

            // 获取你的blogId
            client.GetUsersBlogs();
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
