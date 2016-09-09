using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

namespace Common.Extensions
{
    // From http://blogs.msdn.com/rfarber/archive/2008/05/30/class-extensions-for-xlinq.aspx
    public static class XElementExtensions
    {
        //-- GetAttributeValue --
        public static string GetAttributeValue(this XElement xElement, XName attName, string defaultReturn)
        {
            XAttribute att = xElement.Attribute(attName);
            if (att == null) return defaultReturn;
            return att.Value;
        }

        public static string GetAttributeValue(this XElement xElement, XName attName)
        {
            return xElement.GetAttributeValue(attName, String.Empty);
        }        
        
        public static T GetAttributeValue<T>(this XElement xElement, XName attName, T defaultReturn)
        {
            string returnValue = xElement.GetAttributeValue(attName, String.Empty);
            if (returnValue == String.Empty) return defaultReturn;
            return (T)Convert.ChangeType(returnValue, typeof(T), CultureInfo.InvariantCulture);
        }

        public static T GetAttributeValue<T>(this XElement xElement, XName attName)
        {
            return xElement.GetAttributeValue<T>(attName, default(T));
        }

        public static bool GetAttributeValueAsBoolean(this XElement xElement, string attribute, bool defaultValue = false)
        {
            XAttribute xa = xElement.Attribute(attribute);

            return ((xa != null) ?
                   ((xa.Value == "1" ||
                     xa.Value.ToLowerInvariant() == "true" ||
                     xa.Value.ToLowerInvariant() == "yes") ?
                   true : false) : defaultValue);
        }

        //-- GetElementValue --
        public static string GetElementValue(this XElement xElement, XName elName, string defaultValue = "", string defaultValueForEmpty = "")
        {
            XElement el = xElement.Element(elName);
            if (el == null) return defaultValue;
            return String.IsNullOrEmpty(el.Value) ? defaultValueForEmpty : el.Value;
        }

        public static T GetElementValue<T>(this XElement xElement, XName elName, T defaultReturn)
        {
            string returnValue = xElement.GetElementValue(elName);
            if (returnValue == String.Empty) return defaultReturn;
            return (T)Convert.ChangeType(returnValue, typeof(T));
        }

        public static T GetElementValue<T>(this XElement xElement, XName elName)
        {
            return xElement.GetElementValue<T>(elName, default(T));
        }

        public static bool GetElementValueAsBoolean(this XElement xElement, string elementName, bool defaultValue = false)
        {
            XElement el = xElement.Element(elementName);

            return ((el != null) ?
                   ((el.Value == "1" ||
                     el.Value.ToLowerInvariant() == "true" ||
                     el.Value.ToLowerInvariant() == "yes") ?
                   true : false) : defaultValue);
        }

        public static int GetElementValueAsInteger(this XElement xElement, string elementName, int defaultValue = 0)
        {
            XElement el = xElement.Element(elementName);

            if (el == null || String.IsNullOrWhiteSpace(el.Value)) return defaultValue;

            int elValue = defaultValue;

            if (Int32.TryParse(el.Value, out elValue))
            {
                return elValue;
            }
            else
            {
                return defaultValue;
            }
        }

        //-- RemoveElements --
        public static void RemoveElements(this XElement xElement)
        {
            foreach(XElement el in xElement.Elements().ToArray()) el.Remove();
        }

        public static void RemoveElements(this XElement xElement, XName name)
        {
            foreach(XElement el in xElement.Elements(name).ToArray()) el.Remove();
        }

        //-- HasAttribute --
        public static bool HasAttribute(this XElement xElement, XName attName)
        {
            return xElement.Attribute(attName) != null;
        }

        public static bool HasElement(this XElement xElement, XName elName)
        {
            return (xElement.Element(elName) != null);
        }

        // This is intended to be a cheaper vesion of an XPath since it can only walk elements.
        public static XElement ElementByPath(this XElement xElement, string path)
        {
            //TODO Optimize this or at least remove the split.
            XElement nextElement = xElement;

            foreach(string elementName in path.Split(new char[] { '/' }))
            {
                nextElement = nextElement.Element(elementName);
                if (nextElement == null)
                    break;
            }

            return nextElement;
        }


        //-- Merge --

        // Merges the contents (including attributes) of e2 into e1 (i.e. e1.Merge(e2)).
        public static void Merge(this XElement e1, XElement e2)
        {
            XElement xMerge = internalMerge(e1, e2);

            e1.RemoveNodes();
            e1.Add(xMerge.Nodes());
        }

