using System;
using System.Collections.Generic;
using System.Linq;
using Ater.MetaWeblog.Options;
using Ater.MetaWeBlog;
using Ater.MetaWeBlog.Models;
using Ater.MetaWeBlog.Options;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            TestOsChina();

            // 使用cnblogs
            var option = new CnBlogsOption("", "", "");
            // 使用完整url
            //var option = new ClientOption("blogurl", "metaweblogurl", "blogname", "username", "pat");
            var client = new Client(option);

            // 获取你的blogId
            var blogs = client.GetUsersBlogs();
            // 获取分类
            var categories = client.GetCategories();
            // 调用添加博客方法,这里是写分类名称，而不是分类的id，具体值根据上一步获取的分类信息中提取。
            var addToCategories = new List<string> { "" };
            // 发布新的文章
            var result = client.NewPost("title", "description", addToCategories, DateTime.Now, true);

            Console.WriteLine(result);

            Console.ReadLine();
        }


        static void TestOsChina()
        {
            var option = new OsChinaOption("", "17076007855", "54NilTor");
            var client = new Client(option);
            var blogs = client.GetUsersBlogs();


            var categories = client.GetCategories();
            var addToCategories = new List<string> { "12508039","工作日志" };
            var result = client.NewPost("仅测试", "测试", addToCategories, DateTime.Now, true);
            Console.WriteLine(  );
        }
    }
}
