using System.Xml;

namespace AtTask.OutlookAddIn.Utilities
{
    public static class XmlUtil
    {
        /// <summary>
        /// Returns trimmed inner text of given parent element's child element with given name.
        /// If there is no such element retuns null.
        /// </summary>
        /// <param name="parentElement"></param>
        /// <param name="elemName"></param>
        /// <returns></returns>
        public static string ReadElementValue(XmlElement parentElement, string elemName)
        {
            XmlElement elem = (XmlElement)parentElement.SelectSingleNode(elemName);
            if (elem != null)
            {
                return elem.InnerText.Trim();
            }

            return null;
        }
    }
}