        private static XElement internalMerge(XElement e1, XElement e2)
        {
            if (!e1.Name.LocalName.Equals(e2.Name.LocalName))
            {
                XElement newElement = new XElement(e1);
                newElement.Add(e2);
                return newElement;
            }

            var attrComparer = new XAttributeEqualityComparer();

            var attributes = e2.Attributes().Union(e1.Attributes(), attrComparer);

            var elements1 = e1.Elements().ToArray();
            var elements2 = e2.Elements().ToArray();

            var elements = new List<XNode>();
            int i1 = 0, i2 = 0;
            while (i1 < elements1.Length && i2 < elements2.Length)
            {
                XElement e = null;
                int compResult = String.Compare(elements1[i1].Name.LocalName, elements2[i2].Name.LocalName);

                if (compResult < 0 || compResult > 0)
                {
                    e = elements2[i2];
                    i2++;
                }
                else
                {
                    e = internalMerge(elements1[i1], elements2[i2]);
                    i1++;
                    i2++;
                }
                elements.Add(e);
            }
            while (i1 < elements1.Length)
            {
                elements.Add(elements1[i1]);
                i1++;
            }
            while (i2 < elements2.Length)
            {
                elements.Add(elements2[i2]);
                i2++;
            }

            string value = null;
            if (elements.Count == 0)
            {
                // Note: Merges the 2 element "value" here. Grabs the latest !IsNullOrEmpty() value.

                if (!string.IsNullOrEmpty(e1.Value))
                    value = e1.Value;
                if (!string.IsNullOrEmpty(e2.Value))
                    value = e2.Value;
            }
            if (value != null)
                return new XElement(e1.Name, attributes, elements, value);
            else
                return new XElement(e1.Name, attributes, elements);
        }

        private class XAttributeEqualityComparer : IEqualityComparer<XAttribute>
        {
            public bool Equals(XAttribute x, XAttribute y)
            {
                return x.Name == y.Name;
            }

            public int GetHashCode(XAttribute x)
            {
                return x.Name.GetHashCode();
            }
        } 

        //------------------------------------------        

        public static XElement GetRootNode(this XElement xElement)
        {
            XElement rootElement = xElement;
            while (rootElement != null)
            {
                rootElement = rootElement.Parent;
            }
            return rootElement;            
        }        
    }

    public static class XmlReaderExtensions
    {
        public static bool IsElement(this XmlReader xmlReader, string elName)
        {
            return (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == elName);
        }

        public static bool IsEndElement(this XmlReader xmlReader, string elName)
        {
            if (xmlReader.Name == elName)
                return (xmlReader.NodeType == XmlNodeType.EndElement || xmlReader.IsEmptyElement);

            return false;
        }

        //-- Attributes --

        public static string GetAttributeValue(this XmlReader xmlReader, string attName, string defaultReturn)
        {
            string returnValue = xmlReader.GetAttribute(attName);
            if (returnValue == null) return defaultReturn;
            return returnValue;
        }

        public static string GetAttributeValue(this XmlReader xmlReader, string attName)
        {
            return xmlReader.GetAttribute(attName, String.Empty);
        }

        public static T GetAttributeValue<T>(this XmlReader xmlReader, string attName, T defaultReturn)
        {
            string returnValue = xmlReader.GetAttribute(attName, String.Empty);
            if (returnValue == String.Empty) return defaultReturn;
            return (T)Convert.ChangeType(returnValue, typeof(T), CultureInfo.InvariantCulture);
        }

        public static T GetAttributeValue<T>(this XmlReader xmlReader, string attName)
        {
            return xmlReader.GetAttributeValue<T>(attName, default(T));
        }

        //-- Elements --

        public static string GetElementValue(this XmlReader xmlReader, string defaultReturn)
        {
            string returnValue = xmlReader.ReadString();
            if (returnValue == null) return defaultReturn;
            return returnValue;
        }

        public static string GetElementValue(this XmlReader xmlReader)
        {
            return xmlReader.GetElementValue(String.Empty);
        }

        public static T GetElementValue<T>(this XmlReader xmlReader, T defaultReturn)
        {
            string returnValue = xmlReader.GetElementValue(String.Empty);
            if (returnValue == String.Empty) return defaultReturn;
            return (T)Convert.ChangeType(returnValue, typeof(T), CultureInfo.InvariantCulture);
        }

        public static T GetElementValue<T>(this XmlReader xmlReader)
        {
            return xmlReader.GetElementValue<T>(default(T));
        }
    }
}