# MetaWeBlog.Cnblogs

Cnblogs MetaWeBlog，博客园博客同步迁移库。

可通过该类库管理博客园中的博客。或完成相关同步或迁移的功能。

## 注意事项

博客园新增博客间隔最少30秒。

## 使用示例

```csharp
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

```
