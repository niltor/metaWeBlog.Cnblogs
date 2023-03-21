# MetaWeblog
.NET MetaWeblog实现，可使用该库同步支持该协议的博客，如`cnBlogs`。

项目基于 MetaWeblogSharp 修改而成。
## 注意事项

博客园新增博客间隔最少30秒。

## 安装

 [![NuGet](https://img.shields.io/nuget/v/MSDev.MetaWeBlog.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/MSDev.MetaWeBlog/)

## 使用示例

```csharp
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
    }

```
