namespace Specs.Utilities
{
    public class HtmlElementDataSelector
    {
        public string CssSelector { get; set; }

        public virtual HtmlElementDataType ElementDataType { get; set; }

        public string PageDataName { get; set; }
    }

    public class HtmlElementAttributeDataSelector : HtmlElementDataSelector
    {
        public override HtmlElementDataType ElementDataType { get {return HtmlElementDataType.Attribute;} }
        public string AttributeName { get; set; }
    }


    public enum HtmlElementDataType
    {
        InnerText,
        Value,
        Attribute
    }
}