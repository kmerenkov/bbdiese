using System.Collections.Generic;
using System.Web; /* HttpUtility */


namespace BBDiese
{
    public abstract class BaseTagHandler
    {
        protected BaseTagHandler() {}

        public abstract string Process(Tag tag);
    }

    sealed internal class RootTag:BaseTagHandler
    {
        public RootTag() {}

        public override string Process(Tag tag)
        {
            return tag == null ? "" : tag.Content;
        }
    }
}