namespace eSDSCom.Editor.Shared.DocumentElements;
public class Enums
{
    public enum DataPointOccurence
    {
        /// < xsd:element name = "A" />   means A is required and must appear exactly once.
        RequiredExactlyOnce = 1,

        ///xsd:element name="A" minOccurs="0"/> means A is optional and may appear at most once.
        OptionalZeroOrOne = 2,

        /// <xsd:element name="A" maxOccurs="unbounded"/> means A is required and may repeat an unlimited number of times.
        RequiredOnceOrMore = 3,

        /// <xsd:element name="A" minOccurs="0" maxOccurs="unbounded"/> means A is optional and may repeat an unlimited number of times.
        OptionalZeroOrMore = 4
    }
}
