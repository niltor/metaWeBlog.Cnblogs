using System;
using System.Collections.Generic;
using System.Text;
using Ater.MetaWeBlog;

namespace Ater.MetaWeblog.Options
{
    public class OsChinaOption : ClientOption
    {
        //https://my.oschina.net/u/5701576

        public OsChinaOption(string blogname, string username, string password)
        {
            //base.MetaWeblogURL = "https://rpc.cnblogs.com/metaweblog/" + blogname;
            base.MetaWeblogURL = "https://my.oschina.net/action/xmlrpc" + blogname;
            base.BlogURL = "https://my.oschina.net/blog";
            base.Username = username;
            base.Password = password;
        }
    }
}
