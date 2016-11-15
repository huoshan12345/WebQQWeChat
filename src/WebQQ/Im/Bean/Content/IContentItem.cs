using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WebQQ.Im.Bean.Content
{
    public interface IContentItem
    {
        ContentItemType Type { get; }
        object ToJson();
        string GetText();
    }
}
